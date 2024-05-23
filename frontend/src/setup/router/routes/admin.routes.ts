import { type RouteRecordRaw } from 'vue-router';

import { requireAuth } from '../router-helper';
import { useAuth } from '@/use/auth/Auth.use';

const auth = useAuth();

export const adminRoutes: Array<RouteRecordRaw> = [
    {
        path: '',
        component: () => import('@/views/_shared/login/LoginView.vue'),
        beforeEnter: (to, from, next) => {
            if (auth.details.value === null) {
                next('/login');
                return;
            }

            next('/users');
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
        path: '/users',
        component: () => import('@/views/admin/admin/AdminView.vue'),
        meta: {
            title: 'Users',
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
        path: '/:pathMatch(.*)*',
        component: () => import('@/views/_shared/not-found/NotFoundView.vue'),
        meta: {
            title: 'Page Not Found',
        },
    },
];