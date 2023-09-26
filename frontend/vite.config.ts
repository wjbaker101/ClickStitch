import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import mkcert from 'vite-plugin-mkcert';
import { VitePWA as pwa } from 'vite-plugin-pwa'
import path from 'path';

import { pwaOptions } from './vite/pwa';

export default defineConfig({

    plugins: [
        vue(),
        mkcert(),
        pwa(pwaOptions),
    ],

    resolve: {
        alias: [
            {
                find: /^~(.*)$/,
                replacement: '$1',
            },
            {
                find: '@',
                replacement: path.resolve(__dirname, 'src'),
            }
        ],
    },

    optimizeDeps: {
        exclude: ['@wjb/vue/use/modal.use'],
    },

    build: {
        outDir: '../backend/ClickStitch/wwwroot',
    },

    server: {
        https: true,
        port: 8080,
        proxy: {
            '/api': {
                target: 'https://localhost:44371',
                changeOrigin: true,
                secure: false,
            },
        },
    },

});