<script setup>
defineProps({
  title: String,
  breadcrumb: String,
  addLabel: String,
});
defineEmits(["add", "refresh"]);
</script>

<template>
  <div class="page-wrap">
    <!-- Topbar -->
    <div class="topbar">
      <div>
        <div class="breadcrumb">
          QuickTable / <span>{{ breadcrumb }}</span>
        </div>
        <div class="page-title">{{ title }}</div>
      </div>
      <div class="topbar-right">
        <button class="btn btn-ghost" @click="$emit('refresh')">
          🔄 Refresh
        </button>
        <button class="btn btn-primary" @click="$emit('add')">
          ＋ {{ addLabel }}
        </button>
      </div>
    </div>

    <!-- Content slot -->
    <div class="page-content">
      <slot />
    </div>
  </div>
</template>

<style scoped>
.page-wrap {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}
.topbar {
  padding: 20px 32px;
  border-bottom: 1px solid var(--border);
  background: var(--surface);
  display: flex;
  align-items: center;
  justify-content: space-between;
  position: sticky;
  top: 0;
  z-index: 50;
}
.breadcrumb {
  font-size: 11px;
  color: rgb(190, 190, 190);
  font-family: var(--mono);
  margin-bottom: 4px;
}
.breadcrumb span {
  color: var(--accent);
}
.page-title {
  font-size: 22px;
  font-weight: 800;
  letter-spacing: -0.5px;
}
.topbar-right {
  display: flex;
  align-items: center;
  gap: 10px;
}
.page-content {
  padding: 28px 32px;
  flex: 1;
  animation: fadeIn 0.2s ease;
}
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(8px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>
