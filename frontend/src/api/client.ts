import { auth } from '../stores/auth'

export class ApiError extends Error {
  constructor(
    public status: number,
    message: string,
  ) {
    super(message)
  }
}

/** Wszystkie wywołania idą przez bramkę YARP pod /api/{serwis}/... */
export async function api<T>(path: string, init: RequestInit = {}): Promise<T> {
  const headers = new Headers(init.headers)
  if (init.body) headers.set('Content-Type', 'application/json')
  if (auth.token) headers.set('Authorization', `Bearer ${auth.token}`)

  const response = await fetch(`/api${path}`, { ...init, headers })

  if (response.status === 401 && auth.token) auth.logout()

  if (!response.ok) {
    let message = `Błąd ${response.status}`
    try {
      const problem = await response.json()
      const details = problem.errors ? Object.values(problem.errors).flat().join(' ') : ''
      message = [problem.title, details].filter(Boolean).join(': ')
    } catch {
      /* odpowiedź bez ProblemDetails */
    }
    throw new ApiError(response.status, message)
  }

  return response.status === 204 ? (undefined as T) : ((await response.json()) as T)
}

export const post = <T>(path: string, body: unknown) => api<T>(path, { method: 'POST', body: JSON.stringify(body) })
