// frontend/src/features/activities/components/EditActivityForm.tsx
import { useState, type FormEvent } from 'react'
import type { Activity } from '../types/activity.types'
import { updateActivityAsync, extractBackendErrorMessage } from '../api/activitiesApi'

interface EditActivityFormProps {
    activity: Activity
    onActivityUpdated: (updatedActivity: Activity) => void
    onCancel: () => void
}

export function EditActivityForm({ activity, onActivityUpdated, onCancel }: EditActivityFormProps) {
    const [name, setName] = useState<string>(activity.nombre)
    const [isSaving, setIsSaving] = useState<boolean>(false)
    const [serverError, setServerError] = useState<string | null>(null)

    const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault()

        try {
            setIsSaving(true)
            setServerError(null)

            await updateActivityAsync(activity.id, { nombre: name })

            // PUT responde 204 No Content: no hay JSON de vuelta, armamos el objeto con los datos enviados
            onActivityUpdated({ id: activity.id, nombre: name })
        } catch (error) {
            const backendMessage = extractBackendErrorMessage(error)
            console.error('🚨 Auditoría del rechazo de .NET (PUT):', error)
            setServerError(backendMessage)
        } finally {
            setIsSaving(false)
        }
    }

    return (
        <form onSubmit={handleSubmit} className="bg-surface border border-line rounded-2xl p-5 mb-6 shadow-sm">
            <div className="flex items-center justify-between gap-3 mb-4">
                <h2 className="text-base font-bold text-ink">Editar Actividad #{activity.id}</h2>
                <span className="text-xs font-mono text-brand-700 bg-brand-100 px-2 py-1 rounded border border-brand-200">
                    PUT /api/Activity/{activity.id}
                </span>
            </div>

            <div className="flex flex-col sm:flex-row gap-3">
                <input
                    type="text"
                    placeholder="Escribe el nombre de la actividad..."
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    disabled={isSaving}
                    autoFocus
                    className="flex-1 bg-surface border border-line rounded-lg px-4 py-2.5 text-sm text-ink placeholder-ink-subtle focus:outline-none focus:border-brand-600 focus:ring-2 focus:ring-brand-600/20 transition disabled:opacity-60"
                />
                <button
                    type="submit"
                    disabled={isSaving}
                    className="bg-brand-600 hover:bg-brand-700 text-white font-medium text-sm px-5 py-2.5 rounded-lg shadow-sm transition-colors cursor-pointer focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-600 focus-visible:ring-offset-2 focus-visible:ring-offset-canvas disabled:opacity-60 disabled:cursor-not-allowed"
                >
                    {isSaving ? '⏳ Guardando...' : 'Guardar cambios'}
                </button>
                <button
                    type="button"
                    onClick={onCancel}
                    disabled={isSaving}
                    className="border border-line bg-canvas text-ink-muted hover:text-ink hover:bg-brand-50 font-medium text-sm px-4 py-2.5 rounded-lg transition-colors cursor-pointer disabled:opacity-60 disabled:cursor-not-allowed"
                >
                    Cancelar
                </button>
            </div>

            {serverError && (
                <div className="mt-4 bg-red-50 border border-red-300 rounded-lg p-3 text-xs text-red-700 flex items-center gap-2">
                    <span className="text-base">🛡️</span>
                    <span><strong>Rechazo del Servidor (.NET FluentValidation):</strong> {serverError}</span>
                </div>
            )}
        </form>
    )
}
