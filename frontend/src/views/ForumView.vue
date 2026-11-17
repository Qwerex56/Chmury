<script setup lang="ts">
import { onMounted, ref, watch } from 'vue'
import { useRouter } from 'vue-router'
import { api, post } from '../api/client'
import type { ForumCategory, ThreadSummary } from '../api/types'
import { auth } from '../stores/auth'

const categories: ForumCategory[] = ['Dyskusje', 'Znaleziska', 'Pytania', 'Przepisy']
const router = useRouter()

const category = ref<ForumCategory | ''>('')
const threads = ref<ThreadSummary[]>([])
const loading = ref(true)
const error = ref('')

const showForm = ref(false)
const title = ref('')
const newCategory = ref<ForumCategory>('Dyskusje')
const content = ref('')

async function load() {
  loading.value = true
  try {
    threads.value = await api<ThreadSummary[]>(`/forum/threads${category.value ? `?category=${category.value}` : ''}`)
    error.value = ''
  } catch (e) {
    error.value = (e as Error).message
  } finally {
    loading.value = false
  }
}

async function create() {
  try {
    const { id } = await post<{ id: string }>('/forum/threads', {
      title: title.value,
      category: newCategory.value,
      content: content.value,
    })
    router.push(`/forum/${id}`)
  } catch (e) {
    error.value = (e as Error).message
  }
}

const date = (iso: string) => new Date(iso).toLocaleString('pl-PL', { dateStyle: 'medium', timeStyle: 'short' })

onMounted(load)
watch(category, load)
</script>

<template>
  <div class="head">
    <h1>Forum grzybiarzy</h1>
    <button v-if="auth.isLoggedIn" @click="showForm = !showForm">{{ showForm ? 'Anuluj' : 'Nowy wątek' }}</button>
    <RouterLink v-else to="/logowanie" class="muted">Zaloguj się, żeby pisać</RouterLink>
  </div>

  <form v-if="showForm" class="card form" @submit.prevent="create">
    <label>Tytuł <input v-model="title" required minlength="3" /></label>
    <label>
      Kategoria
      <select v-model="newCategory">
        <option v-for="c in categories" :key="c">{{ c }}</option>
      </select>
    </label>
    <label>Treść <textarea v-model="content" required /></label>
    <button>Opublikuj</button>
  </form>

  <div class="tabs">
    <button :class="{ active: category === '' }" @click="category = ''">Wszystkie</button>
    <button v-for="c in categories" :key="c" :class="{ active: category === c }" @click="category = c">{{ c }}</button>
  </div>

  <p v-if="error" class="error">{{ error }}</p>
  <p v-if="loading" class="muted">Ładowanie…</p>
  <ul v-else class="threads">
    <li v-for="t in threads" :key="t.id" class="card">
      <RouterLink :to="`/forum/${t.id}`" class="title">{{ t.title }}</RouterLink>
      <div class="meta muted small-text">
        <span class="badge">{{ t.category }}</span>
        {{ t.authorName }} · {{ t.postCount }} {{ t.postCount === 1 ? 'post' : 'posty' }} · ostatnio {{ date(t.lastActivityAt) }}
      </div>
    </li>
    <li v-if="!threads.length" class="muted">Brak wątków w tej kategorii.</li>
  </ul>
</template>

<style scoped>
.head { display: flex; justify-content: space-between; align-items: center; margin-bottom: 1rem; }
.form { margin-bottom: 1rem; max-width: 640px; }
.tabs { display: flex; gap: 0.5rem; flex-wrap: wrap; margin-bottom: 1rem; }
.tabs button { background: #e9e3d6; color: var(--ink) !important; font-weight: 500; }
.tabs button.active { background: var(--forest); color: #fff !important; }
.threads { list-style: none; padding: 0; display: grid; gap: 0.6rem; }
.title { font-weight: 600; font-size: 1.05rem; text-decoration: none; }
.meta { margin-top: 0.35rem; display: flex; gap: 0.5rem; align-items: center; flex-wrap: wrap; }
</style>
