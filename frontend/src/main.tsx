import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'

createRoot(document.getElementById('root')!).render(
  //Ejecuta los efectos dos veces en desarrollo
  <StrictMode>
    <App />
  </StrictMode>,
)
