const { defineConfig } = require('@vue/cli-service');

module.exports = defineConfig({

    outputDir: '../backend/ClickStitch/wwwroot',

    devServer: {
        allowedHosts: [
            '.clickstitch.localdev',
        ],
        server: {
            type: 'https',
            options: {
                key: '_wildcard.clickstitch.localdev+1-key.pem',
                cert: '_wildcard.clickstitch.localdev+1.pem',
            },
        },
        proxy: {
            '/api': {
                target: 'https://localhost:44371',
                ws: true,
                changeOrigin: true,
            },
        },
    },

});