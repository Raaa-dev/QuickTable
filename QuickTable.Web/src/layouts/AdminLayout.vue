<script setup>
import { useRoute, useRouter  } from "vue-router";
import { useToast } from "@/composables/useToast";
import '@/assets/admin.css'
import { useAuthStore } from "@/stores/auth";
import { authLogout } from "@/composables/useApi" 
import api from "@/api/axios";

const route = useRoute();
const router = useRouter();
const { toasts } = useToast();
const auth = useAuthStore();

const navItems = [
  { path: "/admin/menu-category", label: "Categories", icon: "🗂️" },
  { path: "/admin/menu-item", label: "Menu Items", icon: "🍽️" },
  { path: "/admin/table", label: "Tables", icon: "🪑" },
  { path: "/admin/reset-table", label: "Reset", icon: "🧹"  },
];

const logout = async () => {
  await authLogout()
  auth.clearUser()
  router.push('/login')
}

</script>

<template>
  <div class="layout">
    <aside class="sidebar">
      <div class="sidebar-logo">
        <div class="logo-mark">
          <div class="logo-icon">🍜</div>
          <div class="logo-text">QuickTable</div>
        </div>
        <div class="logo-sub">Admin Portal</div>
      </div>
      <nav class="sidebar-nav">
        <div class="nav-section-label">Management</div>
        <RouterLink
          v-for="item in navItems"
          :key="item.path"
          :to="item.path"
          class="nav-btn"
          :class="{ active: route.path === item.path }"
        >
          <div class="nav-icon">{{ item.icon }}</div>
          <span>{{ item.label }}</span>
        </RouterLink>
      </nav>
        <div class="sidebar-footer">
          <div class="user-info">
            <div class="user-avatar">👤</div>
            <span class="user-name">{{ auth?.userName }}</span>
          </div>
          <button class="logout-btn" @click="logout">🚪 Logout</button>
          <!-- <div class="api-indicator">
            <div class="api-dot"></div>
            <span>API Connected</span>
          </div> -->
        </div>
    </aside>

    <div class="main">
      <RouterView />
    </div>

    <!-- Toasts -->
    <div class="toast-stack">
      <div
        v-for="t in toasts"
        :key="t.id"
        class="toast-item"
        :class="[t.type, t.show ? 'show' : '']"
      >
        {{ t.msg }}
      </div>
    </div>
  </div>
</template>

<style scoped>
.layout {
  display: flex;
  min-height: 100vh;
}
.sidebar {
  width: 240px;
  min-height: 100vh;
  background: var(--surface);
  border-right: 1px solid var(--border);
  display: flex;
  flex-direction: column;
  position: fixed;
  top: 0;
  left: 0;
  bottom: 0;
  z-index: 100;
}
.sidebar-logo {
  padding: 28px 24px 24px;
  border-bottom: 1px solid var(--border);
}
.logo-mark {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-bottom: 4px;
}
.logo-icon {
  width: 36px;
  height: 36px;
  background: var(--accent);
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 18px;
  box-shadow: 0 0 20px var(--accent-dim);
}
.logo-text {
  font-size: 18px;
  font-weight: 800;
  letter-spacing: -0.5px;
}
.logo-sub {
  font-size: 11px;
  color: rgb(190, 190, 190);
  font-family: var(--mono);
  letter-spacing: 2px;
  text-transform: uppercase;
}
.sidebar-nav {
  flex: 1;
  padding: 20px 14px;
  display: flex;
  flex-direction: column;
  gap: 4px;
}
.nav-section-label {
  font-size: 10px;
  color: rgb(190, 190, 190);
  font-family: var(--mono);
  letter-spacing: 2px;
  text-transform: uppercase;
  padding: 12px 10px 6px;
}
.nav-btn {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 11px 12px;
  border-radius: var(--radius);
  font-size: 14px;
  font-weight: 600;
  color: var(--text2);
  transition: all 0.15s;
  text-decoration: none;
}
.nav-btn:hover {
  background: var(--surface2);
  color: var(--text);
}
.nav-btn.active {
  background: var(--accent-dim);
  color: var(--accent);
  border: 1px solid #ff6b3530;
}
.nav-icon {
  width: 32px;
  height: 32px;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 16px;
  background: var(--surface3);
  flex-shrink: 0;
}
.nav-btn.active .nav-icon {
  background: var(--accent-dim);
}
.sidebar-footer {
  padding: 16px 14px;
  border-top: 1px solid var(--border);
}
.api-indicator {
  padding: 10px 12px;
  background: var(--surface2);
  border-radius: var(--radius);
  font-size: 11px;
  font-family: var(--mono);
  color: var(--text3);
  display: flex;
  align-items: center;
  gap: 8px;
}
.api-dot {
  width: 7px;
  height: 7px;
  border-radius: 50%;
  background: var(--green);
  box-shadow: 0 0 8px var(--green);
  flex-shrink: 0;
  animation: pulse 2s infinite;
}
@keyframes pulse {
  0%,
  100% {
    opacity: 1;
  }
  50% {
    opacity: 0.4;
  }
}
.main {
  margin-left: 240px;
  flex: 1;
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}
.toast-stack {
  position: fixed;
  bottom: 24px;
  right: 24px;
  z-index: 999;
  display: flex;
  flex-direction: column;
  gap: 8px;
  align-items: flex-end;
}
.toast-item {
  background: var(--surface2);
  border: 1px solid var(--border2);
  border-radius: 12px;
  padding: 12px 18px;
  font-size: 14px;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 10px;
  box-shadow: var(--shadow);
  transform: translateX(120%);
  transition: transform 0.3s cubic-bezier(0.34, 1.56, 0.64, 1);
  max-width: 320px;
}
.toast-item.show {
  transform: translateX(0);
}
.toast-item.success {
  border-color: #22c55e40;
  color: var(--green);
}
.toast-item.error {
  border-color: #ef444440;
  color: var(--red);
}
.toast-item.info {
  border-color: #a78bfa40;
  color: var(--purple);
}

.user-info {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 10px 12px;
  margin-bottom: 8px;
}
.user-avatar { font-size: 18px; }
.user-name { font-size: 13px; font-weight: 600; color: var(--text); }

.logout-btn {
  width: 100%;
  padding: 10px 12px;
  background: transparent;
  border: 1px solid var(--border);
  border-radius: var(--radius);
  color: var(--red);
  font-size: 13px;
  font-weight: 600;
  cursor: pointer;
  text-align: left;
  margin-bottom: 8px;
  transition: all 0.15s;
}
.logout-btn:hover { background: rgba(239,68,68,0.1); border-color: var(--red); }
</style>
