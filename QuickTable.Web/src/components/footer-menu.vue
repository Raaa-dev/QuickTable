<script setup lang="ts">
import { computed } from "vue";
import { useRouter, useRoute } from "vue-router";
import { useCartStore } from "../stores/cart";

const router = useRouter();
const route = useRoute();
const cart = useCartStore();

const activeTab = computed(() => {
  if (route.path === "/order") return "orders";
  if (route.path === "/history") return "history";
  return "menu";
});

const switchTab = (tab: "menu" | "orders" | "history") => {
  if (tab === "orders") router.push("/order");
  else if (tab === "history") router.push("/history");
  else router.push("/");
};
</script>

<template>
  <div class="fixed bottom-0 left-1/2 -translate-x-1/2 w-full max-w-[480px] bg-white border-t border-gray-100 shadow-[0_-4px_20px_rgba(0,0,0,0.07)] flex z-50">

    <!-- Menu Tab -->
    <button
      class="flex-1 py-3 flex flex-col items-center gap-1 transition-colors"
      :class="activeTab === 'menu' ? 'text-yellow-400 border-t-[2.5px] border-yellow-400' : 'text-gray-400 border-t-[2.5px] border-transparent'"
      @click="switchTab('menu')"
    >
      <svg width="22" height="22" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24">
        <path d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2"/>
      </svg>
      <span class="text-[11px] font-bold tracking-wide">MENU</span>
    </button>

    <!-- Orders Tab with badge -->
    <button
      class="flex-1 py-3 flex flex-col items-center gap-1 transition-colors relative"
      :class="activeTab === 'orders' ? 'text-yellow-400 border-t-[2.5px] border-yellow-400' : 'text-gray-400 border-t-[2.5px] border-transparent'"
      @click="switchTab('orders')"
    >
      <!-- 🔴 Badge -->
      <span
        v-if="cart.totalItems > 0"
        class="absolute top-1.5 right-6 bg-red-500 text-white text-[10px] font-bold min-w-[18px] h-[18px] rounded-full flex items-center justify-center px-1"
      >
        {{ cart.totalItems }}
      </span>

      <svg width="22" height="22" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24">
        <path d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"/>
      </svg>
      <span class="text-[11px] font-bold tracking-wide">ORDERS</span>
    </button>

    <!-- History Tab -->
    <button
      class="flex-1 py-3 flex flex-col items-center gap-1 transition-colors"
      :class="activeTab === 'history' ? 'text-yellow-400 border-t-[2.5px] border-yellow-400' : 'text-gray-400 border-t-[2.5px] border-transparent'"
      @click="switchTab('history')"
    >
      <svg width="22" height="22" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24">
        <path d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"/>
      </svg>
      <span class="text-[11px] font-bold tracking-wide">HISTORY</span>
    </button>

  </div>
</template>