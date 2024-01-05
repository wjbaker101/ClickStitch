import { type RouteRecordRaw } from 'vue-router';

import { requireAuth } from '../router-helper';
import { useAuth } from '@/use/auth/Auth.use';

const auth = useAuth();

export const creatorRoutes: Array<RouteRecordRaw> = [
    {
        path: '',
        component: () => import('@/views/_shared/login/Login.view.vue'),
        beforeEnter: (to, from, next) => {
            if (auth.details.value === null) {
                next('/login');
                return;
            }

            next('/dashboard');
        },
    },
    {
        path: '/login',
        component: () => import('@/views/_shared/login/Login.view.vue'),
        meta: {
            title: 'Login',
        },
    },
    {
        path: '/dashboard',
        component: () => import('@/views/creator/dashboard/CreatorDashboard.view.vue'),
        meta: {
            title: 'Creator Dashboard',
        },
    },
    {
        path: '/patterns',
        component: () => import('@/views/creator/patterns/CreatorPatterns.view.vue'),
        meta: {
            title: 'Creator Patterns',
        },
    },
    {
        path: '/patterns/new',
        component: () => import('@/views/creator/new-pattern/NewPattern.view.vue'),
        meta: {
            title: 'New Pattern',
        },
    },
    {
        path: '/settings',
        component: () => import('@/views/_shared/settings/Settings.view.vue'),
        beforeEnter: [ requireAuth ],
        meta: {
            title: 'Settings',
        },
    },
    {
        path: '/supported-pattern-formats',
        component: () => import('@/views/_shared/supported-pattern-formats/SupportedPatternFormats.view.vue'),
        meta: {
            title: 'Supported Pattern Formats',
        },
    },
    {
        path: '/:pathMatch(.*)*',
        component: () => import('@/views/_shared/not-found/NotFound.view.vue'),
        meta: {
            title: 'Page Not Found',
        },
    },
];