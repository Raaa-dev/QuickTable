<script setup>
// FormField
defineProps({ label: String, modelValue: [String, Number], type: { default: 'text' }, placeholder: String })
defineEmits(['update:modelValue'])
</script>

<template>
  <div class="form-group">
    <label class="form-label">{{ label }}</label>
    <input
      class="form-input"
      :type="type"
      :value="modelValue"
      @input="$emit('update:modelValue', type === 'number' ? +$event.target.value : $event.target.value)"
      :placeholder="placeholder"
    />
  </div>
</template>

<style scoped>
.form-group { display: flex; flex-direction: column; gap: 6px; }
.form-label { font-size: 11px; color: var(--text2); font-family: var(--mono); letter-spacing: 1.5px; text-transform: uppercase; font-weight: 500; }
.form-input {
  background: var(--surface2); border: 1px solid var(--border); border-radius: var(--radius);
  padding: 10px 14px; color: var(--text); font-family: var(--font); font-size: 14px; outline: none; transition: border 0.15s;
}
.form-input:focus { border-color: var(--accent); box-shadow: 0 0 0 3px var(--accent-dim); }
.form-input::placeholder { color: var(--text3); }
</style>
