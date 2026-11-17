import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// W trybie dev /api idzie do bramki YARP (dotnet run w src/Gateway albo docker compose).
export default defineConfig({
  plugins: [vue()],
  server: {
    port: 5173,
    proxy: {
      '/api': 'http://localhost:8080',
    },
  },
})
