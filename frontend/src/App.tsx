import { useEffect, useState } from 'react'
import { BookOpen, ListChecks, TrendingUp, Plus } from 'lucide-react'
import { ActividadCard, type ActividadProps } from './components/ActividadCard'

const menu = [
  { label: 'Actividades', icon: BookOpen, activo: true },
  { label: 'Tareas', icon: ListChecks, activo: false },
  { label: 'Progreso', icon: TrendingUp, activo: false },
]

function App() {
  // 1. Casillero #0 en el Heap (Fiber)
  const [actividades, setActividades] = useState<ActividadProps[]>([])
  // 2. Casillero #1 en el Heap
  const [cargando, setCargando] = useState<boolean>(true)
  // 3. Casillero #2 en el Heap
  const [busqueda, setBusqueda] = useState<string>('')

  useEffect(() => {
    const temporizador = setTimeout(() => {
      const datosSimuladosDB: ActividadProps[] = [
        { id: 1, nombre: 'Sistemas Expertos' },
        { id: 2, nombre: 'Ingeniería de Software' },
        { id: 3, nombre: 'Finanzas Administrativas' },
        { id: 4, nombre: 'Diseño Digital' },
        { id: 5, nombre: 'Diseño de Compiladores' },
      ]
      setActividades(datosSimuladosDB)
      setCargando(false)
    }, 1200)

    return () => clearTimeout(temporizador)
  }, [])

  const actividadesFiltradas = actividades.filter((actividad) =>
    actividad.nombre.toLowerCase().includes(busqueda.toLowerCase())
  )

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
                    {actividades.length}
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

      {/* ===== CONTENIDO ===== */}
      <main className="flex-1 p-6 md:p-10">
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

          {/* Se conecta con el POST en el laboratorio de integración */}
          <button className="self-start sm:self-auto inline-flex items-center gap-2 bg-brand-600 hover:bg-brand-700 text-white font-medium text-sm px-4 py-2.5 rounded-lg shadow-sm transition-colors cursor-pointer focus:outline-none focus-visible:ring-2 focus-visible:ring-brand-600 focus-visible:ring-offset-2 focus-visible:ring-offset-canvas">
            <Plus size={16} /> Actividad
          </button>
        </div>

        {/* Buscador */}
        <div className="mb-6 flex flex-col sm:flex-row sm:items-center justify-between gap-3">
          <input
            type="text"
            placeholder="Filtrar actividad..."
            value={busqueda}
            onChange={(e) => setBusqueda(e.target.value)}
            className="w-full sm:max-w-md bg-surface border border-line rounded-lg px-4 py-2.5 text-sm text-ink placeholder-ink-subtle focus:outline-none focus:border-brand-600 focus:ring-2 focus:ring-brand-600/20 transition"
          />
          <span className="text-xs text-ink-muted">
            Mostrando <strong className="text-brand-600">{actividadesFiltradas.length}</strong> de{' '}
            {actividades.length} actividades
          </span>
        </div>

        {/* Renderizado condicional: cargando, vacío o lista */}
        {cargando ? (
          <section
            aria-busy="true"
            className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-5"
          >
            {Array.from({ length: 6 }).map((_, i) => (
              <div
                key={i}
                className="h-44 rounded-2xl bg-surface border border-line animate-pulse"
              />
            ))}
          </section>
        ) : actividadesFiltradas.length === 0 ? (
          <div className="rounded-2xl border border-dashed border-line bg-surface p-12 text-center">
            <p className="font-semibold text-ink">Sin resultados</p>
            <p className="text-sm text-ink-muted mt-1">
              {busqueda
                ? `No hay actividades que coincidan con “${busqueda}”.`
                : 'Aún no hay actividades registradas.'}
            </p>
          </div>
        ) : (
          <section className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-5">
            {actividadesFiltradas.map((actividad) => (
              <ActividadCard key={actividad.id} id={actividad.id} nombre={actividad.nombre} />
            ))}
          </section>
        )}
      </main>
    </div>
  )
}

export default App