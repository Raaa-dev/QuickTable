<script setup lang="ts">
import { useCartStore } from "../stores/cart";
import axios from "axios";
import FooterMenu from "../components/footer-menu.vue";
import { useRouter } from "vue-router";

const cart = useCartStore();
const router = useRouter();
const API_BASE = "/api/v1";

const placeOrder = async () => {
  if (cart.items.length === 0) return;

  const payload = {
    items: cart.items.map(ci => ({
      menuItemId: ci.menuItem.id,
      quantity: ci.quantity,
      price: ci.price,
    })),
  };

  try {
    const res = await axios.post(`${API_BASE}/Order/Create`, payload);
    const order = res.data?.data;

    // Save to localStorage as cache
    const history = JSON.parse(localStorage.getItem("orderHistory") || "[]");
    history.unshift({
      id: order?.id,
      orderNumber: order?.orderNumber,
      items: [...cart.items],
      total: cart.totalPrice,
      date: new Date().toISOString(),
      status: "Placed",
    });
    localStorage.setItem("orderHistory", JSON.stringify(history));

    cart.clearCart();
    router.push("/history");
  } catch (err: any) {
    alert("Failed: " + (err.response?.data?.message || err.message));
  }
};
</script>

<template>
  <div class="font-sans max-w-[480px] mx-auto bg-gray-50 min-h-screen pb-36">

    <header class="bg-yellow-400 px-4 py-3 font-bold text-xl text-gray-700">
      🛒 Your Order
    </header>

    <!-- Empty -->
    <div v-if="cart.items.length === 0"
      class="flex flex-col items-center justify-center h-64 text-gray-400 gap-3">
      <span class="text-6xl">🍽️</span>
      <p class="font-medium">Your cart is empty</p>
      <router-link to="/" class="text-yellow-500 font-bold text-sm">← Back to Menu</router-link>
    </div>

    <!-- Cart Items -->
    <div v-else class="p-4 flex flex-col gap-3">
      <div v-for="ci in cart.items" :key="ci.menuItem.id"
        class="bg-white rounded-xl p-3 flex items-center gap-3 shadow-sm">
        <div class="w-14 h-14 bg-yellow-100 rounded-lg flex items-center justify-center text-3xl flex-shrink-0">🍔</div>
        <div class="flex-1">
          <div class="font-semibold text-gray-800 text-sm">{{ ci.menuItem.name }}</div>
          <div class="text-orange-600 font-bold text-sm">${{ (ci.price * ci.quantity).toFixed(2) }}</div>
        </div>
        <div class="flex items-center gap-2">
          <button class="w-7 h-7 rounded-full bg-gray-100 flex items-center justify-center font-bold hover:bg-gray-200"
            @click="cart.changeQuantity(ci.menuItem.id, -1)">−</button>
          <span class="w-5 text-center font-bold text-sm">{{ ci.quantity }}</span>
          <button class="w-7 h-7 rounded-full bg-yellow-400 flex items-center justify-center font-bold hover:bg-yellow-500"
            @click="cart.changeQuantity(ci.menuItem.id, 1)">+</button>
        </div>
      </div>

      <!-- Summary + Place Order -->
      <div class="bg-white rounded-xl p-4 shadow-sm mt-2">
        <div class="flex justify-between items-center mb-1 text-sm text-gray-500">
          <span>Items ({{ cart.totalItems }})</span>
          <span>${{ cart.totalPrice.toFixed(2) }}</span>
        </div>
        <div class="flex justify-between items-center mb-4 font-bold text-gray-800">
          <span>Total</span>
          <span class="text-orange-600 text-lg">${{ cart.totalPrice.toFixed(2) }}</span>
        </div>
        <button
          class="w-full bg-yellow-400 hover:bg-yellow-500 active:scale-95 transition-all font-bold py-3 rounded-xl text-gray-800 text-base"
          @click="placeOrder">
          Place Order 🚀
        </button>
      </div>
    </div>

    <FooterMenu />
  </div>
</template>