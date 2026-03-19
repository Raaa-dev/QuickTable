import { createRouter, createWebHistory } from "vue-router";
import type { RouteRecordRaw } from "vue-router";

const routes: RouteRecordRaw[] = [
  { path: "/", component: () => import("@/views/list-menu.vue") },
  { path: "/order", component: () => import("@/views/order.vue") },
  { path: "/history", component: () => import("@/views/history.vue") },
  // ── Admin routes (with sidebar) ──
  {
    path: "/admin",
    component: () => import("@/layouts/AdminLayout.vue"),
    children: [
      { path: "",              redirect: "/admin/menu-category" }, 
      { path: "menu-category", component: () => import("@/pages/categories/index.vue") },
      { path: "menu-item",     component: () => import("@/pages/menu-items/index.vue") },
      { path: "table",         component: () => import("@/pages/tables/index.vue") },
      { path: "reset-table",         component: () => import("@/pages/reset-table/index.vue") },

    ]
  },
  // { path: "/admin/order", component: () => import("@/pages/order.vue") },
  // { path: "/admin/generate-qr", component: () => import("@/pages/generate-qr.vue") },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;