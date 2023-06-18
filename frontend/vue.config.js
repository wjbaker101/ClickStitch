const { defineConfig } = require('@vue/cli-service');

module.exports = defineConfig({

    outputDir: '../backend/ClickStitch/wwwroot',

    devServer: {
        allowedHosts: 'all',
        proxy: {
            '/api': {
                target: 'https://localhost:44371',
                ws: true,
                changeOrigin: true,
            },
        },
    },

});