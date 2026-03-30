<!-- App.vue or MenuOrder.vue -->
<template>
  <div class="font-sans max-w-[480px] mx-auto bg-gray-50 min-h-screen pb-36 relative">
 
  <section class="sticky top-0 z-10">
    <!-- Header -->
    <header class="bg-yellow-400 px-4 py-3 flex justify-between items-center font-bold">
      <div class="text-xl text-gray-700">Hotpot</div>
        <div v-if="tableStore.tableNumber" class="text-sm text-gray-700 font-medium">
    🪑 {{ tableStore.tableNumber }}
  </div>
      <div
        class="relative bg-white w-10 h-10 rounded-full flex items-center justify-center">
        <img src="/src/assets/store.png" alt="Cart" class="w-8 h-7" />
      </div>
    </header>

    <!-- Categories horizontal scroll -->
    <div class="flex overflow-x-auto px-2 py-3 gap-3 bg-white border-b border-gray-100">
      <button
        v-for="cat in nonEmptyCategories"
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
</section>
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

<!-- ALL TAB grid item -->
      <div
        v-for="item in group.items"
        :key="item.id"
        class="bg-white rounded-2xl overflow-hidden shadow-sm hover:-translate-y-1 transition-transform"
      >
        <div class="relative overflow-hidden" style="height: 170px;">
          <img
            v-if="item.imageUrl"
            :src="`${item.imageUrl}`"
            :alt="item.name"
            class="w-full h-full object-cover transition-transform duration-300 hover:scale-110"
          />
          <div v-else class="w-full h-full bg-yellow-100 flex items-center justify-center text-5xl">🍔</div>

          <!-- gradient + name + price -->
          <div class="absolute bottom-0 left-0 right-0 px-3 pb-3 pt-8 bg-gradient-to-t from-black/65 to-transparent">
            <p class="text-white font-semibold text-sm m-0 leading-tight">{{ item.name }}</p>
            <p class="text-yellow-400 font-bold text-xs m-0">${{ item.price.toFixed(2) }}</p>
          </div>

          <!-- floating add button -->
          <button
            class="absolute bottom-2 right-2 w-9 h-9 rounded-full bg-yellow-400 hover:bg-yellow-500 active:scale-95 flex items-center justify-center transition-all shadow-md border-none cursor-pointer"
            @click="cart.addItem(item)"
          >
            <img src="/src/assets/shopping-bag.png" alt="Add" class="w-4 h-4" />
          </button>
        </div>
      </div>
        </div>
      </div>
    </template>

<!-- SPECIFIC CATEGORY TAB -->
    <div v-else class="grid grid-cols-2 gap-3 p-4">
      <div
        v-for="item in filteredMenuItems"
        :key="item.id"
        class="bg-white rounded-2xl overflow-hidden shadow-sm hover:-translate-y-1 transition-transform"
      >
        <div class="relative overflow-hidden" style="height: 170px;">
          <img
            v-if="item.imageUrl"
            :src="`${item.imageUrl}`"
            :alt="item.name"
            class="w-full h-full object-cover transition-transform duration-300 hover:scale-110"
          />
          <div v-else class="w-full h-full bg-yellow-100 flex items-center justify-center text-5xl">🍔</div>

          <div class="absolute bottom-0 left-0 right-0 px-3 pb-3 pt-8 bg-gradient-to-t from-black/65 to-transparent">
            <p class="text-white font-semibold text-sm m-0 leading-tight">{{ item.name }}</p>
            <p class="text-yellow-400 font-bold text-xs m-0">${{ item.price.toFixed(2) }}</p>
          </div>

          <button
            class="absolute bottom-2 right-2 w-9 h-9 rounded-full bg-yellow-400 hover:bg-yellow-500 active:scale-95 flex items-center justify-center transition-all shadow-md border-none cursor-pointer"
            @click="cart.addItem(item)"
          >
            <img src="/src/assets/shopping-bag.png" alt="Add" class="w-4 h-4" />
          </button>
        </div>
      </div>
    </div>

    <!-- footer menu -->
  <FooterMenu @tab-change="onTabChange"/>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import { useCartStore } from "../stores/cart";
import { useTableStore } from "../stores/table";
import api from "../api/axios";
import FooterMenu from "../components/footer-menu.vue";

// State
const categories = ref([]);
const menuItems = ref([]);
const currentCategory = ref(null);
const showCart = ref(false);
const cart = useCartStore();
const tableStore = useTableStore();
const API_BASE = import.meta.env.VITE_API_BASE_URL;
const loading = ref(false)

const currentTab = ref("menu");
const onTabChange = (tab) => {
  currentTab.value = tab;
};

const nonEmptyCategories = computed(() =>{
  return categories.value.filter(cat =>{
    if(cat.id === null) return true;
    return menuItems.value.some(item => item.categoryId === cat.id);
  })
})
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

// const cartTotalItems = computed(() =>
//   cart.value.reduce((sum, ci) => sum + ci.quantity, 0),
// );

// const cartTotal = computed(() =>
//   cart.value.reduce((sum, ci) => sum + ci.quantity * ci.price, 0),
// );

// Methods
const loadData = async () => {
  loading.value = true;
  try {
    const catRes = await api.get('/api/v1/MenuCategory');
    categories.value = [
      { id: null, name: "All" },
      ...(catRes.data.data || []),
    ];
    currentCategory.value = null;

    const itemsRes = await api.get('/api/v1/MenuItem');
    menuItems.value = (itemsRes.data.data || []).filter(
      (i) => i.isActive !== false,
    );
  } catch (err) {
    console.error("Failed to load menu", err);
    alert("Cannot load menu. Check API / network.");
  }finally{
    loading.value = false
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

// const changeQuantity = (menuItem, delta) => {
//   const idx = cart.value.findIndex((ci) => ci.menuItem.id === menuItem.id);
//   if (idx === -1) return;
//   cart.value[idx].quantity += delta;
//   if (cart.value[idx].quantity <= 0) {
//     cart.value.splice(idx, 1);
//   }
// };

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
    const res = await api.post(`/api/v1/Order/Create`, payload);
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

onMounted(async () => {
  // Check if session was closed by staff
   await tableStore.checkAndClearIfClosed();
  // Read token from URL ?token=xxx
  const urlParams = new URLSearchParams(window.location.search);
  const token = urlParams.get("token");
  if (token) {
    await tableStore.setToken(token);
    // Clean token from URL without reload
    window.history.replaceState({}, "", window.location.pathname);
  }

  loadData();
});
</script>