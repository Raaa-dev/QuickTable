<script setup>
defineProps({
  open: Boolean,
  label: String,
  saving: Boolean,
})
defineEmits(['close', 'confirm'])
</script>

<template>
  <Teleport to="body">
    <div class="overlay" :class="{ open }" @click.self="$emit('close')">
      <div class="confirm-modal" v-if="open">
        <div class="confirm-icon">🗑️</div>
        <div class="confirm-title">Delete Record?</div>
        <div class="confirm-sub">
          Are you sure you want to delete<br/>
          <strong style="color:var(--text)">"{{ label }}"</strong>?<br/>
          This action cannot be undone.
        </div>
        <div class="confirm-actions">
          <button class="btn btn-ghost" @click="$emit('close')">Cancel</button>
          <button class="btn btn-danger" @click="$emit('confirm')" :disabled="saving">
            {{ saving ? '⏳' : '🗑️' }} Delete
          </button>
        </div>
      </div>
    </div>
  </Teleport>
</template>

<style scoped>
.overlay { display: none; position: fixed; inset: 0; background: rgba(0,0,0,0.75); z-index: 300; align-items: center; justify-content: center; backdrop-filter: blur(6px); }
.overlay.open { display: flex; }
.confirm-modal { background: var(--surface); border: 1px solid var(--red); border-radius: 16px; padding: 28px; width: 360px; max-width: 95vw; text-align: center; animation: modalIn 0.22s cubic-bezier(0.34,1.56,0.64,1); }
@keyframes modalIn { from{transform:scale(0.9) translateY(20px);opacity:0} to{transform:scale(1) translateY(0);opacity:1} }
.confirm-icon { font-size: 40px; margin-bottom: 12px; }
.confirm-title { font-size: 17px; font-weight: 800; margin-bottom: 8px; color: var(--red); }
.confirm-sub { font-size: 13px; color: var(--text2); margin-bottom: 24px; line-height: 1.6; }
.confirm-actions { display: flex; gap: 10px; justify-content: center; }
</style>
