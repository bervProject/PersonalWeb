const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function (app) {
    app.use(
        '/api',
        createProxyMiddleware({
            target: `${process.env.services__apiservice__apiservice__0}/v1`,
            changeOrigin: true,
        })
    );
};