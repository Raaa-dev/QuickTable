<script setup>
import { computed } from 'vue'

const props = defineProps({
  currentPage: { type: Number, default: 1 },
  totalRecords: { type: Number, default: 0 },
  pageSize:     { type: Number, default: 10 },
})
const emit = defineEmits(['update:currentPage', 'update:pageSize'])

const totalPages = computed(() => Math.ceil(props.totalRecords / props.pageSize) || 1)

const pages = computed(() => {
  const total = totalPages.value
  const current = props.currentPage
  const delta = 2
  const range = []

  for (let i = Math.max(1, current - delta); i <= Math.min(total, current + delta); i++) {
    range.push(i)
  }

  if (range[0] > 1) {
    if (range[0] > 2) range.unshift('...')
    range.unshift(1)
  }
  if (range[range.length - 1] < total) {
    if (range[range.length - 1] < total - 1) range.push('...')
    range.push(total)
  }

  return range
})

const from = computed(() => Math.min((props.currentPage - 1) * props.pageSize + 1, props.totalRecords))
const to   = computed(() => Math.min(props.currentPage * props.pageSize, props.totalRecords))

function goTo(page) {
  if (page < 1 || page > totalPages.value || page === '...') return
  emit('update:currentPage', page)
}
</script>

<template>
  <div class="pagination-bar" v-if="totalRecords > 0">
    <!-- Info -->
    <div class="page-info">
      Showing <span>{{ from }}–{{ to }}</span> of <span>{{ totalRecords }}</span> records
    </div>

    <!-- Controls -->
    <div class="page-controls">
      <!-- Page size -->
      <select class="page-size-sel" :value="pageSize" @change="$emit('update:pageSize', +$event.target.value); $emit('update:currentPage', 1)">
        <option :value="5">5 / page</option>
        <option :value="10">10 / page</option>
        <option :value="20">20 / page</option>
        <option :value="50">50 / page</option>
      </select>

      <!-- Prev -->
      <button class="page-btn" :disabled="currentPage === 1" @click="goTo(currentPage - 1)">‹</button>

      <!-- Pages -->
      <button
        v-for="p in pages" :key="p"
        class="page-btn"
        :class="{ active: p === currentPage, dots: p === '...' }"
        :disabled="p === '...'"
        @click="goTo(p)"
      >{{ p }}</button>

      <!-- Next -->
      <button class="page-btn" :disabled="currentPage === totalPages" @click="goTo(currentPage + 1)">›</button>
    </div>
  </div>
</template>

<style scoped>
.pagination-bar {
  display: flex; align-items: center; justify-content: space-between;
  padding: 14px 20px; border-top: 1px solid var(--border);
  background: var(--surface);
}
.page-info { font-size: 13px; color: var(--text3); font-family: var(--mono); }
.page-info span { color: var(--text2); font-weight: 600; }
.page-controls { display: flex; align-items: center; gap: 4px; }
.page-size-sel {
  background: var(--surface2); border: 1px solid var(--border); border-radius: var(--radius);
  padding: 5px 10px; color: var(--text2); font-family: var(--mono); font-size: 12px;
  outline: none; cursor: pointer; margin-right: 8px;
}
.page-size-sel option { background: var(--surface2); }
.page-btn {
  min-width: 32px; height: 32px; padding: 0 8px; border-radius: 8px;
  background: var(--surface2); border: 1px solid var(--border);
  color: var(--text2); font-family: var(--mono); font-size: 13px; font-weight: 600;
  cursor: pointer; transition: all 0.15s; display: flex; align-items: center; justify-content: center;
}
.page-btn:hover:not(:disabled):not(.dots) { border-color: var(--accent); color: var(--accent); }
.page-btn.active { background: var(--accent); border-color: var(--accent); color: #fff; }
.page-btn:disabled { opacity: 0.4; cursor: not-allowed; }
.page-btn.dots { border-color: transparent; background: transparent; cursor: default; }
</style>
