// proxy.conf.js
module.exports = [
  {
    context: ['/api'],
    target: 'https://localhost:57683', // Swagger’daki HTTPS portu
    secure: false,
    changeOrigin: true,
    logLevel: 'debug',
  },
];
