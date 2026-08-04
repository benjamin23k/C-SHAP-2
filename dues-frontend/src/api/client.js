import axios from 'axios'

const http = axios.create({
  baseURL: '/api',
  headers: { 'Content-Type': 'application/json' },
})

http.interceptors.response.use(
  (res) => res,
  (err) => {
    const message =
      err.response?.data?.title ||
      err.response?.data?.message ||
      err.response?.data ||
      err.message ||
      'Error de red'
    return Promise.reject(new Error(typeof message === 'string' ? message : JSON.stringify(message)))
  }
)

// Apartments — CRUD estándar asumido (mismo patrón que Residents; no vino
// en el swagger que compartiste, verificar rutas si algo falla)
export const ApartmentsApi = {
  list: () => http.get('/Apartments').then((r) => r.data),
  get: (id) => http.get(`/Apartments/${id}`).then((r) => r.data),
  create: (data) => http.post('/Apartments', data).then((r) => r.data),
  update: (id, data) => http.put(`/Apartments/${id}`, data).then((r) => r.data),
  remove: (id) => http.delete(`/Apartments/${id}`),
}

// Dues
export const DuesApi = {
  list: () => http.get('/Dues').then((r) => r.data),
  get: (id) => http.get(`/Dues/${id}`).then((r) => r.data),
  create: (data) => http.post('/Dues', data).then((r) => r.data),
  remove: (id) => http.delete(`/Dues/${id}`),
  byApartment: (apartmentId) => http.get(`/Dues/apartment/${apartmentId}`).then((r) => r.data),
  generateMonthly: (month, year) => http.post(`/Dues/generate-monthly?month=${month}&year=${year}`).then((r) => r.data),
  debtReport: () => http.get('/Dues/reports/debts').then((r) => r.data),
  updateOverdue: () => http.post('/Dues/update-overdue').then((r) => r.data),
}

// Payments
export const PaymentsApi = {
  list: () => http.get('/Payments').then((r) => r.data),
  get: (id) => http.get(`/Payments/${id}`).then((r) => r.data),
  create: (data) => http.post('/Payments', data).then((r) => r.data),
  remove: (id) => http.delete(`/Payments/${id}`),
  byDue: (dueId) => http.get(`/Payments/due/${dueId}`).then((r) => r.data),
  receipt: (id) => http.get(`/Payments/${id}/receipt`).then((r) => r.data),
}

// Residents
export const ResidentsApi = {
  list: () => http.get('/Residents').then((r) => r.data),
  get: (id) => http.get(`/Residents/${id}`).then((r) => r.data),
  create: (data) => http.post('/Residents', data).then((r) => r.data),
  update: (id, data) => http.put(`/Residents/${id}`, data).then((r) => r.data),
  remove: (id) => http.delete(`/Residents/${id}`),
}
