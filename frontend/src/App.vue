<script setup lang="ts">
import { useRouter } from 'vue-router'
import { auth } from './stores/auth'

const router = useRouter()

function logout() {
  auth.logout()
  router.push('/')
}
</script>

<template>
  <header class="topbar">
    <RouterLink to="/" class="brand">
      <img src="/favicon.svg" alt="" width="28" height="28" />
      Atlas grzybów
    </RouterLink>
    <nav>
      <RouterLink to="/mapa">Mapa</RouterLink>
      <RouterLink to="/forum">Forum</RouterLink>
      <RouterLink to="/status">Stan systemu</RouterLink>
    </nav>
    <div class="account">
      <template v-if="auth.isLoggedIn">
        <span class="muted">{{ auth.user?.userName }}</span>
        <button class="link" @click="logout">Wyloguj</button>
      </template>
      <RouterLink v-else to="/logowanie" class="button small">Zaloguj</RouterLink>
    </div>
  </header>
  <main>
    <RouterView />
  </main>
</template>
