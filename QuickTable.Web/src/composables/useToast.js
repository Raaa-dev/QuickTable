import { ref, reactive } from 'vue'

const toasts = ref([])
let tid = 0

export function useToast() {
  function toast(msg, type = 'info') {
    const id = ++tid
    const t = reactive({ id, msg, type, show: false }) // 👈 reactive
    toasts.value.push(t)
    setTimeout(() => (t.show = true), 10)
    setTimeout(() => {
      t.show = false
      setTimeout(() => { toasts.value = toasts.value.filter(x => x.id !== id) }, 400)
    }, 3000)
  }

  return { toasts, toast }
}