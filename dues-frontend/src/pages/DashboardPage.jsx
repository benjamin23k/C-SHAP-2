import { useEffect, useState } from 'react'
import { BarChart, Bar, XAxis, YAxis, Tooltip, ResponsiveContainer, CartesianGrid } from 'recharts'
import { ApartmentsApi, ResidentsApi, DuesApi, PaymentsApi } from '../api/client'
import { DueStatus, PaymentMethod } from '../config/enums'
import { Link } from 'react-router-dom'

export default function DashboardPage() {
  const [data, setData] = useState(null)
  const [error, setError] = useState(null)

  useEffect(() => {
    Promise.all([
      ApartmentsApi.list(),
      ResidentsApi.list(),
      DuesApi.list(),
      PaymentsApi.list(),
      DuesApi.debtReport(),
    ])
      .then(([apartments, residents, dues, payments, debts]) => {
        setData({ apartments, residents, dues, payments, debts })
      })
      .catch((e) => setError(e.message))
  }, [])

  if (error) {
    return (
      <>
        <div className="page-header">
          <div>
            <h1>Resumen</h1>
          </div>
        </div>
        <div className="error-banner">{error}</div>
      </>
    )
  }

  if (!data) {
    return (
      <>
        <div className="page-header">
          <div>
            <h1>Resumen</h1>
          </div>
        </div>
        <div className="loading">Cargando…</div>
      </>
    )
  }

  const { apartments, residents, dues, payments, debts } = data

  const pending = dues.filter((d) => d.status === 0).length
  const overdue = dues.filter((d) => d.status === 2).length
  const totalDebt = debts.reduce((s, d) => s + Number(d.totalDebt ?? 0), 0)
  const collectedThisMonth = payments
    .filter((p) => {
      const d = p.date ? new Date(p.date) : null
      const now = new Date()
      return d && d.getMonth() === now.getMonth() && d.getFullYear() === now.getFullYear()
    })
    .reduce((s, p) => s + Number(p.amount ?? 0), 0)

  const chartData = debts
    .slice()
    .sort((a, b) => Number(b.totalDebt) - Number(a.totalDebt))
    .slice(0, 8)
    .map((d) => ({ name: d.apartmentNumber, deuda: Number(d.totalDebt ?? 0) }))

  const apartmentLabel = (id) => apartments.find((a) => a.id === id)?.number ?? id
  const apartmentForPayment = (p) => {
    const due = p.due ?? dues.find((d) => d.id === p.dueId)
    return due ? apartmentLabel(due.apartmentId) : `Cuota #${p.dueId}`
  }
  const recentPayments = payments
    .slice()
    .sort((a, b) => new Date(b.date) - new Date(a.date))
    .slice(0, 6)

  return (
    <>
      <div className="hero">
        <div className="hero-eyebrow">Edificio · vista general</div>
        <h1>Resumen del mes</h1>
        <p>Estado de apartamentos, cuotas y cobros al día de hoy.</p>
      </div>

      <div className="kpi-grid">
        <div className="kpi-card" style={{ '--kpi-accent': 'var(--accent)' }}>
          <div className="kpi-label">Apartamentos</div>
          <div className="kpi-value">{apartments.length}</div>
          <div className="kpi-note">{residents.length} residentes registrados</div>
        </div>
        <div className="kpi-card" style={{ '--kpi-accent': 'var(--warning)' }}>
          <div className="kpi-label">Cuotas pendientes</div>
          <div className="kpi-value">{pending}</div>
          <div className="kpi-note">de {dues.length} cuotas totales</div>
        </div>
        <div className="kpi-card" style={{ '--kpi-accent': 'var(--danger)' }}>
          <div className="kpi-label">Cuotas vencidas</div>
          <div className="kpi-value">{overdue}</div>
          <div className="kpi-note">
            <Link to="/debts">ver detalle de deuda →</Link>
          </div>
        </div>
        <div className="kpi-card" style={{ '--kpi-accent': 'var(--success)' }}>
          <div className="kpi-label">Cobrado este mes</div>
          <div className="kpi-value">${collectedThisMonth.toFixed(2)}</div>
          <div className="kpi-note">deuda total: ${totalDebt.toFixed(2)}</div>
        </div>
      </div>

      <div className="dash-grid">
        <div className="panel">
          <h3>Deuda por apartamento</h3>
          <div className="panel-sub">Top 8 unidades con mayor deuda acumulada</div>
          {chartData.length === 0 ? (
            <div className="empty-state" style={{ padding: '24px 0' }}>
              Sin deuda registrada por el momento.
            </div>
          ) : (
            <ResponsiveContainer width="100%" height={260}>
              <BarChart data={chartData} margin={{ top: 4, right: 8, left: -12, bottom: 0 }}>
                <CartesianGrid stroke="#eef0f2" vertical={false} />
                <XAxis
                  dataKey="name"
                  tick={{ fontSize: 12, fill: '#5b6b7c' }}
                  axisLine={{ stroke: '#d8dee4' }}
                  tickLine={false}
                />
                <YAxis tick={{ fontSize: 11, fill: '#8b98a5' }} axisLine={false} tickLine={false} />
                <Tooltip
                  formatter={(v) => [`$${Number(v).toFixed(2)}`, 'Deuda']}
                  contentStyle={{ fontSize: 12, borderRadius: 6, border: '1px solid #d8dee4' }}
                />
                <Bar dataKey="deuda" fill="#a6743d" radius={[3, 3, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          )}
        </div>

        <div className="panel">
          <h3>Pagos recientes</h3>
          <div className="panel-sub">Últimos {recentPayments.length} pagos registrados</div>
          {recentPayments.length === 0 ? (
            <div className="empty-state" style={{ padding: '24px 0' }}>
              Todavía no hay pagos.
            </div>
          ) : (
            <ul className="recent-list">
              {recentPayments.map((p) => (
                <li key={p.id}>
                  <span className="who">{apartmentForPayment(p)}</span>
                  <span>
                    <span className="num">${Number(p.amount ?? 0).toFixed(2)}</span>{' '}
                    <span className="meta">
                      · {PaymentMethod[p.method]?.label ?? p.method} · {p.date?.slice(0, 10)}
                    </span>
                  </span>
                </li>
              ))}
            </ul>
          )}
        </div>
      </div>
    </>
  )
}
