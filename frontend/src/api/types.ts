export interface UserView {
  id: string
  email: string
  userName: string
  role: string
  createdAt: string
}

export interface AuthToken {
  accessToken: string
  expiresAt: string
  user: UserView
}

export interface Species {
  id: string
  polishName: string
  latinName: string
  edibility: 'Jadalny' | 'Niejadalny' | 'Trujacy'
  seasonFrom: number
  seasonTo: number
  color: string
}

export interface Finding {
  id: string
  speciesId: string
  speciesName: string
  edibility: string
  color: string
  latitude: number
  longitude: number
  foundAt: string
  note: string | null
  authorName: string
}

export interface Weather {
  mean_temp_7d: number
  rain_sum_14d: number
  source: string
}

export interface Forecast {
  latitude: number
  longitude: number
  score: number
  weather: Weather
  species: { species_id: string; name: string; probability: number }[]
}

export interface HeatCell {
  south: number
  west: number
  north: number
  east: number
  score: number
}

export interface Heatmap {
  weather: Weather
  cells: HeatCell[]
}

export type ForumCategory = 'Dyskusje' | 'Znaleziska' | 'Pytania' | 'Przepisy'

export interface ThreadSummary {
  id: string
  title: string
  category: ForumCategory
  authorName: string
  postCount: number
  createdAt: string
  lastActivityAt: string
}

export interface Post {
  id: string
  content: string
  authorName: string
  createdAt: string
}

export interface Thread {
  id: string
  title: string
  category: ForumCategory
  authorName: string
  findingId: string | null
  createdAt: string
  posts: Post[]
}
