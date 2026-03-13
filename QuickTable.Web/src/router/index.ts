import { createRouter, createWebHistory } from "vue-router";

const routes = [
  { path: "/", component: () => import("../views/list-menu.vue") },
  { path: "/order", component: () => import("../views/order.vue") },
  { path: "/history", component: () => import("../views/history.vue") },

  // Dynamic route example
  //   { path: '/user/:id', component: () => import('../views/User.vue') }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
