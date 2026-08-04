import { useEffect, useState } from 'react'
import { ApartmentsApi } from '../api/client'

export function useApiHealth(intervalMs = 20000) {
  const [online, setOnline] = useState(null) // null = checking, true/false after first check

  useEffect(() => {
    let cancelled = false
    const ping = () => {
      ApartmentsApi.list()
        .then(() => !cancelled && setOnline(true))
        .catch(() => !cancelled && setOnline(false))
    }
    ping()
    const id = setInterval(ping, intervalMs)
    return () => {
      cancelled = true
      clearInterval(id)
    }
  }, [intervalMs])

  return online
}
