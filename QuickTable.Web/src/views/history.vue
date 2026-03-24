<script setup>
import { ref, onMounted } from "vue";
import api from "../api/axios";
import FooterMenu from "../components/footer-menu.vue";

const history = ref([]);
const loading = ref(false);

const loadHistory = async () => {
  loading.value = true;

  // 1. Load from localStorage first (instant)
  const cached = JSON.parse(localStorage.getItem("orderHistory") || "[]");
  history.value = cached;

  try {
    // 2. Check if current session is still active
    const sessionId = localStorage.getItem("sessionId");
    if (sessionId) {
      const sessionRes = await api.get('/api/v1/Table/session/${sessionId}');
      const sessionStatus = sessionRes.data?.status;

      // ✅ Session closed by staff → clear history
      if (sessionStatus === "Closed") {
        localStorage.removeItem("orderHistory");
        localStorage.removeItem("cart");
        localStorage.removeItem("sessionId");
        localStorage.removeItem("tableToken");
        localStorage.removeItem("tableNumber");
        history.value = [];
        loading.value = false;
        return;
      }
    }

    // 3. Session still active → fetch latest orders
    const res = await api.get('/api/v1/Order/GetAll');
    const apiOrders = res.data?.data || [];

    history.value = apiOrders.map((order) => ({
      id: order.id,
      orderNumber: order.orderNumber,
      total: order.totalAmount,
      date: order.createdAt,
      status: order.status,
      items: order.orderItems || [],
    }));

    localStorage.setItem("orderHistory", JSON.stringify(history.value));

  } catch (err) {
    console.warn("API failed, using cached history");
  } finally {
    loading.value = false;
  }
};

onMounted(() => loadHistory());
</script>

<template>
  <div class="font-sans max-w-[480px] mx-auto bg-gray-50 min-h-screen pb-36">

    <header class="bg-yellow-400 px-4 py-3 font-bold text-xl text-gray-700">
      📋 Order History
    </header>

    <!-- Loading -->
    <div v-if="loading" class="flex justify-center items-center h-32 text-gray-400">
      Loading...
    </div>

    <!-- Empty -->
    <div v-else-if="history.length === 0"
      class="flex flex-col items-center justify-center h-64 text-gray-400 gap-3">
      <span class="text-6xl">📭</span>
      <p class="font-medium">No orders yet</p>
      <router-link to="/" class="text-yellow-500 font-bold text-sm">← Go order something!</router-link>
    </div>

    <!-- History list -->
    <div v-else class="p-4 flex flex-col gap-3">
      <div v-for="order in history" :key="order.id"
        class="bg-white rounded-xl p-4 shadow-sm">
        <div class="flex justify-between items-center mb-2">
          <span class="font-bold text-gray-800">#{{ order.orderNumber }}</span>
          <span class="text-xs px-2 py-1 rounded-full font-medium"
            :class="order.status === 'Placed' ? 'bg-yellow-100 text-yellow-700' : 'bg-green-100 text-green-700'">
            {{ order.status }}
          </span>
        </div>
        <div class="text-sm text-gray-500 mb-2">
          {{ new Date(order.date).toLocaleString() }}
        </div>
        <div class="text-sm text-gray-600 mb-3 flex flex-wrap gap-1">
          <span v-for="ci in order.items" :key="ci.menuItem?.id || ci.menuItemId"
            class="bg-gray-50 px-2 py-0.5 rounded-full text-xs">
            {{ ci.menuItem?.name || ci.name }} x{{ ci.quantity }}
          </span>
        </div>
        <div class="font-bold text-orange-600">${{ Number(order.total).toFixed(2) }}</div>
      </div>
    </div>

    <FooterMenu />
  </div>
</template>