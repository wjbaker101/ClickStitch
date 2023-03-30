const { defineConfig } = require('@vue/cli-service');

module.exports = defineConfig({

    outputDir: '../backend/CrossStitchViewer/wwwroot',

    devServer: {
        proxy: {
            '/api': {
                target: 'https://localhost:44371',
                ws: true,
                changeOrigin: true,
            },
        },
    },

});