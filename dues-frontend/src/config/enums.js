// ⚠️ VERIFICAR: Swagger solo mostró los valores numéricos de estos enums
// (DueStatus: [0,1,2,3], PaymentMethod: array de 4 sin expandir).
// Las etiquetas de abajo son una suposición razonable según el dominio.
// Si no coinciden con el enum real de C#, ajustá solo este archivo.

export const DueStatus = {
  0: { label: 'Pendiente', tone: 'pending' },
  1: { label: 'Pagada', tone: 'paid' },
  2: { label: 'Vencida', tone: 'overdue' },
  3: { label: 'Parcial', tone: 'partial' },
}

export const PaymentMethod = {
  0: { label: 'Efectivo' },
  1: { label: 'Transferencia' },
  2: { label: 'Tarjeta' },
  3: { label: 'Cheque' },
}

export const dueStatusOptions = Object.entries(DueStatus).map(([value, v]) => ({
  value: Number(value),
  label: v.label,
}))

export const paymentMethodOptions = Object.entries(PaymentMethod).map(([value, v]) => ({
  value: Number(value),
  label: v.label,
}))
