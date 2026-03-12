<!-- App.vue or MenuOrder.vue -->
<template>
  <div class="font-sans max-w-[480px] mx-auto bg-gray-50 min-h-screen pb-36 relative">

    <!-- Header -->
    <header class="bg-yellow-400 px-4 py-3 flex justify-between items-center font-bold">
      <div class="text-xl text-gray-700">K3NEY</div>
      <div
        class="relative bg-white w-10 h-10 rounded-full flex items-center justify-center cursor-pointer"
        @click="showCart = !showCart"
      >
        <img src="/src/assets/shopping-bag.png" alt="Cart" class="w-5 h-5" />
        <span
          v-if="cartTotalItems > 0"
          class="absolute -top-1 -right-1 bg-red-500 text-white text-xs font-bold px-1.5 py-0.5 rounded-full min-w-[18px] text-center"
        >
          {{ cartTotalItems }}
        </span>
      </div>
    </header>

    <!-- Categories horizontal scroll -->
    <div class="flex overflow-x-auto px-2 py-3 gap-3 bg-white border-b border-gray-100 sticky top-0 z-10">
      <button
        v-for="cat in categories"
        :key="cat.id"
        class="flex-shrink-0 px-4 py-2 rounded-full border font-medium whitespace-nowrap transition-colors"
        :class="currentCategory === cat.id
          ? 'bg-yellow-400 border-yellow-400 text-gray-900'
          : 'bg-white border-gray-200 text-gray-600 hover:border-yellow-300'"
        @click="currentCategory = cat.id"
      >
        {{ cat.name }}
      </button>
    </div>

    <!-- ALL TAB: grouped by category -->
    <template v-if="currentCategory === null">
      <div v-for="(group, key) in groupedMenuItems" :key="key">
        <!-- Category divider -->
        <div class="flex items-center gap-3 px-4 pt-5 pb-2">
          <div class="flex-1 h-px bg-gray-200"></div>
          <span class="font-bold text-gray-700 text-sm">{{ group.name }}</span>
          <div class="flex-1 h-px bg-gray-200"></div>
        </div>
        <!-- Items grid -->
        <div class="grid grid-cols-2 gap-3 px-4 pb-2">
          <div
            v-for="item in group.items"
            :key="item.id"
            class="bg-white rounded-xl overflow-hidden shadow-sm hover:-translate-y-1 transition-transform"
          >
            <div class="h-36 bg-yellow-100 flex items-center justify-center text-5xl">🍔</div>
            <div class="p-2.5">
              <div class="font-semibold text-gray-800 mb-2 text-sm">{{ item.name }}</div>
              <div class="flex justify-between items-center">
                <span class="text-orange-700 font-bold">${{ item.price.toFixed(2) }}</span>
                <button
                  class="bg-yellow-400 hover:bg-yellow-500 active:scale-95 w-9 h-9 rounded-full flex items-center justify-center transition-all"
                  @click.stop="addToCart(item)"
                >
                  <img src="/src/assets/shopping-bag.png" alt="Add" class="w-4 h-4" />
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </template>

    <!-- SPECIFIC CATEGORY TAB: normal grid -->
    <div v-else class="grid grid-cols-2 gap-3 p-4">
      <div
        v-for="item in filteredMenuItems"
        :key="item.id"
        class="bg-white rounded-xl overflow-hidden shadow-sm hover:-translate-y-1 transition-transform"
      >
        <div class="h-36 bg-yellow-100 flex items-center justify-center text-5xl">🍔</div>
        <div class="p-2.5">
          <div class="font-semibold text-gray-800 mb-2 text-sm">{{ item.name }}</div>
          <div class="flex justify-between items-center">
            <span class="text-orange-700 font-bold">${{ item.price.toFixed(2) }}</span>
            <button
              class="bg-yellow-400 hover:bg-yellow-500 active:scale-95 w-9 h-9 rounded-full flex items-center justify-center transition-all"
              @click.stop="addToCart(item)"
            >
              <img src="/src/assets/shopping-bag.png" alt="Add" class="w-4 h-4" />
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Cart bottom sheet -->
    <div class="fixed bottom-0 left-0 right-0 max-w-[480px] mx-auto z-50">
      <div
        v-if="showCart"
        class="bg-white rounded-t-2xl shadow-2xl max-h-[70vh] overflow-y-auto"
      >
        <!-- Cart header -->
        <div class="flex justify-between items-center px-4 py-4 border-b border-gray-100">
          <h3 class="font-bold text-lg text-gray-800">Your Order</h3>
          <button
            class="text-gray-400 hover:text-gray-600 text-2xl leading-none"
            @click="showCart = false"
          >✕</button>
        </div>

        <!-- Cart items -->
        <div class="px-4">
          <div
            v-for="(group, key) in cartGrouped"
            :key="key"
            class="flex justify-between items-center py-3 border-b border-gray-50"
          >
            <div class="text-sm font-medium text-gray-700 flex-1">{{ group.name }}</div>
            <div class="flex items-center gap-3 mx-3">
              <button
                class="w-8 h-8 rounded-full border border-gray-200 bg-white text-lg flex items-center justify-center hover:bg-gray-50"
                @click="changeQuantity(group.item, -1)"
              >-</button>
              <span class="font-semibold w-4 text-center">{{ group.quantity }}</span>
              <button
                class="w-8 h-8 rounded-full border border-gray-200 bg-white text-lg flex items-center justify-center hover:bg-gray-50"
                @click="changeQuantity(group.item, 1)"
              >+</button>
            </div>
            <div class="text-sm font-bold text-orange-700 w-16 text-right">
              ${{ (group.quantity * group.price).toFixed(2) }}
            </div>
          </div>
        </div>

        <!-- Total -->
        <div class="flex justify-between items-center px-4 py-4 text-lg font-bold">
          <span>Total</span>
          <span>${{ cartTotal.toFixed(2) }}</span>
        </div>

        <!-- Confirm button -->
        <button
          class="w-full bg-green-500 hover:bg-green-600 text-white font-bold py-4 text-lg transition-colors"
          @click="placeOrder"
        >
          Confirm Order • ${{ cartTotal.toFixed(2) }}
        </button>
      </div>
    </div>

  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import axios from "axios";

// const API_BASE = 'https://localhost:7295/api/v1'
const API_BASE = "/api/v1";

// State
const categories = ref([]);
const menuItems = ref([]);
const currentCategory = ref(null);
const cart = ref([]);
const showCart = ref(false);

// Computed
const filteredMenuItems = computed(() => {
  if (!currentCategory.value) return menuItems.value;
  return menuItems.value.filter(
    (item) => item.categoryId === currentCategory.value,
  );
});

// Grouped by category for "All" tab
const groupedMenuItems = computed(() => {
  const groups = {};
  categories.value
    .filter((cat) => cat.id !== null)
    .forEach((cat) => {
      const items = menuItems.value.filter((i) => i.categoryId === cat.id);
      if (items.length > 0) {
        groups[cat.id] = { name: cat.name, items };
      }
    });
  return groups;
});

const cartGrouped = computed(() => {
  const map = {};
  cart.value.forEach((ci) => {
    const key = ci.menuItem.id;
    if (!map[key]) {
      map[key] = {
        item: ci.menuItem,
        name: ci.menuItem.name,
        price: ci.price,
        quantity: 0,
      };
    }
    map[key].quantity += ci.quantity;
  });
  return map;
});

const cartTotalItems = computed(() =>
  cart.value.reduce((sum, ci) => sum + ci.quantity, 0),
);

const cartTotal = computed(() =>
  cart.value.reduce((sum, ci) => sum + ci.quantity * ci.price, 0),
);

// Methods
const loadData = async () => {
  try {
    const catRes = await axios.get(`${API_BASE}/MenuCategory`);
    categories.value = [
      { id: null, name: "All" },
      ...(catRes.data.data || []),
    ];
    currentCategory.value = null;

    const itemsRes = await axios.get(`${API_BASE}/MenuItem`);
    menuItems.value = (itemsRes.data.data || []).filter(
      (i) => i.isActive !== false,
    );
  } catch (err) {
    console.error("Failed to load menu", err);
    alert("Cannot load menu. Check API / network.");
  }
};

const addToCart = (menuItem) => {
  const idx = cart.value.findIndex((ci) => ci.menuItem.id === menuItem.id);
  if (idx !== -1) {
    cart.value[idx].quantity += 1;
  } else {
    cart.value.push({
      menuItem: { ...menuItem },
      quantity: 1,
      price: menuItem.price,
    });
  }
};

const changeQuantity = (menuItem, delta) => {
  const idx = cart.value.findIndex((ci) => ci.menuItem.id === menuItem.id);
  if (idx === -1) return;
  cart.value[idx].quantity += delta;
  if (cart.value[idx].quantity <= 0) {
    cart.value.splice(idx, 1);
  }
};

const placeOrder = async () => {
  if (cart.value.length === 0) return;

  const payload = {
    items: cart.value.map((ci) => ({
      menuItemId: ci.menuItem.id,
      quantity: ci.quantity,
      price: ci.price,
    })),
  };

  try {
    const res = await axios.post(`${API_BASE}/Order/Create`, payload);
    alert(`Order placed! #${res.data?.data?.orderNumber || "—"}`);
    cart.value = [];
    showCart.value = false;
  } catch (err) {
    console.error(err);
    alert(
      "Failed to place order\n" + (err.response?.data?.message || err.message),
    );
  }
};

onMounted(() => {
  loadData();
});
</script>