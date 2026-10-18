/// <reference types="vitest/config" />
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { fileURLToPath, URL } from 'node:url';

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('src', import.meta.url)),
    },
  },
  server: {
    port: parseInt(process.env.PORT ?? '3000'),
    proxy: {
      '/api/auth': {
        target: process.env.services__apiservice__apiservice__0 ?? 'http://localhost:5000',
        changeOrigin: true,
      },
      '/api': {
        target: `${process.env.services__apiservice__apiservice__0 ?? 'http://localhost:5000'}/v1`,
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/api/, ''),
      },
    },
  },
  test: {
    globals: true,
    environment: 'jsdom',
    globalSetup: ['./setup.vitest.ts'],
    coverage: {
      provider: 'v8',
      include: ['src/**/*.{ts,tsx}'],
      reportsDirectory: './coverage',
    },
    alias: {
      axios: 'axios/dist/node/axios.cjs',
    },
  },
});
