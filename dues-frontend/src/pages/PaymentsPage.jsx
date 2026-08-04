import { useEffect, useMemo, useState } from 'react'
import { PaymentsApi, DuesApi } from '../api/client'
import { Ledger, EmptyState } from '../components/Ledger'
import Modal from '../components/Modal'
import { paymentMethodOptions, PaymentMethod } from '../config/enums'
import { Th, SearchInput } from '../components/TableControls'
import { useSort } from '../hooks/useSort'

const empty = { dueId: '', amount: '', method: '' }

export default function PaymentsPage() {
  const [items, setItems] = useState([])
  const [dues, setDues] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [creating, setCreating] = useState(false)
  const [form, setForm] = useState(empty)
  const [saving, setSaving] = useState(false)
  const [receipt, setReceipt] = useState(null)
  const [query, setQuery] = useState('')
  const [methodFilter, setMethodFilter] = useState('')

  const load = () => {
    setLoading(true)
    Promise.all([PaymentsApi.list(), DuesApi.list()])
      .then(([payments, duesList]) => {
        setItems(payments)
        setDues(duesList)
      })
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false))
  }

  useEffect(load, [])

  const dueLabel = (id) => {
    const d = dues.find((x) => x.id === id)
    return d ? `Cuota #${d.id} · ${d.month}/${d.year}` : `Cuota #${id}`
  }

  const methodLabel = (m) => PaymentMethod[m]?.label ?? `#${m}`

  const filtered = useMemo(
    () =>
      items.filter((p) => {
        const matchesQuery = String(dueLabel(p.dueId)).toLowerCase().includes(query.toLowerCase())
        const matchesMethod = methodFilter === '' || String(p.method) === methodFilter
        return matchesQuery && matchesMethod
      }),
    [items, dues, query, methodFilter]
  )
  const { sorted, sortKey, sortDir, toggle } = useSort(filtered, 'date')

  const save = async (e) => {
    e.preventDefault()
    setSaving(true)
    const payload = {
      dueId: Number(form.dueId),
      amount: Number(form.amount),
      method: Number(form.method),
    }
    try {
      await PaymentsApi.create(payload)
      setCreating(false)
      setForm(empty)
      load()
    } catch (e) {
      setError(e.message)
    } finally {
      setSaving(false)
    }
  }

  const remove = async (p) => {
    if (!confirm(`¿Eliminar el pago #${p.id}?`)) return
    try {
      await PaymentsApi.remove(p.id)
      load()
    } catch (e) {
      setError(e.message)
    }
  }

  const viewReceipt = async (p) => {
    try {
      const data = await PaymentsApi.receipt(p.id)
      setReceipt(data)
    } catch (e) {
      setError(e.message)
    }
  }

  return (
    <>
      <div className="page-header">
        <div>
          <h1>Pagos</h1>
          <p>Pagos registrados contra cada cuota.</p>
        </div>
        <button className="btn btn-accent" onClick={() => setCreating(true)}>
          + Nuevo pago
        </button>
      </div>

      <div className="toolbar">
        <SearchInput value={query} onChange={setQuery} placeholder="Buscar por cuota…" />
        <select className="filter-select" value={methodFilter} onChange={(e) => setMethodFilter(e.target.value)}>
          <option value="">Todos los métodos</option>
          {paymentMethodOptions.map((o) => (
            <option key={o.value} value={o.value}>
              {o.label}
            </option>
          ))}
        </select>
      </div>

      {error && <div className="error-banner">{error}</div>}
      {loading ? (
        <div className="loading">Cargando…</div>
      ) : items.length === 0 ? (
        <div className="ledger">
          <EmptyState label="Todavía no hay pagos registrados." />
        </div>
      ) : sorted.length === 0 ? (
        <div className="ledger">
          <EmptyState label="Ningún pago coincide con el filtro." />
        </div>
      ) : (
        <>
        <div className="results-count">
          {sorted.length} de {items.length} pagos
        </div>
        <Ledger>
          <thead>
            <tr>
              <th>Cuota</th>
              <Th label="Monto" sortKey="amount" active={sortKey === 'amount'} dir={sortDir} onClick={toggle} />
              <th>Método</th>
              <Th label="Fecha" sortKey="date" active={sortKey === 'date'} dir={sortDir} onClick={toggle} />
              <th>Recibo</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {sorted.map((p) => (
              <tr key={p.id}>
                <td>{dueLabel(p.dueId)}</td>
                <td className="num">${Number(p.amount ?? 0).toFixed(2)}</td>
                <td>{methodLabel(p.method)}</td>
                <td className="num">{p.date?.slice(0, 10)}</td>
                <td className="num">{p.receiptNumber || '—'}</td>
                <td>
                  <div className="row-actions">
                    <button className="icon-btn" onClick={() => viewReceipt(p)}>
                      Ver recibo
                    </button>
                    <button className="btn-danger-text" onClick={() => remove(p)}>
                      Eliminar
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </Ledger>
        </>
      )}

      {creating && (
        <Modal title="Nuevo pago" onClose={() => setCreating(false)}>
          <form onSubmit={save}>
            <div className="field">
              <label>Cuota</label>
              <select required value={form.dueId} onChange={(e) => setForm({ ...form, dueId: e.target.value })}>
                <option value="" disabled>
                  Seleccionar…
                </option>
                {dues.map((d) => (
                  <option key={d.id} value={d.id}>
                    {dueLabel(d.id)}
                  </option>
                ))}
              </select>
            </div>
            <div className="field">
              <label>Monto</label>
              <input
                required
                type="number"
                step="0.01"
                value={form.amount}
                onChange={(e) => setForm({ ...form, amount: e.target.value })}
              />
            </div>
            <div className="field">
              <label>Método de pago</label>
              <select required value={form.method} onChange={(e) => setForm({ ...form, method: e.target.value })}>
                <option value="" disabled>
                  Seleccionar…
                </option>
                {paymentMethodOptions.map((o) => (
                  <option key={o.value} value={o.value}>
                    {o.label}
                  </option>
                ))}
              </select>
            </div>
            <div className="modal-actions">
              <button type="button" className="btn btn-ghost" onClick={() => setCreating(false)}>
                Cancelar
              </button>
              <button type="submit" className="btn btn-primary" disabled={saving}>
                {saving ? 'Guardando…' : 'Guardar'}
              </button>
            </div>
          </form>
        </Modal>
      )}

      {receipt && (
        <Modal title="Recibo" onClose={() => setReceipt(null)}>
          <pre style={{ fontFamily: 'var(--font-mono)', fontSize: 12.5, whiteSpace: 'pre-wrap' }}>
            {JSON.stringify(receipt, null, 2)}
          </pre>
          <div className="modal-actions">
            <button className="btn btn-primary" onClick={() => setReceipt(null)}>
              Cerrar
            </button>
          </div>
        </Modal>
      )}
    </>
  )
}
