<script setup lang="ts">
import type { Forecast } from '../api/types'

defineProps<{ forecast: Forecast }>()

const percent = (p: number) => `${Math.round(p * 100)}%`
</script>

<template>
  <div class="forecast">
    <h3>Prognoza dla punktu</h3>
    <p class="muted small-text">
      {{ forecast.latitude.toFixed(3) }}, {{ forecast.longitude.toFixed(3) }} ·
      {{ forecast.weather.mean_temp_7d.toFixed(1) }}°C (7 dni) ·
      {{ forecast.weather.rain_sum_14d.toFixed(0) }} mm deszczu (14 dni)
      <span class="badge">{{ forecast.weather.source }}</span>
    </p>
    <p><strong>Ogólna szansa: {{ percent(forecast.score) }}</strong></p>
    <ul>
      <li v-for="s in forecast.species.slice(0, 6)" :key="s.species_id">
        <span class="name">{{ s.name }}</span>
        <span class="bar"><span :style="{ width: percent(s.probability) }" /></span>
        <span class="value">{{ percent(s.probability) }}</span>
      </li>
    </ul>
  </div>
</template>

<style scoped>
ul { list-style: none; padding: 0; margin: 0; display: grid; gap: 0.35rem; }
li { display: grid; grid-template-columns: 1fr 80px 40px; gap: 0.5rem; align-items: center; font-size: 0.85rem; }
.bar { height: 8px; background: #eee6d8; border-radius: 4px; overflow: hidden; }
.bar span { display: block; height: 100%; background: linear-gradient(90deg, var(--moss), var(--forest)); }
.value { text-align: right; color: var(--muted); }
</style>
