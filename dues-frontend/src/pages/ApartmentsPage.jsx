import { useEffect, useMemo, useState } from 'react'
import { ApartmentsApi } from '../api/client'
import { Ledger, EmptyState } from '../components/Ledger'
import Modal from '../components/Modal'
import { Th, SearchInput } from '../components/TableControls'
import { useSort } from '../hooks/useSort'

const empty = { number: '', monthlyFee: '' }

export default function ApartmentsPage() {
  const [items, setItems] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [editing, setEditing] = useState(null) // null = closed, {} = new, {...} = edit
  const [form, setForm] = useState(empty)
  const [saving, setSaving] = useState(false)
  const [query, setQuery] = useState('')

  const load = () => {
    setLoading(true)
    ApartmentsApi.list()
      .then(setItems)
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false))
  }

  useEffect(load, [])

  const openNew = () => {
    setForm(empty)
    setEditing({})
  }

  const openEdit = (apt) => {
    setForm({ number: apt.number ?? '', monthlyFee: apt.monthlyFee ?? '' })
    setEditing(apt)
  }

  const save = async (e) => {
    e.preventDefault()
    setSaving(true)
    const payload = { number: form.number, monthlyFee: Number(form.monthlyFee) }
    try {
      if (editing?.id) {
        await ApartmentsApi.update(editing.id, payload)
      } else {
        await ApartmentsApi.create(payload)
      }
      setEditing(null)
      load()
    } catch (e) {
      setError(e.message)
    } finally {
      setSaving(false)
    }
  }

  const remove = async (apt) => {
    if (!confirm(`¿Eliminar el apartamento ${apt.number}?`)) return
    try {
      await ApartmentsApi.remove(apt.id)
      load()
    } catch (e) {
      setError(e.message)
    }
  }

  const filtered = useMemo(
    () => items.filter((a) => String(a.number).toLowerCase().includes(query.toLowerCase())),
    [items, query]
  )
  const { sorted, sortKey, sortDir, toggle } = useSort(filtered, 'number')

  return (
    <>
      <div className="page-header">
        <div>
          <h1>Apartamentos</h1>
          <p>Unidades del edificio y su cuota mensual base.</p>
        </div>
        <button className="btn btn-accent" onClick={openNew}>
          + Nuevo apartamento
        </button>
      </div>

      <div className="toolbar">
        <SearchInput value={query} onChange={setQuery} placeholder="Buscar por número…" />
      </div>

      {error && <div className="error-banner">{error}</div>}
      {loading ? (
        <div className="loading">Cargando…</div>
      ) : items.length === 0 ? (
        <div className="ledger">
          <EmptyState label="Todavía no hay apartamentos cargados." />
        </div>
      ) : sorted.length === 0 ? (
        <div className="ledger">
          <EmptyState label="Ningún apartamento coincide con la búsqueda." />
        </div>
      ) : (
        <>
          <div className="results-count">
            {sorted.length} de {items.length} apartamentos
          </div>
          <Ledger>
          <thead>
            <tr>
              <Th label="Número" sortKey="number" active={sortKey === 'number'} dir={sortDir} onClick={toggle} />
              <Th
                label="Cuota mensual"
                sortKey="monthlyFee"
                active={sortKey === 'monthlyFee'}
                dir={sortDir}
                onClick={toggle}
              />
              <th></th>
            </tr>
          </thead>
          <tbody>
            {sorted.map((apt) => (
              <tr key={apt.id}>
                <td>{apt.number}</td>
                <td className="num">${Number(apt.monthlyFee ?? 0).toFixed(2)}</td>
                <td>
                  <div className="row-actions">
                    <button className="icon-btn" onClick={() => openEdit(apt)}>
                      Editar
                    </button>
                    <button className="btn-danger-text" onClick={() => remove(apt)}>
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

      {editing !== null && (
        <Modal title={editing?.id ? 'Editar apartamento' : 'Nuevo apartamento'} onClose={() => setEditing(null)}>
          <form onSubmit={save}>
            <div className="field">
              <label>Número</label>
              <input
                required
                value={form.number}
                onChange={(e) => setForm({ ...form, number: e.target.value })}
              />
            </div>
            <div className="field">
              <label>Cuota mensual</label>
              <input
                required
                type="number"
                step="0.01"
                value={form.monthlyFee}
                onChange={(e) => setForm({ ...form, monthlyFee: e.target.value })}
              />
            </div>
            <div className="modal-actions">
              <button type="button" className="btn btn-ghost" onClick={() => setEditing(null)}>
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
