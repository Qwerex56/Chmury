<script setup lang="ts">
import { onMounted, ref } from 'vue'

interface ServiceStatus {
  name: string
  description: string
  url: string
  state: 'checking' | 'ok' | 'down'
  ms?: number
}

const services = ref<ServiceStatus[]>([
  { name: 'Bramka (YARP)', description: 'routing /api/* do serwisów', url: '/health', state: 'checking' },
  { name: 'Użytkownicy', description: '.NET 10, JWT, PostgreSQL', url: '/api/users/health', state: 'checking' },
  { name: 'Forum', description: '.NET 10, PostgreSQL, konsument RabbitMQ', url: '/api/forum/health', state: 'checking' },
  { name: 'Mapa', description: '.NET 10, PostGIS, wydawca RabbitMQ', url: '/api/map/health', state: 'checking' },
  { name: 'Predykcja', description: 'Python / FastAPI, Open-Meteo', url: '/api/prediction/health', state: 'checking' },
])

async function check() {
  await Promise.all(
    services.value.map(async (s) => {
      s.state = 'checking'
      const started = performance.now()
      try {
        const response = await fetch(s.url)
        s.state = response.ok ? 'ok' : 'down'
      } catch {
        s.state = 'down'
      }
      s.ms = Math.round(performance.now() - started)
    }),
  )
}

onMounted(check)
</script>

<template>
  <div class="head">
    <h1>Stan systemu</h1>
    <button class="secondary" @click="check">Odśwież</button>
  </div>
  <div class="grid">
    <div v-for="s in services" :key="s.name" class="card service">
      <span class="light" :class="s.state" />
      <div>
        <strong>{{ s.name }}</strong>
        <p class="muted small-text">{{ s.description }}</p>
        <p class="small-text">
          {{ s.state === 'ok' ? 'działa' : s.state === 'down' ? 'niedostępny' : 'sprawdzam…' }}
          <span v-if="s.ms !== undefined" class="muted">· {{ s.ms }} ms</span>
        </p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.head { display: flex; justify-content: space-between; align-items: center; margin-bottom: 1rem; }
.service { display: flex; gap: 0.75rem; align-items: flex-start; }
.service p { margin: 0.25rem 0 0; }
.light { width: 14px; height: 14px; border-radius: 50%; margin-top: 4px; background: #bbb; flex-shrink: 0; }
.light.ok { background: #43a047; box-shadow: 0 0 0 4px #e3f2e4; }
.light.down { background: var(--danger); box-shadow: 0 0 0 4px #fdecea; }
</style>
