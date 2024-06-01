import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import { VitePWA as pwa } from 'vite-plugin-pwa'
import path from 'path';

import { pwaOptions } from './vite/pwa';
import { chunksOptions } from './vite/chunks';

export default defineConfig({

    plugins: [
        vue(),
        pwa(pwaOptions),
    ],

    resolve: {
        alias: [
            {
                find: '@',
                replacement: path.resolve(__dirname, 'src'),
            }
        ],
    },

    build: {
        outDir: '../backend/ClickStitch/wwwroot',
        emptyOutDir: true,

        rollupOptions: {
            output: {
                manualChunks: chunksOptions,
            },
        },
    },

    server: {
        port: 8080,
        proxy: {
            '/api': {
                target: 'https://localhost:44371',
                secure: false,
                changeOrigin: true,
            },
        },
    },

});