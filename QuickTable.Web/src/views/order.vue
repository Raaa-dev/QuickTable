<script setup lang="ts">
import { ref } from "vue";
import { useCartStore } from "../stores/cart";
import { useTableStore } from "../stores/table";
import api from "../api/axios";
import FooterMenu from "../components/footer-menu.vue";
import { useRouter } from "vue-router";

const cart = useCartStore();
const router = useRouter();
const tableStore = useTableStore();

const API_BASE = import.meta.env.VITE_API_BASE_URL;
const showSuccess = ref(false);
const showError = ref(false);
const errorMessage = ref("");
const isLoading = ref(false);

const placeOrder = async () => {
  if (cart.items.length === 0) return;

  if (!tableStore.token) {
    errorMessage.value = "No table token found. Please scan the QR code again.";
    showError.value = true;
    setTimeout(() => (showError.value = false), 3000);
    return;
  }
  isLoading.value = true;
  const payload = {
    qrToken: tableStore.token,
    items: cart.items.map((ci) => ({
      menuItemId: ci.menuItem.id,
      quantity: ci.quantity,
    })),
  };

  try {
    const res = await api.post(`/api/v1/Order/Create`, payload);
    const order = res.data?.data;

    const history = JSON.parse(localStorage.getItem("orderHistory") || "[]");
    history.unshift({
      id: order?.id,
      orderNumber: order?.orderNumber,
      items: [...cart.items],
      total: cart.totalPrice,
      date: new Date().toISOString(),
      status: "Placed",
      tableNumber: tableStore.tableNumber,
    });
    localStorage.setItem("orderHistory", JSON.stringify(history));

    cart.clearCart();

    // Show success toast then redirect
    showSuccess.value = true;
    setTimeout(() => {
      showSuccess.value = false;
      router.push("/history");
    }, 2000);
  } catch (err: any) {
    errorMessage.value = err.response?.data?.message || err.message;
    showError.value = true;
    setTimeout(() => (showError.value = false), 3000);
  } finally {
    isLoading.value = false;
  }
};
</script>

<template>
  <div class="font-sans max-w-[480px] mx-auto bg-gray-50 min-h-screen pb-36">
    <!-- Loading -->
    <div
      v-if="isLoading"
      class="fixed inset-0 flex items-center justify-center bg-black/40"
      style="z-index: 9999"
    >
      <span
        class="loading loading-spinner text-warning"
        style="width: 3rem; height: 3rem"
      ></span>
    </div>
    <!-- ✅ Success Toast -->
    <div
      v-if="showSuccess"
      class="fixed top-4 left-1/2 -translate-x-1/2 z-50 w-[90%] max-w-[440px] transition-all"
    >
      <div
        role="alert"
        class="alert alert-success bg-green-50 border border-green-200 rounded-xl px-4 py-3 flex items-center gap-3 shadow-lg"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          class="h-6 w-6 shrink-0 stroke-green-600"
          fill="none"
          viewBox="0 0 24 24"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
          />
        </svg>
        <span class="text-green-700 font-medium"
          >Your order has been placed!</span
        >
      </div>
    </div>

    <!-- ❌ Error Toast -->
    <div
      v-if="showError"
      class="fixed top-4 left-1/2 -translate-x-1/2 z-50 w-[90%] max-w-[440px] transition-all"
    >
      <div
        role="alert"
        class="bg-red-50 border border-red-200 rounded-xl px-4 py-3 flex items-center gap-3 shadow-lg"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          class="h-6 w-6 shrink-0 stroke-red-600"
          fill="none"
          viewBox="0 0 24 24"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z"
          />
        </svg>
        <span class="text-red-700 font-medium">{{ errorMessage }}</span>
      </div>
    </div>

    <header class="bg-yellow-400 px-4 py-3 font-bold text-xl text-gray-700">
      🛒 Your Order
    </header>

    <!-- Empty -->
    <div
      v-if="cart.items.length === 0"
      class="flex flex-col items-center justify-center h-64 text-gray-400 gap-3"
    >
      <span class="text-6xl">🍽️</span>
      <p class="font-medium">Your cart is empty</p>
      <router-link to="/" class="text-yellow-500 font-bold text-sm"
        >← Back to Menu</router-link
      >
    </div>

    <!-- Cart Items -->
    <div v-else class="p-4 flex flex-col gap-3">
      <div
        v-for="ci in cart.items"
        :key="ci.menuItem.id"
        class="bg-white rounded-xl p-3 flex items-center gap-3 shadow-sm"
      >
        <img
          v-if="ci.menuItem.imageUrl"
          :src="`${API_BASE}${ci.menuItem.imageUrl}`"
          :alt="ci.menuItem.name"
          class="w-14 h-14 rounded-lg object-cover flex-shrink-0"
        />
        <div v-else
          class="w-14 h-14 bg-yellow-100 rounded-lg flex items-center justify-center text-3xl flex-shrink-0"
        >
          🍔
        </div>
        <div class="flex-1">
          <div class="font-semibold text-gray-800 text-sm">
            {{ ci.menuItem.name }}
          </div>
          <div class="text-orange-600 font-bold text-sm">
            ${{ (ci.price * ci.quantity).toFixed(2) }}
          </div>
        </div>
        <div class="flex items-center gap-2">
          <button
            class="w-7 h-7 rounded-full bg-gray-100 flex items-center justify-center font-bold hover:bg-gray-200"
            @click="cart.changeQuantity(ci.menuItem.id, -1)"
          >
            −
          </button>
          <span class="w-5 text-center font-bold text-sm">{{
            ci.quantity
          }}</span>
          <button
            class="w-7 h-7 rounded-full bg-yellow-400 flex items-center justify-center font-bold hover:bg-yellow-500"
            @click="cart.changeQuantity(ci.menuItem.id, 1)"
          >
            +
          </button>
        </div>
      </div>

      <!-- Summary + Place Order -->
      <div class="bg-white rounded-xl p-4 shadow-sm mt-2">
        <div
          class="flex justify-between items-center mb-1 text-sm text-gray-500"
        >
          <span>Items ({{ cart.totalItems }})</span>
          <span>${{ cart.totalPrice.toFixed(2) }}</span>
        </div>
        <div
          class="flex justify-between items-center mb-4 font-bold text-gray-800"
        >
          <span>Total</span>
          <span class="text-orange-600 text-lg"
            >${{ cart.totalPrice.toFixed(2) }}</span
          >
        </div>
        <button
          class="w-full bg-yellow-400 hover:bg-yellow-500 active:scale-95 transition-all font-bold py-3 rounded-xl text-gray-800 text-base"
          @click="placeOrder"
        >
          Place Order 🚀
        </button>
      </div>
    </div>

    <FooterMenu />
  </div>
</template>
