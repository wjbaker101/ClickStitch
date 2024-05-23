import { type RouteRecordRaw } from 'vue-router';

import { requireAuth } from '../router-helper';
import { useAuth } from '@/use/auth/Auth.use';

const auth = useAuth();

export const creatorRoutes: Array<RouteRecordRaw> = [
    {
        path: '',
        component: () => import('@/views/_shared/login/LoginView.vue'),
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
        component: () => import('@/views/_shared/login/LoginView.vue'),
        meta: {
            title: 'Login',
        },
    },
    {
        path: '/dashboard',
        component: () => import('@/views/creator/dashboard/CreatorDashboardView.vue'),
        meta: {
            title: 'Creator Dashboard',
        },
    },
    {
        path: '/patterns',
        component: () => import('@/views/creator/patterns/CreatorPatternsView.vue'),
        meta: {
            title: 'Creator Patterns',
        },
    },
    {
        path: '/patterns/new',
        component: () => import('@/views/creator/new-pattern/NewPatternView.vue'),
        meta: {
            title: 'New Pattern',
        },
    },
    {
        path: '/settings',
        component: () => import('@/views/_shared/settings/SettingsView.vue'),
        beforeEnter: [ requireAuth ],
        meta: {
            title: 'Settings',
        },
    },
    {
        path: '/supported-pattern-formats',
        component: () => import('@/views/_shared/supported-pattern-formats/SupportedPatternFormatsView.vue'),
        meta: {
            title: 'Supported Pattern Formats',
        },
    },
    {
        path: '/:pathMatch(.*)*',
        component: () => import('@/views/_shared/not-found/NotFoundView.vue'),
        meta: {
            title: 'Page Not Found',
        },
    },
];