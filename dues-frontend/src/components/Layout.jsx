import { NavLink, Outlet } from 'react-router-dom'
import { useApiHealth } from '../hooks/useApiHealth'

const sections = [
  { no: '00', to: '/', label: 'Resumen', end: true },
  { no: '01', to: '/apartments', label: 'Apartamentos' },
  { no: '02', to: '/residents', label: 'Residentes' },
  { no: '03', to: '/dues', label: 'Cuotas' },
  { no: '04', to: '/payments', label: 'Pagos' },
  { no: '05', to: '/debts', label: 'Deudas' },
]

export default function Layout() {
  const online = useApiHealth()

  return (
    <div className="app-shell">
      <aside className="sidebar">
        <div className="sidebar-brand">
          <div className="mark">Dues</div>
          <div className="sub">Panel de administración</div>
        </div>
        <ul className="directory">
          {sections.map((s) => (
            <li key={s.to}>
              <NavLink to={s.to} end={s.end} className={({ isActive }) => (isActive ? 'active' : '')}>
                <span>{s.label}</span>
                <span className="plaque-no">{s.no}</span>
              </NavLink>
            </li>
          ))}
        </ul>
        <div className="sidebar-foot">
          <div className="conn-indicator">
            <span
              className={`conn-dot ${online === null ? '' : online ? 'online' : 'offline'}`}
            />
            {online === null ? 'Verificando API…' : online ? 'API conectada' : 'Sin conexión con la API'}
          </div>
        </div>
      </aside>
      <main className="main">
        <Outlet />
      </main>
    </div>
  )
}
