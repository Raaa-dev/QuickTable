<script setup lang="ts">
import { reactive, ref } from "vue";
import { useRouter, useRoute } from "vue-router";
import { useAuthStore } from "@/stores/auth";
import api from "@/api/axios";

import "@/assets/admin.css";

// const API_BASE = import.meta.env.VITE_API_BASE_URL
const router = useRouter();
const route = useRoute();
const auth = useAuthStore();

const form = reactive({ userName: "", password: "" });
const error = ref("");
const loading = ref(false);

const login = async () => {
  error.value = ""
  loading.value = true
  try {
    const res = await api.post("/api/v1/auth/login", form)
    auth.setUser(res.data.userName)
    const redirect = (route.query.redirect as string) || "/admin"
    router.push(redirect)
  } catch (err: any) {
    error.value = err.response?.data?.message || "Login failed"
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="login-wrap">
    <div class="login-card">
      <h2 class="login-title">🔐 Admin Login</h2>

      <div v-if="error" class="error-msg">{{ error }}</div>

      <div class="form-group">
        <label class="form-label">Username</label>
        <input
          v-model="form.userName"
          class="form-input"
          placeholder="Username"
          @keyup.enter="login"
        />
      </div>

      <div class="form-group">
        <label class="form-label">Password</label>
        <input
          v-model="form.password"
          type="password"
          class="form-input"
          placeholder="Password"
          @keyup.enter="login"
        />
      </div>

      <button class="btn-login" @click="login" :disabled="loading">
        {{ loading ? "Logging in..." : "Login" }}
      </button>
    </div>
  </div>
</template>

<style scoped>
.login-wrap {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: var(--bg);
}
.login-card {
  background: var(--surface);
  border: 1px solid var(--border);
  border-radius: var(--radius);
  padding: 40px;
  width: 100%;
  max-width: 380px;
  display: flex;
  flex-direction: column;
  gap: 18px;
}
.login-title {
  font-size: 20px;
  font-weight: 700;
  text-align: center;
  margin: 0;
}
.error-msg {
  background: rgba(239, 68, 68, 0.1);
  border: 1px solid var(--red);
  color: var(--red);
  padding: 10px 14px;
  border-radius: var(--radius);
  font-size: 13px;
}
.form-group {
  display: flex;
  flex-direction: column;
  gap: 6px;
}
.form-label {
  font-size: 11px;
  color: var(--text2);
  font-family: var(--mono);
  letter-spacing: 1.5px;
  text-transform: uppercase;
}
.form-input {
  background: var(--surface2);
  border: 1px solid var(--border);
  border-radius: var(--radius);
  padding: 10px 14px;
  color: var(--text);
  font-size: 14px;
  outline: none;
  transition: border 0.15s;
}
.form-input:focus {
  border-color: var(--accent);
  box-shadow: 0 0 0 3px var(--accent-dim);
}
.btn-login {
  background: var(--accent);
  color: #fff;
  border: none;
  border-radius: var(--radius);
  padding: 12px;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  transition: opacity 0.15s;
}
.btn-login:hover {
  opacity: 0.85;
}
.btn-login:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
</style>
