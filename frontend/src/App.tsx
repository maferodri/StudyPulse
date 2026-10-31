// src/App.tsx
import { useEffect, useState } from 'react'
import './App.css'
import { ActividadCard } from './components/ActividadCard'

function App() {
  return (
    <div className="min-h-screen bg-slate-900 text-slate-100 flex flex-col md:flex-row">
      
      {/* =====================================================================
          1. COLUMNA IZQUIERDA: MENÚ CORPORATIVO (SIDEBAR)
          Fijo de 256px (w-64) en Escritorio | Oculto en Celular
         ===================================================================== */}
      <aside className="hidden md:flex md:w-64 md:flex-col bg-slate-950 border-r border-slate-800 justify-between p-5 shrink-0">
        <div>
          {/* Identidad de Marca */}
          <div className="flex items-center gap-3 mb-8 px-2">
            <div className="w-8 h-8 rounded-lg bg-indigo-600 flex items-center justify-center font-bold text-white shadow">
              SP
            </div>
            <span className="text-xl font-bold tracking-tight text-white">StudyPulse</span>
          </div>

          {/* Menú de Navegación del Sistema */}
          <nav className="space-y-2">
            <a href="#" className="flex items-center gap-3 px-3 py-2.5 bg-indigo-600/15 text-indigo-400 rounded-lg font-medium text-sm border border-indigo-500/20">
              <span>🌐</span> Actividades
            </a>
            <a href="#" className="flex items-center gap-3 px-3 py-2.5 text-slate-500 rounded-lg font-medium text-sm cursor-not-allowed">
              <span>📈</span> Task
            </a>
            <a href="#" className="flex items-center gap-3 px-3 py-2.5 text-slate-500 rounded-lg font-medium text-sm cursor-not-allowed">
              <span>📊</span> Progreso
            </a>
          </nav>
        </div>

        {/* Estado de Sesión (Se conectará con JWT en la Unidad 3) */}
        <div className="p-3.5 bg-slate-900 rounded-xl border border-slate-800">
          <p className="text-xs text-slate-400 mb-1">Estado de Seguridad</p>
          <p className="text-sm font-semibold text-amber-400">👤 Modo Invitado</p>
        </div>
      </aside>

      {/* =====================================================================
          2. CABECERA MÓVIL (Visible únicamente en teléfonos celulares: md:hidden)
         ===================================================================== */}
      <header className="md:hidden bg-slate-950 border-b border-slate-800 p-4 flex items-center justify-between">
        <div className="flex items-center gap-2.5">
          <div className="w-7 h-7 rounded-md bg-indigo-600 flex items-center justify-center font-bold text-sm text-white">
            HF
          </div>
          <span className="font-bold text-lg text-white">StudyPulse</span>
        </div>
        <span className="text-xs bg-amber-500/10 text-amber-400 px-2.5 py-1 rounded-md border border-amber-500/20 font-medium">
          👤 Invitado
        </span>
      </header>

      {/* =====================================================================
          3. ÁREA CENTRAL DE CONTENIDO (WORKSPACE)
         ===================================================================== */}
      <main className="flex-1 p-6 md:p-10 overflow-y-auto">
        
        {/* Barra Superior de Título y Acción Principal */}
        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4 pb-6 mb-8 border-b border-slate-800">
          <div>
            <h1 className="text-2xl md:text-3xl font-bold text-white">Actividades</h1>
            <p className="text-slate-400 text-sm mt-1">
              Explora las actividades registradas en la tabla <code className="text-indigo-400 font-mono">Activity</code>.
            </p>
          </div>

          {/* Botón preparado para el Laboratorio de Integración POST */}
          <button className="bg-indigo-600 hover:bg-indigo-500 text-white font-medium text-sm px-4 py-2.5 rounded-lg transition-colors shadow-sm self-start sm:self-auto cursor-pointer">
            + Actividad
          </button>
        </div>

        {/* REJILLA RESPONSIVA DE TARJETAS (1 col en Móvil | 2 en Tablet | 3 en Escritorio) */}
        <section className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-5">
          <ActividadCard id={1} nombre="Sistemas Expertos" />
          <ActividadCard id={2} nombre="Ingeniería De Software" />
          <ActividadCard id={3} nombre="Finanzas Administrativas" />
        </section>

      </main>
    </div>
  )
}

export default App