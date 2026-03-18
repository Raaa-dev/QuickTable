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
const allItems = ref([]); // for item count per category
const loading = ref(false);
const saving = ref(false);
const search = ref("");
const statusFilter = ref("");
const modal = reactive({ open: false, isEdit: false });
const delModal = reactive({ open: false, id: null, label: "" });
const form = reactive({ id: null, name: "", isActive: true });

const COLORS = [
  "#ff6b35",
  "#4ecdc4",
  "#a78bfa",
  "#f59e0b",
  "#22c55e",
  "#3b82f6",
  "#ec4899",
];
const aColor = (name) => COLORS[(name?.charCodeAt(0) || 0) % COLORS.length];
const itemCount = (catId) =>
  allItems.value.filter((m) => m.categoryId === catId).length;

const filtered = computed(() => {
  let d = data.value;
  if (search.value)
    d = d.filter((r) =>
      r.name.toLowerCase().includes(search.value.toLowerCase()),
    );
  if (statusFilter.value === "active") d = d.filter((r) => r.isActive);
  if (statusFilter.value === "inactive") d = d.filter((r) => !r.isActive);
  return d;
});

async function load() {
  loading.value = true;
  try {
    const [cats, items] = await Promise.all([
      fetchAll("/MenuCategory"),
      fetchAll("/MenuItem"),
    ]);
    data.value = cats;
    allItems.value = items;
  } catch {
    toast("❌ Failed to load", "error");
  } finally {
    loading.value = false;
  }
}

function openCreate() {
  Object.assign(form, { id: null, name: "", isActive: true });
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
  delModal.label = row.name;
  delModal.open = true;
}

async function save() {
  saving.value = true;
  try {
    const payload = { name: form.name, isActive: form.isActive };
    if (modal.isEdit)
      await updateRecord("/MenuCategory/Update", form.id, payload);
    else await createRecord("/MenuCategory/Create", payload);
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
    await deleteRecord("/MenuCategory/Delete", delModal.id);
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
    title="Categories"
    breadcrumb="Categories"
    add-label="Category"
    @add="openCreate"
    @refresh="load"
  >
    <StatsRow
      icon="🗂️"
      label="Categories"
      :total="data.length"
      :active="data.filter((r) => r.isActive).length"
      :inactive="data.filter((r) => !r.isActive).length"
      extra-icon="📋"
      :extra-value="data.length"
      extra-label="All Records"
    />

    <DataTable
      title="Categories"
      icon="🗂️"
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
              <th>Name</th>
              <th>Status</th>
              <th>Items</th>
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
                  <div
                    class="avatar"
                    :style="{ background: aColor(row.name), color: '#fff' }"
                  >
                    {{ row.name[0] }}
                  </div>
                  {{ row.name }}
                </div>
              </td>
              <td>
                <span
                  class="chip"
                  :class="row.isActive ? 'c-green' : 'c-red'"
                  >{{ row.isActive ? "● Active" : "● Inactive" }}</span
                >
              </td>
              <td>
                <span class="chip c-teal">{{ itemCount(row.id) }} items</span>
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
    singular="Category"
    :saving="saving"
    @close="modal.open = false"
    @save="save"
  >
    <FormField
      label="Category Name"
      v-model="form.name"
      placeholder="e.g. Drinks, Soups..."
    />
    <ToggleField
      label="Active Status"
      :description="
        form.isActive ? 'Visible to customers' : 'Hidden from customers'
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
.avatar {
  width: 32px;
  height: 32px;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 14px;
  font-weight: 800;
  flex-shrink: 0;
}
.row-actions {
  display: flex;
  gap: 6px;
}
</style>
