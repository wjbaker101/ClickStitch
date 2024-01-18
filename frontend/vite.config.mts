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
        emptyOutDir: true,

        rollupOptions: {
            output: {
                manualChunks: {
                    'chunk_shared': [
                        '@/views/_shared/login/Login.view.vue',
                        '@/views/_shared/settings/Settings.view.vue',
                        '@/views/_shared/not-found/NotFound.view.vue',
                        '@/views/_shared/supported-pattern-formats/SupportedPatternFormats.view.vue',
                    ],
                    'chunk_admin': [
                        '@/views/admin/admin/Admin.view.vue',
                        '@/views/admin/admin/Admin.view.vue',
                    ],
                    'chunk_creator': [
                        '@/views/creator/dashboard/CreatorDashboard.view.vue',
                        '@/views/creator/patterns/CreatorPatterns.view.vue',
                        '@/views/creator/new-pattern/NewPattern.view.vue',
                    ],
                    'chunk_stitcher': [
                        '@/views/stitcher/about/About.view.vue',
                        '@/views/stitcher/dashboard/Dashboard.view.vue',
                        '@/views/stitcher/new-pattern/NewPattern.view.vue',
                        '@/views/stitcher/patterns/Patterns.view.vue',
                        '@/views/stitcher/project/Project.view.vue',
                        '@/views/stitcher/tools/Tools.view.vue',
                        '@/views/stitcher/project-analytics/ProjectAnalytics.view.vue',
                        '@/views/stitcher/inventory/Inventory.view.vue',
                        '@/views/_shared/signup/Signup.view.vue',
                        '@/views/_shared/marketing/Marketing.view.vue',
                    ],
                },
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