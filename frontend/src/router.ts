import { createRouter, createWebHistory } from 'vue-router'
import HomeView from './views/HomeView.vue'

export const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', name: 'home', component: HomeView },
    { path: '/mapa', name: 'map', component: () => import('./views/MapView.vue') },
    { path: '/forum', name: 'forum', component: () => import('./views/ForumView.vue') },
    { path: '/forum/:id', name: 'thread', component: () => import('./views/ThreadView.vue'), props: true },
    { path: '/logowanie', name: 'login', component: () => import('./views/AuthView.vue') },
    { path: '/status', name: 'status', component: () => import('./views/StatusView.vue') },
  ],
})
