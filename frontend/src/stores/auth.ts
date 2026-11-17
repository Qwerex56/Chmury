import { reactive } from 'vue'
import type { AuthToken, UserView } from '../api/types'

const STORAGE_KEY = 'atlas.auth'

function load(): AuthToken | null {
  try {
    const saved = JSON.parse(localStorage.getItem(STORAGE_KEY) ?? 'null') as AuthToken | null
    return saved && new Date(saved.expiresAt) > new Date() ? saved : null
  } catch {
    return null
  }
}

const initial = load()

export const auth = reactive({
  token: initial?.accessToken ?? (null as string | null),
  user: initial?.user ?? (null as UserView | null),

  get isLoggedIn() {
    return this.token !== null
  },

  signIn(token: AuthToken) {
    this.token = token.accessToken
    this.user = token.user
    localStorage.setItem(STORAGE_KEY, JSON.stringify(token))
  },

  logout() {
    this.token = null
    this.user = null
    localStorage.removeItem(STORAGE_KEY)
  },
})
