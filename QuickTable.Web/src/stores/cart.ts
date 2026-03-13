import { defineStore } from "pinia";
import { ref, computed } from "vue";

export const useCartStore = defineStore("cart", () => {
  const items = ref<any[]>(
    JSON.parse(localStorage.getItem("cart") || "[]")
  );

  const save = () => {
    localStorage.setItem("cart", JSON.stringify(items.value));
  };

  const addItem = (menuItem: any) => {
    const idx = items.value.findIndex(i => i.menuItem.id === menuItem.id);
    if (idx !== -1) {
      items.value[idx].quantity += 1;
    } else {
      items.value.push({ menuItem: { ...menuItem }, quantity: 1, price: menuItem.price });
    }
    save();
  };

  const changeQuantity = (menuItemId: number, delta: number) => {
    const idx = items.value.findIndex(i => i.menuItem.id === menuItemId);
    if (idx === -1) return;
    items.value[idx].quantity += delta;
    if (items.value[idx].quantity <= 0) items.value.splice(idx, 1);
    save();
  };

  const clearCart = () => {
    items.value = [];
    localStorage.removeItem("cart");
  };

  const totalItems = computed(() =>
    items.value.reduce((s, i) => s + i.quantity, 0)
  );
  const totalPrice = computed(() =>
    items.value.reduce((s, i) => s + i.quantity * i.price, 0)
  );

  return { items, addItem, changeQuantity, clearCart, totalItems, totalPrice };
});