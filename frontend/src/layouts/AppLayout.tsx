import { useEffect, useState, type ReactNode } from 'react'
import { BookOpen, ListChecks, TrendingUp } from 'lucide-react'
import { getActivitiesAsync } from '../features/activities/api/activitiesApi'

interface AppLayoutProps {
  children: ReactNode // Representa la vista que se inyectará en el área de trabajo central
}

const menu = [
  { label: 'Actividades', icon: BookOpen, activo: true },
  { label: 'Tareas', icon: ListChecks, activo: false },
  { label: 'Progreso', icon: TrendingUp, activo: false },
]

export function AppLayout({ children }: AppLayoutProps) {
  // Contador del sidebar: comparte la misma capa de servicios que el catálogo
  const [totalActivities, setTotalActivities] = useState<number>(0)

  useEffect(() => {
    let isMounted = true
    getActivitiesAsync().then((data) => {
      if (isMounted) setTotalActivities(data.length)
    })
    return () => {
      isMounted = false
    }
  }, [])

  return (
    <div className="min-h-screen flex flex-col md:flex-row">
      {/* ===== SIDEBAR (escritorio) ===== */}
      <aside className="hidden md:flex md:w-64 md:flex-col md:sticky md:top-0 md:h-screen shrink-0 justify-between bg-sidebar p-5">
        <div>
          <div className="flex items-center gap-3 mb-10 px-2">
            <div className="w-9 h-9 rounded-xl bg-white flex items-center justify-center font-bold text-sidebar shadow-sm">
              SP
            </div>
            <span className="text-xl font-bold tracking-tight text-white">StudyPulse</span>
          </div>

          <nav className="space-y-1.5">
            {menu.map(({ label, icon: Icono, activo }) => (
              <a
                key={label}
                href="#"
                aria-current={activo ? 'page' : undefined}
                aria-disabled={!activo}
                className={
                  activo
                    ? 'flex items-center gap-3 px-3 py-2.5 rounded-lg bg-white/15 text-white text-sm font-medium'
                    : 'flex items-center gap-3 px-3 py-2.5 rounded-lg text-white/70 text-sm font-medium cursor-not-allowed'
                }
              >
                <Icono size={18} strokeWidth={2} />
                <span className="flex-1">{label}</span>

                {activo ? (
                  <span className="bg-white/20 text-white text-xs px-2 py-0.5 rounded-full">
                    {totalActivities}
                  </span>
                ) : (
                  <span className="text-[10px] uppercase tracking-wider">Pronto</span>
                )}
              </a>
            ))}
          </nav>
        </div>

        {/* Estado de sesión (se conecta con JWT en la Unidad 3) */}
        <div className="rounded-xl bg-white/10 border border-white/15 p-3.5">
          <p className="text-xs text-white/70 mb-1">Sesión</p>
          <p className="text-sm font-semibold text-white">Modo invitado</p>
        </div>
      </aside>

      {/* ===== HEADER MÓVIL ===== */}
      <header className="md:hidden bg-sidebar px-4 py-3 flex items-center justify-between">
        <div className="flex items-center gap-2.5">
          <div className="w-8 h-8 rounded-lg bg-white flex items-center justify-center font-bold text-sm text-sidebar">
            SP
          </div>
          <span className="font-bold text-lg text-white">StudyPulse</span>
        </div>
        <span className="text-xs bg-white/15 text-white px-2.5 py-1 rounded-md font-medium">
          Invitado
        </span>
      </header>

      {/* ===== ÁREA CENTRAL DE CONTENIDO ===== */}
      <main className="flex-1 p-6 md:p-10">{children}</main>
    </div>
  )
}
