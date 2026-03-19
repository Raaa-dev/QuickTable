<template>
  <div>
    <!-- Loading overlay -->
    <div v-if="loading" class="fixed inset-0 flex items-center justify-center bg-black/40 z-50">
      <span class="loading loading-spinner text-warning" style="width:3rem;height:3rem;"></span>
    </div>

    <!-- Toast -->
    <div v-if="toast.show" class="fixed top-4 left-1/2 -translate-x-1/2 z-50 w-[90%] max-w-[440px]">
      <div :class="toast.type === 'success' ? 'bg-green-50 border-green-200 text-green-700' : 'bg-red-50 border-red-200 text-red-700'"
        class="border rounded-xl px-4 py-3 flex items-center gap-3 shadow-lg">
        <span>{{ toast.type === 'success' ? '✅' : '❌' }}</span>
        <span class="font-medium">{{ toast.message }}</span>
      </div>
    </div>

    <!-- Page header -->
    <div class="flex items-center justify-between mb-6">
      <div>
        <h1 class="text-xl font-bold text-gray-800">Reset Table Sessions</h1>
        <p class="text-sm text-gray-500 mt-0.5">Close active sessions when customers leave</p>
      </div>
      <button class="btn btn-sm btn-outline" @click="load">🔄 Refresh</button>
    </div>

    <!-- Stats row -->
    <div class="grid grid-cols-3 gap-3 mb-6">
      <div class="bg-white rounded-xl p-4 shadow-sm border border-gray-100 text-center">
        <div class="text-2xl font-bold text-gray-800">{{ tables.length }}</div>
        <div class="text-xs text-gray-500 mt-1">Total Tables</div>
      </div>
      <div class="bg-white rounded-xl p-4 shadow-sm border border-gray-100 text-center">
        <div class="text-2xl font-bold text-green-600">{{ activeTables.length }}</div>
        <div class="text-xs text-gray-500 mt-1">Active Sessions</div>
      </div>
      <div class="bg-white rounded-xl p-4 shadow-sm border border-gray-100 text-center">
        <div class="text-2xl font-bold text-gray-400">{{ freeTables.length }}</div>
        <div class="text-xs text-gray-500 mt-1">Free Tables</div>
      </div>
    </div>

    <!-- Reset All button -->
    <div v-if="activeTables.length > 0" class="mb-5">
      <button
        class="w-full bg-red-500 hover:bg-red-600 active:scale-95 transition-all text-white font-bold py-3 rounded-xl flex items-center justify-center gap-2"
        @click="confirmResetAll"
      >
        🗑️ Reset All Active Sessions ({{ activeTables.length }})
      </button>
    </div>

    <!-- Table grid -->
    <div class="grid grid-cols-2 gap-3">
      <div
        v-for="table in tables"
        :key="table.id"
        class="bg-white rounded-2xl p-4 shadow-sm border transition-all"
        :class="table.hasActiveSession ? 'border-green-200' : 'border-gray-100'"
      >
        <!-- Top: badge + number -->
        <div class="flex items-center justify-between mb-3">
          <div
            class="w-14 h-14 rounded-xl flex items-center justify-center font-bold text-xl"
            :class="table.hasActiveSession ? 'bg-green-100 text-green-700' : 'bg-gray-100 text-gray-400'"
          >
            {{ table.tableNumber }}
          </div>
          <span
            class="text-xs font-semibold px-2.5 py-1 rounded-full"
            :class="table.hasActiveSession
              ? 'bg-green-100 text-green-700'
              : 'bg-gray-100 text-gray-400'"
          >
            {{ table.hasActiveSession ? '● Active' : '○ Free' }}
          </span>
        </div>

        <!-- Name -->
        <div class="font-semibold text-gray-800 text-sm mb-3">Table {{ table.tableNumber }}</div>

        <!-- Reset button or free label -->
        <button
          v-if="table.hasActiveSession"
          class="w-full bg-red-50 hover:bg-red-100 active:scale-95 transition-all text-red-600 font-semibold text-sm py-2 rounded-lg border border-red-200"
          @click="confirmReset(table)"
        >
          🔄 Reset Session
        </button>
        <div
          v-else
          class="w-full text-center text-xs text-gray-300 font-medium py-2 rounded-lg bg-gray-50"
        >
          No active session
        </div>
      </div>

      <!-- Empty -->
      <div v-if="tables.length === 0 && !loading" class="col-span-2 text-center py-16 text-gray-400">
        <div class="text-5xl mb-3">🪑</div>
        <p class="font-medium">No tables found</p>
      </div>
    </div>

    <!-- Confirm Modal -->
    <div v-if="confirmModal.open"
      class="fixed inset-0 bg-black/50 flex items-center justify-center z-40 px-4"
      @click.self="confirmModal.open = false">
      <div class="bg-white rounded-2xl p-6 w-full max-w-sm shadow-xl">
        <div class="text-4xl text-center mb-3">⚠️</div>
        <h3 class="text-center font-bold text-gray-800 text-lg mb-1">{{ confirmModal.title }}</h3>
        <p class="text-center text-gray-500 text-sm mb-6">{{ confirmModal.message }}</p>
        <div class="flex gap-3">
          <button
            class="flex-1 py-2.5 rounded-xl border border-gray-200 text-gray-600 font-semibold hover:bg-gray-50"
            @click="confirmModal.open = false"
          >Cancel</button>
          <button
            class="flex-1 py-2.5 rounded-xl bg-red-500 hover:bg-red-600 text-white font-semibold"
            @click="confirmModal.action"
          >Reset</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, reactive } from "vue";
import api from "@/api/axios";

interface TableItem {
  id: number;
  tableNumber: string;
  hasActiveSession: boolean;
}

const tables = ref<TableItem[]>([]);
const loading = ref(false);
const toast = reactive({ show: false, type: "success", message: "" });
const confirmModal = reactive({
  open: false,
  title: "",
  message: "",
  action: () => {},
});

const activeTables = computed(() => tables.value.filter(t => t.hasActiveSession));
const freeTables = computed(() => tables.value.filter(t => !t.hasActiveSession));

function showToast(type: "success" | "error", message: string) {
  toast.type = type;
  toast.message = message;
  toast.show = true;
  setTimeout(() => (toast.show = false), 3000);
}

async function load() {
  loading.value = true;
  try {
    // ✅ use the new endpoint that includes hasActiveSession
    const res = await api.get("/api/v1/Table/with-session-status");
    tables.value = res.data?.data || [];
  } catch (e: any) {
    showToast("error", e.response?.data?.message || "Failed to load tables");
  } finally {
    loading.value = false;
  }
}

function confirmReset(table: TableItem) {
  confirmModal.title = `Reset Table ${table.tableNumber}?`;
  confirmModal.message = "This will close the active session. Customers will be logged out.";
  confirmModal.action = () => doReset(table.id);
  confirmModal.open = true;
}

function confirmResetAll() {
  confirmModal.title = "Reset All Active Sessions?";
  confirmModal.message = `This will close ${activeTables.value.length} active sessions. All customers will be logged out.`;
  confirmModal.action = () => doResetAll();
  confirmModal.open = true;
}

async function doReset(tableId: number) {
  confirmModal.open = false;
  loading.value = true;
  try {
    await api.post(`/api/v1/TableSession/ResetTable/${tableId}`);
    showToast("success", "Session closed successfully!");
    await load();
  } catch (e: any) {
    showToast("error", e.response?.data?.message || "Failed to reset session");
  } finally {
    loading.value = false;
  }
}

async function doResetAll() {
  confirmModal.open = false;
  loading.value = true;
  try {
    await Promise.all(
      activeTables.value.map(t =>
        api.post(`/api/v1/TableSession/ResetTable/${t.id}`)
      )
    );
    showToast("success", `${activeTables.value.length} sessions closed!`);
    await load();
  } catch (e: any) {
    showToast("error", "Failed to reset some sessions");
  } finally {
    loading.value = false;
  }
}

onMounted(load);
</script>