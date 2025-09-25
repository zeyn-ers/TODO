/** Angular (Vite) dev proxy — HTTPS + /api -> /api/v1 rewrite */
module.exports = {
  '/api': {
    target: 'https://localhost:57683', // 👈 Swagger'ın HTTPS adresi
    changeOrigin: true,
    secure: false,       // 👈 dev self-signed sertifika için şart
    ws: false,
    logLevel: 'debug',
    pathRewrite: (path) => path.replace(/^\/api/, '/api/v1'),
  },
};