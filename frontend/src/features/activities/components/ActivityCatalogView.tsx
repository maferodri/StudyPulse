import { useState, useEffect } from 'react'
import type { Activity } from '../types/activity.types'
import { getActivitiesAsync, deleteActivityAsync, extractBackendErrorMessage } from '../api/activitiesApi'
import { ActivityCard } from './ActivityCard'
import { CreateActivityForm } from './CreateActivityForm'
import { EditActivityForm } from './EditActivityForm'

export function ActivityCatalogView() {
  // --- ZONA 3: CEREBRO DEL MÓDULO ---
  const [activities, setActivities] = useState<Activity[]>([])
  const [loading, setLoading] = useState<boolean>(true)
  const [networkError, setNetworkError] = useState<string | null>(null)
  const [search, setSearch] = useState<string>('')
  // Controla la visibilidad del formulario de creación (POST)
  const [isFormOpen, setIsFormOpen] = useState<boolean>(false)
  // Guarda la actividad en edición (PUT). null = formulario cerrado
  const [editingActivity, setEditingActivity] = useState<Activity | null>(null)
  // Id de la actividad cuyo DELETE está en vuelo (para el spinner)
  const [deletingId, setDeletingId] = useState<number | null>(null)
  // Errores de operaciones que no tienen formulario propio (DELETE)
  const [actionError, setActionError] = useState<string | null>(null)

  useEffect(() => {
    // Disparamos la petición HTTP GET real hacia .NET al montar el componente
    getActivitiesAsync()
      .then((dataFromPostgres) => {
        setActivities(dataFromPostgres)
        setNetworkError(null)
      })
      .catch((error) => {
        console.error('Fallo en la comunicación HTTP con .NET:', error)
        setNetworkError(extractBackendErrorMessage(error))
      })
      .finally(() => {
        // Apaga el estado de carga tanto si hubo éxito como si ocurrió un error
        setLoading(false)
      })
  }, [])

  const handleActivityCreated = (createdActivity: Activity) => {
    setActivities((currentActivities) => [...currentActivities, createdActivity])
    setIsFormOpen(false)
  }

  // PUT: reemplaza la tarjeta manteniendo el orden de la lista
  const handleActivityUpdated = (updatedActivity: Activity) => {
    setActivities((currentActivities) =>
      currentActivities.map((activity) =>
        activity.id === updatedActivity.id ? updatedActivity : activity
      )
    )
    setEditingActivity(null)
  }

  // Abre el formulario de edición y cierra el de creación para no mostrar dos
  const handleEdit = (activity: Activity) => {
    setActionError(null)
    setIsFormOpen(false)
    setEditingActivity(activity)
  }

  // DELETE: confirma, borra en el backend y recién ahí quita la tarjeta
  const handleDelete = async (activity: Activity) => {
    const confirmed = window.confirm(
      `¿Eliminar la actividad "${activity.nombre}"?\n\nEsta acción no se puede deshacer.`
    )
    if (!confirmed) return

    try {
      setDeletingId(activity.id)
      setActionError(null)

      await deleteActivityAsync(activity.id)

      setActivities((currentActivities) =>
        currentActivities.filter((item) => item.id !== activity.id)
      )
      // Si justo estábamos editando esa actividad, cerramos el formulario
      setEditingActivity((current) => (current?.id === activity.id ? null : current))
    } catch (error) {
      console.error('Falló el DELETE:', error)
      setActionError(extractBackendErrorMessage(error))
    } finally {
      setDeletingId(null)
    }
  }

  const filteredActivities = activities.filter((activity) =>
    (activity.nombre ?? '')
      .toLowerCase()
      .includes(search.toLowerCase())
  )

  // --- ZONA 4: RENDERIZADO EXCLUSIVO DEL CATÁLOGO ---
  return (
    <div>
      {/* Barra Superior de Título y Acción Principal */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4 pb-6 mb-8 border-b border-line">
        <div>
          <h1 className="text-2xl md:text-3xl font-bold text-ink">Actividades</h1>
          <p className="text-ink-muted text-sm mt-1">
            Clases, cargos e instructorías registradas en{' '}
            <code className="bg-brand-100 text-brand-700 font-mono text-xs px-1.5 py-0.5 rounded">
              Activity
            </code>
            .
          </p>
        </div>

        {/* Abre el formulario que dispara el POST /api/Activity */}
        {!isFormOpen && !editingActivity && (
          <button
            onClick={() => setIsFormOpen(true)}
            className="self-start sm:self-auto inline-flex items-center gap-2 bg-brand-600 hover:bg-brand-700 text-white font-medium text-sm px-4 py-2.5 rounded-lg shadow-sm transition-colors cursor-pointer focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-600 focus-visible:ring-offset-2 focus-visible:ring-offset-canvas"
          >
            + Actividad
          </button>
        )}
      </div>

      {/* Formulario de creación: al guardar, inserta la nueva fila sin recargar la página */}
      {isFormOpen && (
        <CreateActivityForm
          onActivityCreated={handleActivityCreated}
          onCancel={() => setIsFormOpen(false)}
        />
      )}

      {/* Formulario de edición: key={id} obliga a React a reiniciar el input si cambia la actividad */}
      {editingActivity && (
        <EditActivityForm
          key={editingActivity.id}
          activity={editingActivity}
          onActivityUpdated={handleActivityUpdated}
          onCancel={() => setEditingActivity(null)}
        />
      )}

      {/* Error de operaciones sin formulario (por ejemplo, un DELETE con tareas asociadas) */}
      {actionError && (
        <div className="mb-6 bg-red-50 border border-red-300 rounded-lg p-3 text-xs text-red-700 flex items-center gap-2">
          <span className="text-base">🛡️</span>
          <span><strong>No se pudo completar la operación:</strong> {actionError}</span>
        </div>
      )}

      {/* Buscador Reactivo */}
      <div className="mb-6 flex flex-col sm:flex-row sm:items-center justify-between gap-3">
        <input
          type="text"
          placeholder="Filtrar actividad..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          className="w-full sm:max-w-md bg-surface border border-line rounded-lg px-4 py-2.5 text-sm text-ink placeholder-ink-subtle focus:outline-none focus:border-brand-600 focus:ring-2 focus:ring-brand-600/20 transition"
        />
        <span className="text-xs text-ink-muted">
          Mostrando <strong className="text-brand-600">{filteredActivities.length}</strong> de{' '}
          {activities.length} actividades
        </span>
      </div>

    {/* 1. Estado de Carga */}
      {loading && (
        <div aria-busy="true" className="bg-surface border border-line rounded-2xl p-12 text-center">
          <p className="text-brand-600 font-semibold animate-pulse text-lg">
            ⏳ Consultando endpoint GET /api/Activity en .NET...
          </p>
        </div>
      )}

      {/* 2. Estado de Error de Red o Bloqueo CORS */}
      {!loading && networkError && (
        <div className="bg-red-50 border border-red-300 rounded-2xl p-6 text-center">
          <p className="text-red-700 font-bold text-base mb-1">🚨 Error de Comunicación Full-Stack</p>
          <p className="text-ink-muted text-sm">{networkError}</p>
        </div>
      )}

      {/* 3. Estado de Éxito: Renderizado de las filas reales de PostgreSQL */}
      {!loading && !networkError && filteredActivities.length === 0 && (
        <div className="rounded-2xl border border-dashed border-line bg-surface p-12 text-center">
          <p className="font-semibold text-ink">Sin resultados</p>
          <p className="text-sm text-ink-muted mt-1">
            {search
              ? `No hay actividades que coincidan con “${search}”.`
              : 'Aún no hay actividades registradas.'}
          </p>
        </div>
      )}

      {!loading && !networkError && filteredActivities.length > 0 && (
        <section className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-5">
          {filteredActivities.map((activity) => (
            <ActivityCard
              key={activity.id}
              id={activity.id}
              nombre={activity.nombre}
              isDeleting={deletingId === activity.id}
              onEdit={handleEdit}
              onDelete={handleDelete}
            />
          ))}
        </section>
      )}
    </div>
  )
}
