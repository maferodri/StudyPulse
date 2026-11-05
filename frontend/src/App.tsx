// src/App.tsx
import { AppLayout } from './layouts/AppLayout'
import { ActivityCatalogView } from './features/activities/components/ActivityCatalogView'

function App() {
  return (
    <AppLayout>
      <ActivityCatalogView />
    </AppLayout>
  )
}

export default App
