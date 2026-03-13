import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import tailwindcss from '@tailwindcss/vite'

export default defineConfig({
  plugins: [
    vue(),
    tailwindcss(),],
  // server: {
  //   proxy: {
  //     '/api': {
  //       // target: 'https://localhost:7295',
  //       changeOrigin: true,
  //       secure: false,           // ← ignores self-signed cert errors
  //       rewrite: (path) => path.replace(/^\/api/, '/api')
  //     }
  //   }
  // }
})