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
const API_BASE = import.meta.env.VITE_API_BASE_URL;

const data = ref([]);
const categories = ref([]);
const loading = ref(false);
const saving = ref(false);
const search = ref("");
const statusFilter = ref("");
const catFilter = ref("");
const modal = reactive({ open: false, isEdit: false });
const delModal = reactive({ open: false, id: null, label: "" });

// Image state
const imageFile = ref(null);
const imagePreview = ref(null);
const imageInput = ref(null);

const form = reactive({
  id: null,
  name: "",
  categoryId: "",
  price: 0,
  isActive: true,
  imageUrl: null,
});

const COLORS = ["#ff6b35","#4ecdc4","#a78bfa","#f59e0b","#22c55e","#3b82f6","#ec4899"];
const aColor = (name) => COLORS[(name?.charCodeAt(0) || 0) % COLORS.length];
const catName = (id) => (categories.value.find((c) => c.id === id) || {}).name || `#${id}`;

const avgPrice = computed(() => {
  if (!data.value.length) return "$0.00";
  const avg = data.value.reduce((s, r) => s + (r.price || 0), 0) / data.value.length;
  return "$" + avg.toFixed(2);
});

const filtered = computed(() => {
  let d = data.value;
  if (search.value)
    d = d.filter((r) => r.name.toLowerCase().includes(search.value.toLowerCase()));
  if (statusFilter.value === "active") d = d.filter((r) => r.isActive);
  if (statusFilter.value === "inactive") d = d.filter((r) => !r.isActive);
  if (catFilter.value) d = d.filter((r) => r.categoryId == catFilter.value);
  return d;
});

async function load() {
  loading.value = true;
  try {
    const [items, cats] = await Promise.all([
      fetchAll("/MenuItem"),
      fetchAll("/MenuCategory"),
    ]);
    data.value = items;
    categories.value = cats;
  } catch {
    toast("❌ Failed to load", "error");
  } finally {
    loading.value = false;
  }
}

function resetImageState() {
  imageFile.value = null;
  imagePreview.value = null;
  if (imageInput.value) imageInput.value.value = "";
}

function openCreate() {
  Object.assign(form, {
    id: null,
    name: "",
    categoryId: categories.value[0]?.id || "",
    price: 0,
    isActive: true,
    imageUrl: null,
  });
  resetImageState();
  modal.isEdit = false;
  modal.open = true;
}

function openEdit(row) {
  Object.assign(form, row);
  resetImageState();
  // Show existing image as preview
  imagePreview.value = row.imageUrl ? `${API_BASE}${row.imageUrl}` : null;
  modal.isEdit = true;
  modal.open = true;
}

function askDelete(row) {
  delModal.id = row.id;
  delModal.label = row.name;
  delModal.open = true;
}

// Handle image file pick
function onImagePick(e) {
  const file = e.target.files[0];
  if (!file) return;
  imageFile.value = file;
  imagePreview.value = URL.createObjectURL(file);
}

function removeImage() {
  resetImageState();
  // Keep existing imageUrl in form if editing (don't delete from server yet)
  imagePreview.value = null;
}

async function save() {
  saving.value = true;
  try {
    const payload = {
      name: form.name,
      categoryId: parseInt(form.categoryId),
      price: parseFloat(form.price),
      isActive: form.isActive,
    };

    let savedItem;
    if (modal.isEdit) {
      savedItem = await updateRecord("/MenuItem/Update", form.id, payload);
    } else {
      savedItem = await createRecord("/MenuItem/Create", payload);
    }

    // If a new image was picked, upload it separately
    if (imageFile.value) {
      const itemId = savedItem?.id || form.id;
      await uploadImage(itemId, imageFile.value);
    }

    modal.open = false;
    toast(modal.isEdit ? "✅ Updated!" : "✅ Created!", "success");
    load();
  } catch (e) {
    toast("❌ " + e.message, "error");
  } finally {
    saving.value = false;
  }
}

// Upload image to PUT /MenuItem/{id}/image
async function uploadImage(id, file) {
  const formData = new FormData();
  formData.append("file", file);
  const res = await fetch(`${API_BASE}/api/v1/MenuItem/${id}/image`, {
    method: "PUT",
    body: formData,
  });
  if (!res.ok) throw new Error("Image upload failed!");
}

async function doDelete() {
  saving.value = true;
  try {
    await deleteRecord("/MenuItem/Delete", delModal.id);
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
    title="Menu Items"
    breadcrumb="Menu Items"
    add-label="Menu Item"
    @add="openCreate"
    @refresh="load"
  >
    <StatsRow
      icon="🍽️"
      label="Menu Items"
      :total="data.length"
      :active="data.filter((r) => r.isActive).length"
      :inactive="data.filter((r) => !r.isActive).length"
      extra-icon="💰"
      :extra-value="avgPrice"
      extra-label="Avg Price"
    />

    <DataTable
      title="Menu Items"
      icon="🍽️"
      :count="filtered.length"
      :loading="loading"
      v-model:search="search"
      v-model:statusFilter="statusFilter"
    >
      <template #filters>
        <select class="filter-sel" v-model="catFilter">
          <option value="">All Categories</option>
          <option v-for="c in categories" :key="c.id" :value="c.id">
            {{ c.name }}
          </option>
        </select>
      </template>

      <template #table>
        <table>
          <thead>
            <tr>
              <th>No.</th>
              <th>Image</th>
              <th>Name</th>
              <th>Category</th>
              <th>Price</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(row, index) in filtered" :key="row.id">
              <td><span class="id-tag">#{{ index + 1 }}</span></td>

              <!-- Image column -->
              <td>
                <div class="thumb-wrap">
                  <img
                    v-if="row.imageUrl"
                    :src="`${API_BASE}${row.imageUrl}`"
                    class="thumb"
                    :alt="row.name"
                     @error="(e) => e.target.style.display='none'"
                  />
                  <div
                    v-else
                    class="thumb-placeholder"
                    :style="{ background: aColor(row.name) }"
                  >
                    {{ row.name?.[0] }}
                  </div>
                </div>
              </td>

              <td>
                <span class="name-text">{{ row.name }}</span>
              </td>
              <td>
                <span class="chip c-purple">{{ catName(row.categoryId) }}</span>
              </td>
              <td>
                <span class="price-tag">${{ Number(row.price).toFixed(2) }}</span>
              </td>
              <td>
                <span class="chip" :class="row.isActive ? 'c-green' : 'c-red'">
                  {{ row.isActive ? "● Active" : "● Inactive" }}
                </span>
              </td>
              <td>
                <div class="row-actions">
                  <button class="btn btn-ghost btn-sm" @click="openEdit(row)">✏️ Edit</button>
                  <button class="btn btn-danger btn-sm" @click="askDelete(row)">🗑️</button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </template>
    </DataTable>
  </PageLayout>

  <!-- CRUD Modal -->
  <CrudModal
    :open="modal.open"
    :is-edit="modal.isEdit"
    singular="Menu Item"
    :saving="saving"
    @close="modal.open = false"
    @save="save"
  >
    <!-- Image Upload Field -->
    <div class="form-group">
      <label class="form-label">Item Image</label>
      <div class="image-upload-area" @click="imageInput.click()">
        <!-- Preview -->
        <template v-if="imagePreview">
          <img :src="imagePreview" class="image-preview" alt="preview" />
          <button class="remove-img-btn" @click.stop="removeImage">✕</button>
        </template>
        <!-- Placeholder -->
        <template v-else>
          <div class="upload-placeholder">
            <span class="upload-icon">🖼️</span>
            <span class="upload-text">Click to upload image</span>
            <span class="upload-hint">JPEG, PNG, WEBP</span>
          </div>
        </template>
      </div>
      <input
        ref="imageInput"
        type="file"
        accept="image/jpeg,image/png,image/webp"
        style="display: none"
        @change="onImagePick"
      />
    </div>

    <FormField
      label="Item Name"
      v-model="form.name"
      placeholder="e.g. Fried Rice, Beer..."
    />

    <div class="form-row">
      <div class="form-group">
        <label class="form-label">Category</label>
        <select class="form-select" v-model="form.categoryId">
          <option disabled value="">Select category</option>
          <option v-for="c in categories" :key="c.id" :value="c.id">
            {{ c.name }}
          </option>
        </select>
      </div>
      <FormField
        label="Price ($)"
        v-model="form.price"
        type="number"
        placeholder="0.00"
      />
    </div>

    <ToggleField
      label="Active Status"
      :description="form.isActive ? 'Available to order' : 'Not available'"
      v-model="form.isActive"
    />
  </CrudModal>

  <DeleteConfirm
    :open="delModal.open"
    :label="delModal.label"
    title="Delete Record"
    text=" Are you sure you want to delete?"
    textFooter=" This action cannot be undone."
    :saving="saving"
    @close="delModal.open = false"
    @confirm="doDelete"
  />
</template>

<style scoped>
table { width: 100%; border-collapse: collapse; }
thead tr { background: var(--surface2); }
th { padding: 11px 16px; text-align: left; font-size: 11px; font-family: var(--mono); color: var(--text3); letter-spacing: 1.5px; text-transform: uppercase; }
td { padding: 13px 16px; font-size: 14px; border-bottom: 1px solid var(--border); vertical-align: middle; }
tr:last-child td { border-bottom: none; }
tbody tr { transition: background 0.1s; }
tbody tr:hover { background: var(--surface2); }

/* Thumbnail in table */
.thumb-wrap { width: 44px; height: 44px; border-radius: 10px; overflow: hidden; flex-shrink: 0; }
.thumb { width: 100%; height: 100%; object-fit: cover; }
.thumb-placeholder { width: 44px; height: 44px; border-radius: 10px; display: flex; align-items: center; justify-content: center; font-size: 18px; font-weight: 800; color: #fff; }

.name-text { font-weight: 600; }
.price-tag { font-family: var(--mono); color: var(--accent); font-weight: 700; }
.row-actions { display: flex; gap: 6px; }

/* Image upload in modal */
.form-group { display: flex; flex-direction: column; gap: 6px; }
.form-label { font-size: 11px; color: var(--text2); font-family: var(--mono); letter-spacing: 1.5px; text-transform: uppercase; font-weight: 500; }

.image-upload-area {
  position: relative;
  border: 2px dashed var(--border2);
  border-radius: var(--radius);
  height: 140px;
  cursor: pointer;
  overflow: hidden;
  transition: border-color 0.2s;
  display: flex;
  align-items: center;
  justify-content: center;
}
.image-upload-area:hover { border-color: var(--accent); }

.image-preview { width: 100%; height: 100%; object-fit: cover; }

.remove-img-btn {
  position: absolute;
  top: 8px;
  right: 8px;
  width: 26px;
  height: 26px;
  border-radius: 6px;
  background: rgba(0,0,0,0.6);
  border: none;
  color: #fff;
  font-size: 12px;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
}
.remove-img-btn:hover { background: var(--red); }

.upload-placeholder {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 6px;
  pointer-events: none;
}
.upload-icon { font-size: 28px; }
.upload-text { font-size: 13px; color: var(--text2); font-weight: 600; }
.upload-hint { font-size: 11px; color: var(--text3); }

/* Form layout */
.form-row { display: grid; grid-template-columns: 1fr 1fr; gap: 14px; }
.form-select { background: var(--surface2); border: 1px solid var(--border); border-radius: var(--radius); padding: 10px 14px; color: var(--text); font-family: var(--font); font-size: 14px; outline: none; transition: border 0.15s; cursor: pointer; }
.form-select:focus { border-color: var(--accent); box-shadow: 0 0 0 3px var(--accent-dim); }
.form-select option { background: var(--surface2); }
.filter-sel { background: var(--surface); border: 1px solid var(--border); border-radius: var(--radius); padding: 10px 14px; color: var(--text); font-family: var(--font); font-size: 14px; outline: none; cursor: pointer; }
.filter-sel option { background: var(--surface2); }
</style>