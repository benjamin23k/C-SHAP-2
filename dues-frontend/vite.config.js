import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// Frontend runs on the same origin as the API in production.
// In dev, Vite proxies /api requests to the local DuesApi backend.
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'http://localhost:5104',
        changeOrigin: true,
      },
    },
  },
})
