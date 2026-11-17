<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { api, post } from '../api/client'
import type { Thread } from '../api/types'
import { auth } from '../stores/auth'

const props = defineProps<{ id: string }>()

const thread = ref<Thread | null>(null)
const reply = ref('')
const error = ref('')

async function load() {
  try {
    thread.value = await api<Thread>(`/forum/threads/${props.id}`)
  } catch (e) {
    error.value = (e as Error).message
  }
}

async function send() {
  try {
    await post(`/forum/threads/${props.id}/posts`, { content: reply.value })
    reply.value = ''
    await load()
  } catch (e) {
    error.value = (e as Error).message
  }
}

const date = (iso: string) => new Date(iso).toLocaleString('pl-PL', { dateStyle: 'medium', timeStyle: 'short' })

onMounted(load)
</script>

<template>
  <RouterLink to="/forum">← Wróć do forum</RouterLink>
  <p v-if="error" class="error">{{ error }}</p>
  <template v-if="thread">
    <h1 class="title">{{ thread.title }}</h1>
    <p class="muted"><span class="badge">{{ thread.category }}</span> założony przez {{ thread.authorName }}</p>
    <p v-if="thread.findingId" class="small-text">
      Wątek utworzony automatycznie z <RouterLink to="/mapa">mapy znalezisk</RouterLink>.
    </p>

    <article v-for="p in thread.posts" :key="p.id" class="card post">
      <header class="muted small-text"><strong>{{ p.authorName }}</strong> · {{ date(p.createdAt) }}</header>
      <p>{{ p.content }}</p>
    </article>

    <form v-if="auth.isLoggedIn" class="card" @submit.prevent="send">
      <label>Twoja odpowiedź <textarea v-model="reply" required /></label>
      <button>Odpowiedz</button>
    </form>
    <p v-else class="muted"><RouterLink to="/logowanie">Zaloguj się</RouterLink>, żeby odpowiedzieć.</p>
  </template>
</template>

<style scoped>
.title { margin-top: 1rem; }
.post { margin-bottom: 0.75rem; }
.post p { white-space: pre-line; margin: 0.5rem 0 0; }
</style>
