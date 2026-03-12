import { createRouter, createWebHistory } from "vue-router";

const routes = [
  { path: "/", component: () => import("../views/list-menu.vue") },
  { path: "/cart", component: () => import("../views/order.vue") },
  // Dynamic route example
  //   { path: '/user/:id', component: () => import('../views/User.vue') }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
