import axios from 'axios'
import type { Activity, CreateActivityDto, UpdateActivityDto, FluentValidationError } from '../types/activity.types'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL

if (!API_BASE_URL) {
  console.error('ERROR CRÍTICO: No se detectó VITE_API_BASE_URL en frontend/.env')
}

export const apiClient = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json'
    }
})

// GET /api/Activity
export const getActivitiesAsync = async (): Promise<Activity[]> => {
  const response = await apiClient.get<Activity[]>('/Activity')

  if (!Array.isArray(response.data)) {
    throw new Error('La respuesta recibida no es un arreglo JSON válido')
  }

  return response.data
}

// POST /api/Activity
export const createActivityAsync = async (newActivity: CreateActivityDto): Promise<Activity> => {
  const response = await apiClient.post<Activity>('/Activity', newActivity)
  return response.data
}

// PUT /api/Activity/{id}
export const updateActivityAsync = async (id: number, activity: UpdateActivityDto): Promise<void> => {
  await apiClient.put(`/Activity/${id}`, activity)
}

// DELETE /api/Activity/{id}
export const deleteActivityAsync = async (id: number): Promise<void> => {
  await apiClient.delete(`/Activity/${id}`)
}

// Artefacto extractor de los mensajes de error del backend
export const extractBackendErrorMessage = (error: unknown): string => {
    if (axios.isAxiosError(error)) {
        // Caso A: El servidor .NET está apagado o el navegador bloqueó por CORS
        if (!error.response) {
            return 'Sin respuesta del servidor (.NET apagado o bloqueo de seguridad CORS en el navegador).'
        }

        const status = error.response.status
        const data = error.response.data

        // Caso B: Arreglo de errores devuelto por FluentValidation en .NET -> [ { propertyName, errorMessage }, ... ]
        if (Array.isArray(data)) {
            const fluentErrors = (data as FluentValidationError[])
                .map((item) => item?.errorMessage)
                .filter((msg): msg is string => typeof msg === 'string' && msg.trim().length > 0)

            if (fluentErrors.length > 0) {
                return `[HTTP ${status}] ${fluentErrors.join(' | ')}`
            }
        }

        // Caso C: El controlador de .NET devolvió un string directo -> BadRequest("Mensaje...")
        if (typeof data === 'string' && data.trim().length > 0) {
            return `[HTTP ${status}] ${data}`
        }

        // Caso D: El servidor devolvió un objeto JSON (ProblemDetails o { error / mensaje })
        if (data && typeof data === 'object') {
            const obj = data as Record<string, unknown>

            if ('errors' in obj && obj.errors && typeof obj.errors === 'object') {
                const errorList = Object.values(obj.errors as Record<string, string[]>).flat()
                if (errorList.length > 0) {
                    return `[HTTP ${status}] ${errorList.join(' | ')}`
                }
            }

            const explicitMessage = obj.error || obj.mensaje || obj.message || obj.detail || obj.title
            if (typeof explicitMessage === 'string' && explicitMessage.trim().length > 0) {
                return `[HTTP ${status}] ${explicitMessage}`
            }
        }

        return `[HTTP ${status}] El servidor .NET rechazó la operación.`
    }

    return error instanceof Error ? error.message : 'Error inesperado en la aplicación.'
}
