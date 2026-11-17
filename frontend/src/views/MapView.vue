<script setup lang="ts">
import { onBeforeUnmount, onMounted, ref, shallowRef, watch } from 'vue'
import L from 'leaflet'
import { api } from '../api/client'
import type { Finding, Forecast, Heatmap, Species } from '../api/types'
import { auth } from '../stores/auth'
import ForecastPanel from '../components/ForecastPanel.vue'
import FindingForm from '../components/FindingForm.vue'

const mapElement = ref<HTMLDivElement>()
const map = shallowRef<L.Map>()
const findingsLayer = L.layerGroup()
const heatLayer = L.layerGroup()
let pointMarker: L.Marker | null = null

const species = ref<Species[]>([])
const speciesFilter = ref('')
const days = ref(30)
const showHeat = ref(true)
const findingsCount = ref(0)
const heatSource = ref('')

const selected = ref<{ lat: number; lng: number } | null>(null)
const forecast = ref<Forecast | null>(null)
const error = ref('')

function bbox() {
  const b = map.value!.getBounds()
  return { south: b.getSouth(), west: b.getWest(), north: b.getNorth(), east: b.getEast() }
}

const escapeHtml = (text: string) =>
  text.replace(/[&<>"']/g, (c) => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' })[c]!)

function query(params: Record<string, string | number>) {
  return new URLSearchParams(Object.entries(params).map(([k, v]) => [k, String(v)])).toString()
}

async function loadFindings() {
  const params: Record<string, string | number> = { ...bbox(), days: days.value }
  if (speciesFilter.value) params.species = speciesFilter.value
  try {
    const findings = await api<Finding[]>(`/map/findings?${query(params)}`)
    findingsLayer.clearLayers()
    findingsCount.value = findings.length
    for (const f of findings) {
      L.circleMarker([f.latitude, f.longitude], {
        radius: 7,
        color: '#fff',
        weight: 1.5,
        fillColor: f.color,
        fillOpacity: 0.95,
      })
        .bindPopup(
          `<strong>${escapeHtml(f.speciesName)}</strong><br>${new Date(f.foundAt).toLocaleDateString('pl-PL')} · ${escapeHtml(f.authorName)}` +
            (f.note ? `<br><em>${escapeHtml(f.note)}</em>` : ''),
        )
        .addTo(findingsLayer)
    }
    error.value = ''
  } catch (e) {
    error.value = `Mapa: ${(e as Error).message}`
  }
}

function heatColor(score: number) {
  // od bladej zieleni (mała szansa) do brązu kapelusza (duża szansa)
  const hue = 95 - score * 75
  return `hsl(${hue}, 60%, ${55 - score * 20}%)`
}

async function loadHeatmap() {
  heatLayer.clearLayers()
  if (!showHeat.value) return
  try {
    const heat = await api<Heatmap>(`/prediction/heatmap?${query({ ...bbox(), size: 18 })}`)
    heatSource.value = heat.weather.source
    for (const c of heat.cells) {
      L.rectangle(
        [
          [c.south, c.west],
          [c.north, c.east],
        ],
        { stroke: false, fillColor: heatColor(c.score), fillOpacity: 0.15 + c.score * 0.45, interactive: false },
      ).addTo(heatLayer)
    }
  } catch (e) {
    error.value = `Predykcja: ${(e as Error).message}`
  }
}

async function selectPoint(lat: number, lng: number) {
  selected.value = { lat, lng }
  pointMarker?.remove()
  pointMarker = L.marker([lat, lng]).addTo(map.value!)
  forecast.value = null
  try {
    forecast.value = await api<Forecast>(`/prediction/forecast?${query({ lat, lon: lng })}`)
  } catch (e) {
    error.value = `Predykcja: ${(e as Error).message}`
  }
}

function refresh() {
  loadFindings()
  loadHeatmap()
}

onMounted(async () => {
  // Ikona markera Leaflet nie działa z bundlerem bez wskazania ścieżek.
  L.Icon.Default.mergeOptions({
    iconUrl: new URL('leaflet/dist/images/marker-icon.png', import.meta.url).href,
    iconRetinaUrl: new URL('leaflet/dist/images/marker-icon-2x.png', import.meta.url).href,
    shadowUrl: new URL('leaflet/dist/images/marker-shadow.png', import.meta.url).href,
  })

  map.value = L.map(mapElement.value!, { zoomControl: true }).setView([52.0, 19.4], 6)
  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 18,
    attribution: '&copy; OpenStreetMap',
  }).addTo(map.value)
  heatLayer.addTo(map.value)
  findingsLayer.addTo(map.value)
  map.value.on('moveend', refresh)
  map.value.on('click', (e: L.LeafletMouseEvent) => selectPoint(e.latlng.lat, e.latlng.lng))

  try {
    species.value = await api<Species[]>('/map/species')
  } catch (e) {
    error.value = `Mapa: ${(e as Error).message}`
  }
  refresh()
})

onBeforeUnmount(() => map.value?.remove())

watch([speciesFilter, days], loadFindings)
watch(showHeat, loadHeatmap)

function onSaved() {
  loadFindings()
}
</script>

<template>
  <div class="layout">
    <aside class="card side">
      <h2>Mapa grzybów</h2>
      <label>
        Gatunek
        <select v-model="speciesFilter">
          <option value="">Wszystkie</option>
          <option v-for="s in species" :key="s.id" :value="s.id">{{ s.polishName }}</option>
        </select>
      </label>
      <label>
        Okres
        <select v-model.number="days">
          <option :value="7">Ostatnie 7 dni</option>
          <option :value="30">Ostatnie 30 dni</option>
          <option :value="365">Ostatni rok</option>
        </select>
      </label>
      <label class="check"><input v-model="showHeat" type="checkbox" /> Warstwa predykcji</label>
      <p class="muted small-text">
        {{ findingsCount }} znalezisk w widoku<span v-if="showHeat && heatSource"> · pogoda: {{ heatSource }}</span>
      </p>
      <p v-if="error" class="error small-text">{{ error }}</p>

      <hr />
      <p v-if="!selected" class="muted">Kliknij w mapę, żeby zobaczyć prognozę dla miejsca.</p>
      <ForecastPanel v-if="forecast" :forecast="forecast" />
      <template v-if="selected">
        <hr />
        <FindingForm
          v-if="auth.isLoggedIn && species.length"
          :key="`${selected.lat},${selected.lng}`"
          :latitude="selected.lat"
          :longitude="selected.lng"
          :species="species"
          @saved="onSaved"
        />
        <p v-else class="muted small-text">
          <RouterLink to="/logowanie">Zaloguj się</RouterLink>, żeby dodać znalezisko w tym miejscu.
        </p>
      </template>

      <hr />
      <h3>Legenda</h3>
      <ul class="legend">
        <li v-for="s in species" :key="s.id">
          <span class="dot" :style="{ background: s.color }" />
          {{ s.polishName }}
          <span v-if="s.edibility === 'Trujacy'" class="badge poison">trujący</span>
        </li>
      </ul>
    </aside>
    <div ref="mapElement" class="map" />
  </div>
</template>

<style scoped>
.layout { display: grid; grid-template-columns: 320px 1fr; gap: 1rem; height: calc(100vh - 7rem); min-height: 520px; }
.side { overflow-y: auto; }
.map { border-radius: var(--radius); border: 1px solid var(--line); min-height: 400px; }
.check { display: flex; align-items: center; gap: 0.5rem; }
.check input { width: auto; margin: 0; }
hr { border: 0; border-top: 1px solid var(--line); margin: 1rem 0; }
.legend { list-style: none; padding: 0; margin: 0; display: grid; gap: 0.3rem; font-size: 0.85rem; }
.dot { display: inline-block; width: 10px; height: 10px; border-radius: 50%; margin-right: 0.4rem; }
.poison { background: #fdecea; color: var(--danger); }
@media (max-width: 800px) {
  .layout { grid-template-columns: 1fr; height: auto; }
  .map { height: 60vh; order: -1; }
}
</style>
