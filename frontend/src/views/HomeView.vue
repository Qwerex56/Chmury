<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { api } from '../api/client'
import type { Finding, Species, ThreadSummary } from '../api/types'

const findings = ref<number | null>(null)
const species = ref<number | null>(null)
const threads = ref<number | null>(null)

onMounted(async () => {
  const [f, s, t] = await Promise.allSettled([
    api<Finding[]>('/map/findings?days=30'),
    api<Species[]>('/map/species'),
    api<ThreadSummary[]>('/forum/threads'),
  ])
  if (f.status === 'fulfilled') findings.value = f.value.length
  if (s.status === 'fulfilled') species.value = s.value.length
  if (t.status === 'fulfilled') threads.value = t.value.length
})

const stack = [
  ['Vue 3 + Leaflet', 'frontend'],
  ['YARP', 'bramka API'],
  ['.NET 10', 'Użytkownicy, Forum, Mapa'],
  ['Python / FastAPI', 'Predykcja'],
  ['PostgreSQL + PostGIS', 'dane i geometria'],
  ['RabbitMQ + MassTransit', 'zdarzenia między serwisami'],
  ['Docker Compose + Cloudflare Tunnel', 'uruchomienie'],
]
</script>

<template>
  <section class="hero">
    <div>
      <span class="badge">Demo technologiczne · etap 1</span>
      <h1>Atlas grzybów</h1>
      <p>
        Mapa znalezisk, przewidywanie gdzie i jakich grzybów można się spodziewać
        oraz forum dla grzybiarzy. W jednym miejscu.
      </p>
      <div class="actions">
        <RouterLink to="/mapa" class="button">Otwórz mapę</RouterLink>
        <RouterLink to="/forum" class="button secondary-btn">Przejdź do forum</RouterLink>
      </div>
    </div>
    <div class="stats card">
      <div><strong>{{ findings ?? '–' }}</strong><span>znalezisk z 30 dni</span></div>
      <div><strong>{{ species ?? '–' }}</strong><span>gatunków w atlasie</span></div>
      <div><strong>{{ threads ?? '–' }}</strong><span>wątków na forum</span></div>
    </div>
  </section>

  <section class="grid features">
    <RouterLink to="/mapa" class="card feature">
      <h3>🗺️ Mapa grzybów</h3>
      <p class="muted">Znaleziska społeczności na mapie Polski, filtrowane po gatunku i czasie.</p>
    </RouterLink>
    <RouterLink to="/mapa" class="card feature">
      <h3>🌦️ Predykcja</h3>
      <p class="muted">Warstwa szans na grzyby liczona z pogody (Open-Meteo) i sezonu każdego gatunku.</p>
    </RouterLink>
    <RouterLink to="/forum" class="card feature">
      <h3>💬 Forum</h3>
      <p class="muted">Dyskusje, pytania o oznaczanie i automatyczne wątki z nowych znalezisk.</p>
    </RouterLink>
  </section>

  <section class="card">
    <h2>Stos technologiczny</h2>
    <ul class="stack">
      <li v-for="[tech, role] in stack" :key="tech"><strong>{{ tech }}</strong><span class="muted">{{ role }}</span></li>
    </ul>
  </section>
</template>

<style scoped>
.hero { display: grid; grid-template-columns: 1.4fr 1fr; gap: 2rem; align-items: center; margin: 1rem 0 2rem; }
.hero h1 { font-size: 2.6rem; color: var(--forest-dark); margin-top: 0.5rem; }
.hero p { font-size: 1.1rem; max-width: 36rem; }
.actions { display: flex; gap: 0.75rem; flex-wrap: wrap; }
.secondary-btn { background: var(--cap); }
.stats { display: grid; gap: 1rem; }
.stats div { display: flex; align-items: baseline; gap: 0.75rem; }
.stats strong { font-size: 2rem; color: var(--cap); min-width: 3.5rem; }
.features { margin-bottom: 1.5rem; }
.feature { text-decoration: none; color: inherit; transition: transform 0.15s; }
.feature:hover { transform: translateY(-2px); border-color: var(--moss); }
.stack { list-style: none; padding: 0; margin: 0; display: grid; gap: 0.5rem; grid-template-columns: repeat(auto-fit, minmax(260px, 1fr)); }
.stack li { display: flex; flex-direction: column; padding: 0.5rem 0.75rem; border-left: 3px solid var(--moss); background: #f7f4ec; }
@media (max-width: 800px) { .hero { grid-template-columns: 1fr; } }
</style>
