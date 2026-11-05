import { BookOpen, Loader2, Pencil, Trash2 } from 'lucide-react'
import type { Activity } from '../types/activity.types'

interface ActivityCardProps extends Activity {
  isDeleting: boolean
  onEdit: (activity: Activity) => void
  onDelete: (activity: Activity) => void
}

// --- ZONA 2: LA FIRMA ---
export function ActivityCard({ id, nombre, isDeleting, onEdit, onDelete }: ActivityCardProps) {
  // --- ZONA 3: LÓGICA ---
  // (Vacío: componente puramente presentacional; solo avisa hacia arriba)

  // --- ZONA 4: JSX ---
  return (
    <article className="h-full bg-surface border border-line rounded-2xl p-5 flex flex-col gap-3 shadow-sm hover:shadow-md hover:border-brand-400 transition">
    {/* Fila superior */}
    <div className="flex items-center justify-between gap-2">
        <div className="w-10 h-10 rounded-xl bg-brand-100 text-brand-700 flex items-center justify-center">
        <BookOpen size={20} />
        </div>

        <div className="flex items-center gap-1.5">

        <button
            type="button"
            onClick={() => onEdit({ id, nombre })}
            disabled={isDeleting}
            title="Editar actividad"
            aria-label={`Editar ${nombre}`}
            className="w-8 h-8 rounded-lg border border-line text-ink-muted hover:text-brand-600 hover:bg-brand-50 flex items-center justify-center transition-colors cursor-pointer disabled:opacity-40 disabled:cursor-not-allowed"
        >
            <Pencil size={15} />
        </button>

        <button
            type="button"
            onClick={() => onDelete({ id, nombre })}
            disabled={isDeleting}
            title="Eliminar actividad"
            aria-label={`Eliminar ${nombre}`}
            className="w-8 h-8 rounded-lg border border-line text-ink-muted hover:text-red-600 hover:bg-red-50 flex items-center justify-center transition-colors cursor-pointer disabled:opacity-40 disabled:cursor-not-allowed"
        >
            {isDeleting ? <Loader2 size={15} className="animate-spin" /> : <Trash2 size={15} />}
        </button>
        </div>
    </div>

    {/* Título sin altura reservada */}
    <h3 className="text-lg font-bold text-ink leading-snug line-clamp-2" title={nombre}>
        {nombre}
    </h3>

    <button
        disabled
        title="Disponible próximamente"
        className="mt-auto w-full rounded-lg border border-line bg-brand-50 py-2.5 px-4 text-sm font-medium text-ink-subtle cursor-not-allowed"
    >
        Agregar tarea
    </button>
    </article>
  )
}
