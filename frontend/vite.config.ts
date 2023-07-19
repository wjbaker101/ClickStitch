import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import mkcert from 'vite-plugin-mkcert';
import { VitePWA as pwa } from 'vite-plugin-pwa'
import path from 'path';

export default defineConfig({

    plugins: [
        vue(),
        mkcert(),
        pwa({
            registerType: 'autoUpdate',
            includeAssets: ['favicon.ico', 'apple-touch-icon.png', 'mask-icon.svg'],
            manifest: {
                name: 'ClickStitch',
                short_name: 'ClickStitch',
                description: 'A companion for tracking your cross-stitching progress!',
                theme_color: '#22927f',
                icons: [
                    {
                        src: 'pwa-192x192.png',
                        sizes: '192x192',
                        type: 'image/png',
                    },
                    {
                        src: 'pwa-512x512.png',
                        sizes: '512x512',
                        type: 'image/png',
                    },
                ],
            },
            devOptions: {
                enabled: true,
            },
        }),
    ],

    resolve: {
        alias: [
            {
                find: /^~(.*)$/,
                replacement: '$1',
            },
            {
                find: '@',
                replacement: path.resolve(__dirname,'src'),
            }
        ],
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