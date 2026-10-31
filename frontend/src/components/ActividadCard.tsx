// src/components/HabitoGlobalCard.tsx

// --- ZONA 1: EL CONTRATO (INTERFAZ) ---
// Actúa como un espejo exacto de nuestra tabla PostgreSQL / DTO en C#.
// Nuestra tabla actual solo tiene Id y Nombre.
export interface ActividadProps {
    id: number;
    nombre: string;
}

// --- ZONA 2: LA FIRMA (EXPORTACIÓN NOMBRADA) ---
// Desestructuramos las props cumpliendo el contrato de HabitoGlobalProps
export function ActividadCard({ id, nombre }: ActividadProps) {
    
    // --- ZONA 3: EL CEREBRO (LÓGICA) ---
    // (Vacío por los momentos, este componente es puramente presentacional)
    
    // --- ZONA 4: EL RENDERIZADO (JSX) ---
    return (
        <article className="bg-slate-800 border border-slate-700 rounded-xl p-5 flex flex-col justify-between hover:border-indigo-500 transition-colors shadow-sm">
            <div>
                {/* Cabecera de la Tarjeta: Badge con ID de Base de Datos */}
                <div className="flex items-center justify-between mb-3">
                    <span className="bg-indigo-500/10 text-indigo-400 text-xs font-semibold px-2.5 py-1 rounded-md border border-indigo-500/20">
                        ID Catálogo #{id}
                    </span>
                    <span className="text-xs text-slate-400 font-medium">Clases</span>
                </div>
                
                {/* Nombre del Hábito */}
                <h3 className="text-lg font-bold text-white mb-5">
                    {nombre}
                </h3>
            </div>

            {/* Botón de Acción (Se habilitará cuando implementemos Usuarios en la Unidad 3) */}
            <button 
                disabled 
                className="w-full bg-slate-900/80 text-slate-400 text-sm font-medium py-2.5 px-4 rounded-lg cursor-not-allowed border border-slate-700"
            >
                🔒 Agregar Task (Requiere Sesión)
            </button>
        </article>
    );
}
