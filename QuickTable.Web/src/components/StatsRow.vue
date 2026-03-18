<script setup>
defineProps({
  icon: String,
  label: String,
  total: Number,
  active: Number,
  inactive: Number,
  extraIcon: String,
  extraValue: [String, Number],
  extraLabel: String,
})
</script>

<template>
  <div class="stats-grid">
    <div class="stat-card s-total">
      <div class="stat-icon">{{ icon }}</div>
      <div>
        <div class="stat-val">{{ total }}</div>
        <div class="stat-label">Total {{ label }}</div>
      </div>
    </div>
    <div class="stat-card s-active">
      <div class="stat-icon">✅</div>
      <div>
        <div class="stat-val">{{ active }}</div>
        <div class="stat-label">Active</div>
      </div>
    </div>
    <div class="stat-card s-inactive">
      <div class="stat-icon">🚫</div>
      <div>
        <div class="stat-val">{{ inactive }}</div>
        <div class="stat-label">Inactive</div>
      </div>
    </div>
    <div class="stat-card s-extra">
      <div class="stat-icon">{{ extraIcon }}</div>
      <div>
        <div class="stat-val extra-val">{{ extraValue }}</div>
        <div class="stat-label">{{ extraLabel }}</div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.stats-grid { display: grid; grid-template-columns: repeat(4,1fr); gap: 14px; margin-bottom: 24px; }
.stat-card {
  background: var(--surface); border: 1px solid var(--border); border-radius: var(--radius-lg);
  padding: 18px 20px; display: flex; align-items: center; gap: 14px; transition: border-color 0.15s;
}
.stat-card:hover { border-color: var(--border2); }
.stat-icon { width: 44px; height: 44px; border-radius: 10px; display: flex; align-items: center; justify-content: center; font-size: 22px; flex-shrink: 0; }
.s-total .stat-icon  { background: var(--accent-dim); }
.s-active .stat-icon { background: var(--green-dim); }
.s-inactive .stat-icon { background: var(--red-dim); }
.s-extra .stat-icon  { background: var(--purple-dim); }
.stat-val { font-size: 28px; font-weight: 800; line-height: 1; font-family: var(--mono); }
.extra-val { font-size: 20px; }
.stat-label { font-size: 12px; color: var(--text2); margin-top: 3px; }
</style>
