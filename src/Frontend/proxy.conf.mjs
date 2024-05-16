export default [
  {
    context: [
      '/user/**',
      '/Home/**',
      '/account/**',
      '/Account/**',
      '/bundle/css/*',
      '/bundle/js/*',
      '/api/**',
      '/hubs/**',
      '/hangfire/**',
      '/_vs/**',
      '/_framework/**'
    ],
    target: 'https://localhost:3001',
    secure: false,
    logLevel: 'debug',
    changeOrigin: true
  }
  // {
  //   context: [
  //     '/*.js',
  //     '/index.html',
  //     '/*.map',
  //     '/*.woff2',
  //     '/*.woff',
  //     '/*.ttf',
  //     '/assets/**'
  //   ],
  //   target: 'https://localhost:4200',
  //   secure: false,
  //   logLevel: 'debug',
  //   changeOrigin: false
  // }
]
