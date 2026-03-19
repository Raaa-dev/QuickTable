<template>
  <PageLayout
    title="Reset Table Sessions"
    breadcrumb="Reset Table Sessions"
    @refresh="load"
  >
    <!-- Stats Row -->
    <StatsRow
      icon="🪑"
      label="Tables"
      :total="tables.length"
      :active="activeTables.length"
      :inactive="freeTables.length"
      active-label="Active Sessions"
      inactive-label="Free Tables"
    />

    <!-- DataTable wrapper -->
    <DataTable
      title="Table Sessions"
      icon="🪑"
      :count="tables.length"
      :loading="loading"
    >
      <!-- Reset All button in toolbar slot -->
      <template #toolbar>
        <button
          v-if="activeTables.length > 0"
          class="btn btn-danger btn-sm"
          @click="confirmResetAll"
        >
          🗑️ Reset All ({{ activeTables.length }})
        </button>
      </template>

      <template #table>
        <table>
          <thead>
            <tr>
              <th>No.</th>
              <th>Table</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(table, index) in tables" :key="table.id">
              <td><span class="id-tag">#{{ index + 1 }}</span></td>
              <td>
                <div class="table-name-cell">
                  <div
                    class="table-avatar"
                    :class="table.hasActiveSession ? 'active' : 'free'"
                  >
                    {{ table.tableNumber }}
                  </div>
                  <span class="name-text">Table {{ table.tableNumber }}</span>
                </div>
              </td>
              <td>
                <span class="chip" :class="table.hasActiveSession ? 'c-green' : 'c-gray'">
                  {{ table.hasActiveSession ? '● Busy' : '○ Free' }}
                </span>
              </td>
              <td>
                <div class="row-actions">
                  <button
                    v-if="table.hasActiveSession"
                    class="btn btn-danger btn-sm"
                    @click="confirmReset(table)"
                  >
                    🔄 Reset
                  </button>
                  <span v-else class="no-action-text">—</span>
                </div>
              </td>
            </tr>

            <!-- Empty state -->
            <tr v-if="tables.length === 0 && !loading">
              <td colspan="4">
                <div class="empty-state">
                  <div class="text-4xl mb-2">🪑</div>
                  <p>No tables found</p>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </template>
    </DataTable>
  </PageLayout>

  <!-- Confirm Modal reusing DeleteConfirm style -->
  <DeleteConfirm
    :open="confirmModal.open"
    :label="confirmModal.label"
    :saving="loading"
    title="Reset Table"
    text=" Are you sure you want to reset this table?"
    :message="confirmModal.message"
    confirm-label="Reset"
    @close="confirmModal.open = false"
    @confirm="confirmModal.action"
  />
</template>

<script setup lang="ts">
import { ref, computed, onMounted, reactive } from "vue";
import PageLayout from "@/components/PageLayout.vue";
import StatsRow from "@/components/StatsRow.vue";
import DataTable from "@/components/DataTable.vue";
import DeleteConfirm from "@/components/DeleteConfirm.vue";
import { useToast } from "@/composables/useToast";
import api from "@/api/axios";

interface TableItem {
  id: number;
  tableNumber: string;
  hasActiveSession: boolean;
}

const { toast } = useToast();

const tables = ref<TableItem[]>([]);
const loading = ref(false);

const confirmModal = reactive({
  open: false,
  title: "",
  label: "",
  message: "",
  action: () => {},
});

const activeTables = computed(() => tables.value.filter(t => t.hasActiveSession));
const freeTables = computed(() => tables.value.filter(t => !t.hasActiveSession));

async function load() {
  loading.value = true;
  try {
    const res = await api.get("/api/v1/TableSession/with-session-status");
    tables.value = res.data?.data || [];
  } catch (e: any) {
    toast("❌ " + (e.response?.data?.message || "Failed to load tables"), "error");
  } finally {
    loading.value = false;
  }
}

function confirmReset(table: TableItem) {
  confirmModal.title = `Reset Table ${table.tableNumber}?`;
  confirmModal.label = `Table ${table.tableNumber}`;
  confirmModal.message = "This will close the active session. Customers will be logged out.";
  confirmModal.action = () => doReset(table.id);
  confirmModal.open = true;
}

function confirmResetAll() {
  confirmModal.title = "Reset All Active Sessions?";
  confirmModal.label = `${activeTables.value.length} active sessions`;
  confirmModal.message = `This will close ${activeTables.value.length} active sessions. All customers will be logged out.`;
  confirmModal.action = () => doResetAll();
  confirmModal.open = true;
}

async function doReset(tableId: number) {
  confirmModal.open = false;
  loading.value = true;
  try {
    await api.post(`/api/v1/TableSession/ResetTable/${tableId}`);
    toast("✅ Session closed successfully!", "success");
    await load();
  } catch (e: any) {
    toast("❌ " + (e.response?.data?.message || "Failed to reset session"), "error");
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
    toast(`✅ ${activeTables.value.length} sessions closed!`, "success");
    await load();
  } catch (e: any) {
    toast("❌ Failed to reset some sessions", "error");
  } finally {
    loading.value = false;
  }
}

onMounted(load);
</script>

<style scoped>
table { width: 100%; border-collapse: collapse; }
thead tr { background: var(--surface2); }
th { padding: 11px 16px; text-align: left; font-size: 11px; font-family: var(--mono); color: var(--text3); letter-spacing: 1.5px; text-transform: uppercase; }
td { padding: 13px 16px; font-size: 14px; border-bottom: 1px solid var(--border); vertical-align: middle; }
tr:last-child td { border-bottom: none; }
tbody tr { transition: background 0.1s; }
tbody tr:hover { background: var(--surface2); }

.table-name-cell { display: flex; align-items: center; gap: 10px; }

.table-avatar {
  width: 40px; height: 40px;
  border-radius: 10px;
  display: flex; align-items: center; justify-content: center;
  font-weight: 800; font-size: 16px;
  flex-shrink: 0;
}
.table-avatar.active { background: #dcfce7; color: #16a34a; }
.table-avatar.free   { background: var(--surface2); color: var(--text3); }

.name-text { font-weight: 600; }

.row-actions { display: flex; gap: 6px; }

.no-action-text { color: var(--text3); font-size: 13px; }

.empty-state {
  text-align: center;
  padding: 48px 0;
  color: var(--text3);
}

/* Chip variants */
.chip { font-size: 11px; font-weight: 700; padding: 3px 10px; border-radius: 999px; display: inline-block; }
.c-green  { background: #dcfce7; color: #16a34a; }
.c-gray   { background: var(--surface2); color: var(--text3); }
</style>