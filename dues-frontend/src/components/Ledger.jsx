export function Ledger({ children }) {
  return (
    <div className="ledger">
      <table className="ledger-table">{children}</table>
    </div>
  )
}

export function EmptyState({ label }) {
  return (
    <div className="empty-state">
      <div className="glyph">—</div>
      <div>{label}</div>
    </div>
  )
}
