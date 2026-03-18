<script setup>
defineProps({
  open: Boolean,
  isEdit: Boolean,
  singular: String,
  saving: Boolean,
})
defineEmits(['close', 'save'])
</script>

<template>
  <Teleport to="body">
    <div class="overlay" :class="{ open }" @click.self="$emit('close')">
      <div class="modal" v-if="open">
        <div class="modal-head">
          <div class="modal-title">{{ isEdit ? '✏️ Edit' : '➕ New' }} {{ singular }}</div>
          <button class="modal-close-btn" @click="$emit('close')">✕</button>
        </div>
        <div class="modal-body">
          <slot />
        </div>
        <div class="modal-footer">
          <button class="btn btn-ghost" @click="$emit('close')">Cancel</button>
          <button class="btn btn-primary" @click="$emit('save')" :disabled="saving">
            {{ saving ? '⏳ Saving...' : (isEdit ? '💾 Update' : '➕ Create') }}
          </button>
        </div>
      </div>
    </div>
  </Teleport>
</template>

<style scoped>
.overlay { display: none; position: fixed; inset: 0; background: rgba(0,0,0,0.75); z-index: 300; align-items: center; justify-content: center; backdrop-filter: blur(6px); }
.overlay.open { display: flex; }
.modal { background: var(--surface); border: 1px solid var(--border2); border-radius: 20px; width: 480px; max-width: 95vw; box-shadow: var(--shadow); animation: modalIn 0.22s cubic-bezier(0.34,1.56,0.64,1); overflow: hidden; }
@keyframes modalIn { from{transform:scale(0.9) translateY(20px);opacity:0} to{transform:scale(1) translateY(0);opacity:1} }
.modal-head { padding: 24px 28px 20px; border-bottom: 1px solid var(--border); display: flex; align-items: center; justify-content: space-between; }
.modal-title { font-size: 18px; font-weight: 800; }
.modal-close-btn { width: 32px; height: 32px; border-radius: 8px; background: var(--surface2); border: 1px solid var(--border); color: var(--text2); font-size: 16px; cursor: pointer; display: flex; align-items: center; justify-content: center; transition: all 0.15s; font-family: var(--font); }
.modal-close-btn:hover { background: var(--red-dim); border-color: var(--red); color: var(--red); }
.modal-body { padding: 24px 28px; display: flex; flex-direction: column; gap: 16px; }
.modal-footer { padding: 16px 28px 24px; display: flex; gap: 10px; justify-content: flex-end; }
</style>
