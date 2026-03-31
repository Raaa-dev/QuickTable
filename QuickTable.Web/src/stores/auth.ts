import { defineStore } from 'pinia'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    userName: localStorage.getItem('userName') || null as string | null,
    isAuthenticated: !!localStorage.getItem('userName'),
  }),
  actions: {
    setUser(userName: string) {
      this.userName = userName
      this.isAuthenticated = true
      localStorage.setItem('userName', userName)
    },
    clearUser() {
      this.userName = null
      this.isAuthenticated = false
      localStorage.removeItem('userName')
    }
  }
})