<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { post } from '../api/client'
import type { AuthToken } from '../api/types'
import { auth } from '../stores/auth'

const router = useRouter()
const mode = ref<'login' | 'register'>('login')
const email = ref('demo@atlas.pl')
const userName = ref('')
const password = ref('Grzybek123!')
const error = ref('')
const busy = ref(false)

async function submit() {
  busy.value = true
  error.value = ''
  try {
    const token =
      mode.value === 'login'
        ? await post<AuthToken>('/users/auth/login', { email: email.value, password: password.value })
        : await post<AuthToken>('/users/auth/register', {
            email: email.value,
            userName: userName.value,
            password: password.value,
          })
    auth.signIn(token)
    router.push('/mapa')
  } catch (e) {
    error.value = (e as Error).message
  } finally {
    busy.value = false
  }
}

function switchMode(next: 'login' | 'register') {
  mode.value = next
  error.value = ''
  if (next === 'register') {
    email.value = ''
    password.value = ''
  }
}
</script>

<template>
  <div class="card auth">
    <div class="tabs">
      <button :class="{ active: mode === 'login' }" @click="switchMode('login')">Logowanie</button>
      <button :class="{ active: mode === 'register' }" @click="switchMode('register')">Rejestracja</button>
    </div>
    <form @submit.prevent="submit">
      <label>E-mail <input v-model="email" type="email" required autocomplete="email" /></label>
      <label v-if="mode === 'register'">Nazwa użytkownika <input v-model="userName" required minlength="3" /></label>
      <label>Hasło <input v-model="password" type="password" required autocomplete="current-password" /></label>
      <p v-if="error" class="error small-text">{{ error }}</p>
      <button :disabled="busy">{{ mode === 'login' ? 'Zaloguj' : 'Załóż konto' }}</button>
    </form>
    <p v-if="mode === 'login'" class="muted small-text">Konto demo: demo@atlas.pl / Grzybek123!</p>
  </div>
</template>

<style scoped>
.auth { max-width: 420px; margin: 2rem auto; }
.tabs { display: flex; gap: 0.5rem; margin-bottom: 1rem; }
.tabs button { flex: 1; background: #e9e3d6; color: var(--ink) !important; }
.tabs button.active { background: var(--forest); color: #fff !important; }
</style>
