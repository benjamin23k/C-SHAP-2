import { useEffect, useState } from 'react'
import { DuesApi } from '../api/client'
import { Ledger, EmptyState } from '../components/Ledger'

export default function DebtReportPage() {
  const [items, setItems] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)

  useEffect(() => {
    DuesApi.debtReport()
      .then(setItems)
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false))
  }, [])

  const totalDebt = items.reduce((sum, i) => sum + Number(i.totalDebt ?? 0), 0)
  const totalOverdue = items.reduce((sum, i) => sum + Number(i.overdueCount ?? 0), 0)

  return (
    <>
      <div className="page-header">
        <div>
          <h1>Deudas</h1>
          <p>Estado de deuda por apartamento.</p>
        </div>
      </div>

      {error && <div className="error-banner">{error}</div>}

      {!loading && !error && (
        <div className="stat-grid">
          <div className="stat-card">
            <div className="stat-label">Apartamentos con deuda</div>
            <div className="stat-value">{items.length}</div>
          </div>
          <div className="stat-card">
            <div className="stat-label">Deuda total</div>
            <div className="stat-value">${totalDebt.toFixed(2)}</div>
          </div>
          <div className="stat-card">
            <div className="stat-label">Cuotas vencidas</div>
            <div className="stat-value">{totalOverdue}</div>
          </div>
        </div>
      )}

      {loading ? (
        <div className="loading">Cargando…</div>
      ) : items.length === 0 ? (
        <div className="ledger">
          <EmptyState label="No hay apartamentos con deuda registrada." />
        </div>
      ) : (
        <Ledger>
          <thead>
            <tr>
              <th>Apartamento</th>
              <th>Deuda total</th>
              <th>Cuotas vencidas</th>
            </tr>
          </thead>
          <tbody>
            {items.map((r) => (
              <tr key={r.apartmentId}>
                <td>{r.apartmentNumber}</td>
                <td className="num">${Number(r.totalDebt ?? 0).toFixed(2)}</td>
                <td className="num">{r.overdueCount}</td>
              </tr>
            ))}
          </tbody>
        </Ledger>
      )}
    </>
  )
}
