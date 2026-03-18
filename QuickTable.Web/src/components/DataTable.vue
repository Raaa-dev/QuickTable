<script setup>
defineProps({
  title: String,
  icon: String,
  count: Number,
  loading: Boolean,
  search: String,
  statusFilter: String,
})
defineEmits(['update:search', 'update:statusFilter'])
</script>

<template>
  <div>
    <!-- Toolbar -->
    <div class="toolbar">
      <div class="search-wrap">
        <span class="search-ico">🔍</span>
        <input
          class="search-input"
          :value="search"
          @input="$emit('update:search', $event.target.value)"
          :placeholder="'Search ' + title.toLowerCase() + '...'"
        />
      </div>
      <select
        class="filter-sel"
        :value="statusFilter"
        @change="$emit('update:statusFilter', $event.target.value)"
      >
        <option value="">All Status</option>
        <option value="active">Active Only</option>
        <option value="inactive">Inactive Only</option>
      </select>
      <!-- Extra filters slot (e.g. category dropdown) -->
      <slot name="filters" />
    </div>

    <!-- Table card -->
    <div class="table-card">
      <div class="table-head-bar">
        <div class="table-head-title">
          {{ icon }} {{ title }}
          <span class="record-count">{{ count }} records</span>
        </div>
      </div>

      <!-- Loading -->
      <div class="state-box" v-if="loading">
        <div class="loader"></div>
        <div class="state-title" style="margin-top:16px">Loading {{ title }}...</div>
      </div>

      <!-- Empty -->
      <div class="state-box" v-else-if="count === 0">
        <div class="state-emoji">📭</div>
        <div class="state-title">No records found</div>
        <div class="state-sub">Try adjusting your search or add a new record</div>
      </div>

      <!-- Table content slot -->
      <slot v-else name="table" />
    </div>
  </div>
</template>

<style scoped>
.toolbar { display: flex; align-items: center; gap: 10px; margin-bottom: 18px; }
.search-wrap { position: relative; flex: 1; }
.search-ico { position: absolute; left: 12px; top: 50%; transform: translateY(-50%); color: var(--text3); pointer-events: none; font-size: 14px; }
.search-input {
  width: 100%; background: var(--surface); border: 1px solid var(--border);
  border-radius: var(--radius); padding: 10px 14px 10px 36px; color: var(--text);
  font-family: var(--font); font-size: 14px; outline: none; transition: border 0.15s;
}
.search-input:focus { border-color: var(--accent); }
.search-input::placeholder { color: var(--text3); }
.filter-sel {
  background: var(--surface); border: 1px solid var(--border); border-radius: var(--radius);
  padding: 10px 14px; color: var(--text); font-family: var(--font); font-size: 14px; outline: none; cursor: pointer;
}
.filter-sel:focus { border-color: var(--accent); }
.filter-sel option { background: var(--surface2); }
.table-card { background: var(--surface); border: 1px solid var(--border); border-radius: var(--radius-lg); overflow: hidden; }
.table-head-bar { padding: 16px 20px; border-bottom: 1px solid var(--border); display: flex; align-items: center; justify-content: space-between; }
.table-head-title { font-size: 14px; font-weight: 700; display: flex; align-items: center; gap: 8px; }
.record-count { font-size: 12px; color: var(--text3); font-family: var(--mono); background: var(--surface2); padding: 2px 10px; border-radius: 20px; }
.state-box { text-align: center; padding: 64px 20px; color: var(--text3); }
.state-emoji { font-size: 48px; margin-bottom: 16px; }
.state-title { font-size: 16px; font-weight: 700; color: var(--text2); margin-bottom: 6px; }
.state-sub { font-size: 13px; font-family: var(--mono); }
.loader { display: inline-block; width: 32px; height: 32px; border: 3px solid var(--border); border-top-color: var(--accent); border-radius: 50%; animation: spin 0.7s linear infinite; }
@keyframes spin { to { transform: rotate(360deg); } }
</style>
