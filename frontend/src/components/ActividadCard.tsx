import { BookOpen } from 'lucide-react'

// --- ZONA 1: EL CONTRATO (espejo del DTO en C#) ---
export interface ActividadProps {
  id: number
  nombre: string
}

// --- ZONA 2: LA FIRMA ---
export function ActividadCard({ id, nombre }: ActividadProps) {
  // --- ZONA 3: LÓGICA ---
  // (Vacío: componente puramente presentacional)

  // --- ZONA 4: JSX ---
  return (
    <article className="h-full bg-surface border border-line rounded-2xl p-5 flex flex-col gap-3 shadow-sm hover:shadow-md hover:border-brand-400 transition">
    {/* Fila superior */}
    <div className="flex items-center justify-between">
        <div className="w-10 h-10 rounded-xl bg-brand-100 text-brand-700 flex items-center justify-center">
        <BookOpen size={20} />
        </div>
        <span className="text-xs font-medium text-ink-subtle">#{id}</span>
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