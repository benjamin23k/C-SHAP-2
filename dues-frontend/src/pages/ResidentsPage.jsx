import { useEffect, useMemo, useState } from 'react'
import { ResidentsApi, ApartmentsApi } from '../api/client'
import { Ledger, EmptyState } from '../components/Ledger'
import Modal from '../components/Modal'
import { Th, SearchInput } from '../components/TableControls'
import { useSort } from '../hooks/useSort'

const empty = { name: '', phone: '', email: '', apartmentId: '' }

export default function ResidentsPage() {
  const [items, setItems] = useState([])
  const [apartments, setApartments] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [editing, setEditing] = useState(null)
  const [form, setForm] = useState(empty)
  const [saving, setSaving] = useState(false)
  const [query, setQuery] = useState('')
  const [apartmentFilter, setApartmentFilter] = useState('')

  const load = () => {
    setLoading(true)
    Promise.all([ResidentsApi.list(), ApartmentsApi.list()])
      .then(([residents, apts]) => {
        setItems(residents)
        setApartments(apts)
      })
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false))
  }

  useEffect(load, [])

  const openNew = () => {
    setForm(empty)
    setEditing({})
  }

  const openEdit = (r) => {
    setForm({
      name: r.name ?? '',
      phone: r.phone ?? '',
      email: r.email ?? '',
      apartmentId: r.apartmentId ?? '',
    })
    setEditing(r)
  }

  const apartmentLabel = (id) => {
    const a = apartments.find((x) => x.id === id)
    return a ? a.number : id
  }

  const save = async (e) => {
    e.preventDefault()
    setSaving(true)
    const payload = {
      name: form.name,
      phone: form.phone || null,
      email: form.email || null,
      apartmentId: Number(form.apartmentId),
    }
    try {
      if (editing?.id) {
        await ResidentsApi.update(editing.id, payload)
      } else {
        await ResidentsApi.create(payload)
      }
      setEditing(null)
      load()
    } catch (e) {
      setError(e.message)
    } finally {
      setSaving(false)
    }
  }

  const remove = async (r) => {
    if (!confirm(`¿Eliminar a ${r.name}?`)) return
    try {
      await ResidentsApi.remove(r.id)
      load()
    } catch (e) {
      setError(e.message)
    }
  }

  const filtered = useMemo(
    () =>
      items.filter((r) => {
        const matchesQuery = r.name?.toLowerCase().includes(query.toLowerCase())
        const matchesApt = !apartmentFilter || String(r.apartmentId) === apartmentFilter
        return matchesQuery && matchesApt
      }),
    [items, query, apartmentFilter]
  )
  const { sorted, sortKey, sortDir, toggle } = useSort(filtered, 'name')

  return (
    <>
      <div className="page-header">
        <div>
          <h1>Residentes</h1>
          <p>Personas asociadas a cada apartamento.</p>
        </div>
        <button className="btn btn-accent" onClick={openNew}>
          + Nuevo residente
        </button>
      </div>

      <div className="toolbar">
        <SearchInput value={query} onChange={setQuery} placeholder="Buscar por nombre…" />
        <select
          className="filter-select"
          value={apartmentFilter}
          onChange={(e) => setApartmentFilter(e.target.value)}
        >
          <option value="">Todos los apartamentos</option>
          {apartments.map((a) => (
            <option key={a.id} value={a.id}>
              {a.number}
            </option>
          ))}
        </select>
      </div>

      {error && <div className="error-banner">{error}</div>}
      {loading ? (
        <div className="loading">Cargando…</div>
      ) : items.length === 0 ? (
        <div className="ledger">
          <EmptyState label="Todavía no hay residentes cargados." />
        </div>
      ) : sorted.length === 0 ? (
        <div className="ledger">
          <EmptyState label="Ningún residente coincide con el filtro." />
        </div>
      ) : (
        <>
        <div className="results-count">
          {sorted.length} de {items.length} residentes
        </div>
        <Ledger>
          <thead>
            <tr>
              <Th label="Nombre" sortKey="name" active={sortKey === 'name'} dir={sortDir} onClick={toggle} />
              <th>Apartamento</th>
              <th>Teléfono</th>
              <th>Email</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {sorted.map((r) => (
              <tr key={r.id}>
                <td>{r.name}</td>
                <td>{apartmentLabel(r.apartmentId)}</td>
                <td className="num">{r.phone || '—'}</td>
                <td>{r.email || '—'}</td>
                <td>
                  <div className="row-actions">
                    <button className="icon-btn" onClick={() => openEdit(r)}>
                      Editar
                    </button>
                    <button className="btn-danger-text" onClick={() => remove(r)}>
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
        <Modal title={editing?.id ? 'Editar residente' : 'Nuevo residente'} onClose={() => setEditing(null)}>
          <form onSubmit={save}>
            <div className="field">
              <label>Nombre</label>
              <input required value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} />
            </div>
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
              <label>Teléfono</label>
              <input value={form.phone} onChange={(e) => setForm({ ...form, phone: e.target.value })} />
            </div>
            <div className="field">
              <label>Email</label>
              <input
                type="email"
                value={form.email}
                onChange={(e) => setForm({ ...form, email: e.target.value })}
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
