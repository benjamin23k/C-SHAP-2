import { DueStatus } from '../config/enums'

export default function StatusBadge({ status }) {
  const entry = DueStatus[status] ?? { label: `#${status}`, tone: 'pending' }
  return <span className={`badge badge-${entry.tone}`}>{entry.label}</span>
}
