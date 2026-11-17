<script setup lang="ts">
import { ref } from 'vue'
import { post } from '../api/client'
import type { Species } from '../api/types'

const props = defineProps<{ latitude: number; longitude: number; species: Species[] }>()
const emit = defineEmits<{ saved: [] }>()

const speciesId = ref(props.species[0]?.id ?? '')
const note = ref('')
const error = ref('')
const saving = ref(false)

async function save() {
  saving.value = true
  error.value = ''
  try {
    await post('/map/findings', {
      speciesId: speciesId.value,
      latitude: props.latitude,
      longitude: props.longitude,
      note: note.value || null,
    })
    note.value = ''
    emit('saved')
  } catch (e) {
    error.value = (e as Error).message
  } finally {
    saving.value = false
  }
}
</script>

<template>
  <form @submit.prevent="save">
    <h3>Dodaj znalezisko tutaj</h3>
    <label>
      Gatunek
      <select v-model="speciesId">
        <option v-for="s in species" :key="s.id" :value="s.id">{{ s.polishName }}</option>
      </select>
    </label>
    <label>
      Notatka (opcjonalnie)
      <textarea v-model="note" placeholder="np. 5 sztuk pod brzozami" />
    </label>
    <p v-if="error" class="error small-text">{{ error }}</p>
    <button :disabled="saving">{{ saving ? 'Zapisuję…' : 'Zapisz znalezisko' }}</button>
    <p class="muted small-text">Znalezisko pojawi się też jako wątek na forum (przez RabbitMQ).</p>
  </form>
</template>
