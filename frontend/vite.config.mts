import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import { VitePWA as pwa } from 'vite-plugin-pwa'
import path from 'path';

import { pwaOptions } from './vite/pwa';

export default defineConfig({

    plugins: [
        vue(),
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
        exclude: [
            '@wjb/vue/use/modal.use',
            '@wjb/vue/use/popup.use',
        ],
    },

    build: {
        outDir: '../backend/ClickStitch/wwwroot',
    },

    server: {
        port: 8080,
        proxy: {
            '/api': {
                target: 'https://localhost:44371',
                secure: false,
            },
        },
    },

});