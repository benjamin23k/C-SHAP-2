import { Routes, Route } from 'react-router-dom'
import Layout from './components/Layout'
import DashboardPage from './pages/DashboardPage'
import ApartmentsPage from './pages/ApartmentsPage'
import ResidentsPage from './pages/ResidentsPage'
import DuesPage from './pages/DuesPage'
import PaymentsPage from './pages/PaymentsPage'
import DebtReportPage from './pages/DebtReportPage'

export default function App() {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route index element={<DashboardPage />} />
        <Route path="/apartments" element={<ApartmentsPage />} />
        <Route path="/residents" element={<ResidentsPage />} />
        <Route path="/dues" element={<DuesPage />} />
        <Route path="/payments" element={<PaymentsPage />} />
        <Route path="/debts" element={<DebtReportPage />} />
      </Route>
    </Routes>
  )
}
