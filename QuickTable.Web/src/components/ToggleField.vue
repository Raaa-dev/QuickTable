<script setup>
defineProps({ label: String, description: String, modelValue: Boolean })
defineEmits(['update:modelValue'])
</script>

<template>
  <div class="toggle-field">
    <div>
      <div class="toggle-label">{{ label }}</div>
      <div class="toggle-desc">{{ description }}</div>
    </div>
    <label class="sw">
      <input type="checkbox" :checked="modelValue" @change="$emit('update:modelValue', $event.target.checked)" />
      <div class="sw-track"></div>
      <div class="sw-thumb"></div>
    </label>
  </div>
</template>

<style scoped>
.toggle-field { display: flex; align-items: center; justify-content: space-between; background: var(--surface2); border: 1px solid var(--border); border-radius: var(--radius); padding: 12px 14px; }
.toggle-label { font-size: 14px; font-weight: 600; }
.toggle-desc { font-size: 12px; color: var(--text3); margin-top: 2px; }
.sw { position: relative; width: 48px; height: 26px; flex-shrink: 0; cursor: pointer; }
.sw input { opacity: 0; width: 0; height: 0; position: absolute; }
.sw-track { position: absolute; inset: 0; background: var(--border); border-radius: 26px; transition: 0.2s; }
.sw-thumb { position: absolute; width: 20px; height: 20px; background: #fff; border-radius: 50%; top: 3px; left: 3px; transition: 0.2s; box-shadow: 0 2px 4px rgba(0,0,0,0.3); pointer-events: none; }
.sw input:checked ~ .sw-track { background: var(--green); }
.sw input:checked ~ .sw-thumb { transform: translateX(22px); }
</style>
