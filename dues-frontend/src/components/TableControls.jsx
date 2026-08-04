export function Th({ label, sortKey, active, dir, onClick, align }) {
  return (
    <th
      onClick={sortKey ? () => onClick(sortKey) : undefined}
      className={sortKey ? 'sortable' : ''}
      style={align ? { textAlign: align } : undefined}
    >
      <span className="th-inner">
        {label}
        {sortKey && (
          <span className={`sort-arrow ${active ? 'active' : ''}`}>
            {active ? (dir === 'asc' ? '↑' : '↓') : '↕'}
          </span>
        )}
      </span>
    </th>
  )
}

export function SearchInput({ value, onChange, placeholder }) {
  return (
    <div className="search-input">
      <span className="search-icon">⌕</span>
      <input value={value} onChange={(e) => onChange(e.target.value)} placeholder={placeholder} />
    </div>
  )
}
