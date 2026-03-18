<script setup>
import { ref, computed, onMounted, reactive } from "vue";
import PageLayout from "@/components/PageLayout.vue";
import StatsRow from "@/components/StatsRow.vue";
import DataTable from "@/components/DataTable.vue";
import CrudModal from "@/components/CrudModal.vue";
import DeleteConfirm from "@/components/DeleteConfirm.vue";
import FormField from "@/components/FormField.vue";
import ToggleField from "@/components/ToggleField.vue";
import {
  fetchAll,
  createRecord,
  updateRecord,
  deleteRecord,
} from "@/composables/useApi";
import { useToast } from "@/composables/useToast";

const { toast } = useToast();

const data = ref([]);
const loading = ref(false);
const saving = ref(false);
const search = ref("");
const statusFilter = ref("");
const modal = reactive({ open: false, isEdit: false });
const delModal = reactive({ open: false, id: null, label: "" });
const form = reactive({
  id: null,
  tableNumber: "",
  capacity: 2,
  isActive: true,
});

const totalSeats = computed(() =>
  data.value.reduce((s, r) => s + (r.capacity || 0), 0),
);

const filtered = computed(() => {
  let d = data.value;
  if (search.value)
    d = d.filter((r) =>
      r.tableNumber?.toLowerCase().includes(search.value.toLowerCase()),
    );
  if (statusFilter.value === "active") d = d.filter((r) => r.isActive);
  if (statusFilter.value === "inactive") d = d.filter((r) => !r.isActive);
  return d;
});

async function load() {
  loading.value = true;
  try {
    data.value = await fetchAll("/Table");
  } catch {
    toast("❌ Failed to load", "error");
  } finally {
    loading.value = false;
  }
}

function openCreate() {
  Object.assign(form, {
    id: null,
    tableNumber: "",
    capacity: 2,
    isActive: true,
  });
  modal.isEdit = false;
  modal.open = true;
}
function openEdit(row) {
  Object.assign(form, row);
  modal.isEdit = true;
  modal.open = true;
}
function askDelete(row) {
  delModal.id = row.id;
  delModal.label = row.tableNumber;
  delModal.open = true;
}

async function save() {
  saving.value = true;
  try {
    const payload = {
      tableNumber: form.tableNumber,
      capacity: parseInt(form.capacity),
      isActive: form.isActive,
    };
    if (modal.isEdit) await updateRecord("/Table/Update", form.id, payload);
    else await createRecord("/Table/Create", payload);
    modal.open = false;
    toast(modal.isEdit ? "✅ Updated!" : "✅ Created!", "success");
    load();
  } catch (e) {
    toast("❌ " + e.message, "error");
  } finally {
    saving.value = false;
  }
}

async function doDelete() {
  saving.value = true;
  try {
    await deleteRecord("/Table", delModal.id);
    delModal.open = false;
    toast("🗑️ Deleted!", "success");
    load();
  } catch (e) {
    toast("❌ " + e.message, "error");
  } finally {
    saving.value = false;
  }
}

onMounted(load);
</script>

<template>
  <PageLayout
    title="Tables"
    breadcrumb="Tables"
    add-label="Table"
    @add="openCreate"
    @refresh="load"
  >
    <StatsRow
      icon="🪑"
      label="Tables"
      :total="data.length"
      :active="data.filter((r) => r.isActive).length"
      :inactive="data.filter((r) => !r.isActive).length"
      extra-icon="💺"
      :extra-value="totalSeats"
      extra-label="Total Seats"
    />

    <DataTable
      title="Tables"
      icon="🪑"
      :count="filtered.length"
      :loading="loading"
      v-model:search="search"
      v-model:statusFilter="statusFilter"
    >
      <template #table>
        <table>
          <thead>
            <tr>
              <th>No.</th>
              <th>Table Number</th>
              <th>Capacity</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(row, index) in filtered" :key="row.id">
              <td>
                <span class="id-tag">#{{ index + 1 }}</span>
              </td>
              <td>
                <div class="name-cell">
                  <div class="table-icon">🪑</div>
                  <span class="table-number">{{ row.tableNumber }}</span>
                </div>
              </td>
              <td>
                <span class="chip c-yellow">{{ row.capacity }} seats</span>
              </td>
              <td>
                <span
                  class="chip"
                  :class="row.isActive ? 'c-green' : 'c-red'"
                  >{{ row.isActive ? "● Active" : "● Inactive" }}</span
                >
              </td>
              <td>
                <div class="row-actions">
                  <button class="btn btn-ghost btn-sm" @click="openEdit(row)">
                    ✏️ Edit
                  </button>
                  <button class="btn btn-danger btn-sm" @click="askDelete(row)">
                    🗑️
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </template>
    </DataTable>
  </PageLayout>

  <CrudModal
    :open="modal.open"
    :is-edit="modal.isEdit"
    singular="Table"
    :saving="saving"
    @close="modal.open = false"
    @save="save"
  >
    <div class="form-row">
      <FormField
        label="Table Number"
        v-model="form.tableNumber"
        placeholder="e.g. T-001"
      />
      <FormField
        label="Capacity (seats)"
        v-model="form.capacity"
        type="number"
        placeholder="4"
      />
    </div>
    <ToggleField
      label="Active Status"
      :description="
        form.isActive ? 'Table is available' : 'Table is unavailable'
      "
      v-model="form.isActive"
    />
  </CrudModal>

  <DeleteConfirm
    :open="delModal.open"
    :label="delModal.label"
    :saving="saving"
    @close="delModal.open = false"
    @confirm="doDelete"
  />
</template>

<style scoped>
table {
  width: 100%;
  border-collapse: collapse;
}
thead tr {
  background: var(--surface2);
}
th {
  padding: 11px 16px;
  text-align: left;
  font-size: 11px;
  font-family: var(--mono);
  color: var(--text3);
  letter-spacing: 1.5px;
  text-transform: uppercase;
}
td {
  padding: 13px 16px;
  font-size: 14px;
  border-bottom: 1px solid var(--border);
  vertical-align: middle;
}
tr:last-child td {
  border-bottom: none;
}
tbody tr {
  transition: background 0.1s;
}
tbody tr:hover {
  background: var(--surface2);
}
.name-cell {
  display: flex;
  align-items: center;
  gap: 8px;
  font-weight: 600;
}
.table-icon {
  width: 32px;
  height: 32px;
  border-radius: 8px;
  background: var(--accent2-dim);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 14px;
  flex-shrink: 0;
}
.table-number {
  font-family: var(--mono);
  color: var(--accent2);
  font-weight: 700;
}
.row-actions {
  display: flex;
  gap: 6px;
}
.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 14px;
}
</style>
