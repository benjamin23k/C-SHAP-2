import { useEffect, useMemo, useState } from 'react'
import { DuesApi, ApartmentsApi } from '../api/client'
import { Ledger, EmptyState } from '../components/Ledger'
import Modal from '../components/Modal'
import StatusBadge from '../components/StatusBadge'
import { Th, SearchInput } from '../components/TableControls'
import { useSort } from '../hooks/useSort'
import { dueStatusOptions } from '../config/enums'

const empty = { apartmentId: '', month: '', year: new Date().getFullYear(), amount: '', dueDate: '' }
const months = [
  'Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
  'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre',
]

export default function DuesPage() {
  const [items, setItems] = useState([])
  const [apartments, setApartments] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [creating, setCreating] = useState(false)
  const [form, setForm] = useState(empty)
  const [saving, setSaving] = useState(false)
  const [busyAction, setBusyAction] = useState(null)
  const [generating, setGenerating] = useState(false)
  const [genForm, setGenForm] = useState({ month: new Date().getMonth() + 1, year: new Date().getFullYear() })
  const [query, setQuery] = useState('')
  const [statusFilter, setStatusFilter] = useState('')

  const load = () => {
    setLoading(true)
    Promise.all([DuesApi.list(), ApartmentsApi.list()])
      .then(([dues, apts]) => {
        setItems(dues)
        setApartments(apts)
      })
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false))
  }

  useEffect(load, [])

  const apartmentLabel = (id) => apartments.find((a) => a.id === id)?.number ?? id

  const filtered = useMemo(
    () =>
      items.filter((d) => {
        const matchesQuery = String(apartmentLabel(d.apartmentId)).toLowerCase().includes(query.toLowerCase())
        const matchesStatus = statusFilter === '' || String(d.status) === statusFilter
        return matchesQuery && matchesStatus
      }),
    [items, apartments, query, statusFilter]
  )
  const { sorted, sortKey, sortDir, toggle } = useSort(filtered, 'dueDate')

  const save = async (e) => {
    e.preventDefault()
    setSaving(true)
    const payload = {
      apartmentId: Number(form.apartmentId),
      month: Number(form.month),
      year: Number(form.year),
      amount: Number(form.amount),
      dueDate: form.dueDate,
    }
    try {
      await DuesApi.create(payload)
      setCreating(false)
      setForm(empty)
      load()
    } catch (e) {
      setError(e.message)
    } finally {
      setSaving(false)
    }
  }

  const remove = async (due) => {
    if (!confirm(`¿Eliminar la cuota #${due.id}?`)) return
    try {
      await DuesApi.remove(due.id)
      load()
    } catch (e) {
      setError(e.message)
    }
  }

  const runAction = async (action, fn) => {
    setBusyAction(action)
    setError(null)
    try {
      await fn()
      load()
    } catch (e) {
      setError(e.message)
    } finally {
      setBusyAction(null)
    }
  }

  const runGenerate = async (e) => {
    e.preventDefault()
    setBusyAction('generate')
    setError(null)
    try {
      await DuesApi.generateMonthly(Number(genForm.month), Number(genForm.year))
      setGenerating(false)
      load()
    } catch (e) {
      setError(e.message)
    } finally {
      setBusyAction(null)
    }
  }

  return (
    <>
      <div className="page-header">
        <div>
          <h1>Cuotas</h1>
          <p>Cuotas generadas por apartamento, mes y año.</p>
        </div>
        <button className="btn btn-accent" onClick={() => setCreating(true)}>
          + Nueva cuota
        </button>
      </div>

      <div className="toolbar">
        <button
          className="btn btn-ghost"
          disabled={busyAction === 'generate'}
          onClick={() => setGenerating(true)}
        >
          {busyAction === 'generate' ? 'Generando…' : 'Generar cuotas del mes'}
        </button>
        <button
          className="btn btn-ghost"
          disabled={busyAction === 'overdue'}
          onClick={() => runAction('overdue', DuesApi.updateOverdue)}
        >
          {busyAction === 'overdue' ? 'Actualizando…' : 'Actualizar vencidas'}
        </button>
      </div>

      <div className="toolbar">
        <SearchInput value={query} onChange={setQuery} placeholder="Buscar por apartamento…" />
        <select className="filter-select" value={statusFilter} onChange={(e) => setStatusFilter(e.target.value)}>
          <option value="">Todos los estados</option>
          {dueStatusOptions.map((o) => (
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
          <EmptyState label="Todavía no hay cuotas generadas." />
        </div>
      ) : sorted.length === 0 ? (
        <div className="ledger">
          <EmptyState label="Ninguna cuota coincide con el filtro." />
        </div>
      ) : (
        <>
        <div className="results-count">
          {sorted.length} de {items.length} cuotas
        </div>
        <Ledger>
          <thead>
            <tr>
              <th>Apartamento</th>
              <th>Período</th>
              <Th label="Monto" sortKey="amount" active={sortKey === 'amount'} dir={sortDir} onClick={toggle} />
              <th>Pagado</th>
              <th>Saldo</th>
              <Th
                label="Vence"
                sortKey="dueDate"
                active={sortKey === 'dueDate'}
                dir={sortDir}
                onClick={toggle}
              />
              <th>Estado</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {sorted.map((d) => (
              <tr key={d.id}>
                <td>{apartmentLabel(d.apartmentId)}</td>
                <td>
                  {months[d.month - 1] ?? d.month} {d.year}
                </td>
                <td className="num">${Number(d.amount ?? 0).toFixed(2)}</td>
                <td className="num">${Number(d.amountPaid ?? 0).toFixed(2)}</td>
                <td className="num">${Number(d.balance ?? 0).toFixed(2)}</td>
                <td className="num">{d.dueDate?.slice(0, 10)}</td>
                <td>
                  <StatusBadge status={d.status} />
                </td>
                <td>
                  <div className="row-actions">
                    <button className="btn-danger-text" onClick={() => remove(d)}>
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

      {generating && (
        <Modal title="Generar cuotas del mes" onClose={() => setGenerating(false)}>
          <form onSubmit={runGenerate}>
            <div className="field">
              <label>Mes</label>
              <select
                required
                value={genForm.month}
                onChange={(e) => setGenForm({ ...genForm, month: e.target.value })}
              >
                {months.map((m, i) => (
                  <option key={m} value={i + 1}>
                    {m}
                  </option>
                ))}
              </select>
            </div>
            <div className="field">
              <label>Año</label>
              <input
                required
                type="number"
                value={genForm.year}
                onChange={(e) => setGenForm({ ...genForm, year: e.target.value })}
              />
            </div>
            <div className="modal-actions">
              <button type="button" className="btn btn-ghost" onClick={() => setGenerating(false)}>
                Cancelar
              </button>
              <button type="submit" className="btn btn-primary" disabled={busyAction === 'generate'}>
                {busyAction === 'generate' ? 'Generando…' : 'Generar'}
              </button>
            </div>
          </form>
        </Modal>
      )}

      {creating && (
        <Modal title="Nueva cuota" onClose={() => setCreating(false)}>
          <form onSubmit={save}>
            <div className="field">
              <label>Apartamento</label>
              <select
                required
                value={form.apartmentId}
                onChange={(e) => setForm({ ...form, apartmentId: e.target.value })}
              >
                <option value="" disabled>
                  Seleccionar…
                </option>
                {apartments.map((a) => (
                  <option key={a.id} value={a.id}>
                    {a.number}
                  </option>
                ))}
              </select>
            </div>
            <div className="field">
              <label>Mes</label>
              <select required value={form.month} onChange={(e) => setForm({ ...form, month: e.target.value })}>
                <option value="" disabled>
                  Seleccionar…
                </option>
                {months.map((m, i) => (
                  <option key={m} value={i + 1}>
                    {m}
                  </option>
                ))}
              </select>
            </div>
            <div className="field">
              <label>Año</label>
              <input
                required
                type="number"
                value={form.year}
                onChange={(e) => setForm({ ...form, year: e.target.value })}
              />
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
              <label>Fecha de vencimiento</label>
              <input
                required
                type="date"
                value={form.dueDate}
                onChange={(e) => setForm({ ...form, dueDate: e.target.value })}
              />
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
    </>
  )
}
