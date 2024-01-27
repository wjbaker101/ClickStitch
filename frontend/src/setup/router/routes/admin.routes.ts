import { type RouteRecordRaw } from 'vue-router';

import { requireAuth } from '../router-helper';
import { useAuth } from '@/use/auth/Auth.use';

const auth = useAuth();

export const adminRoutes: Array<RouteRecordRaw> = [
    {
        path: '',
        component: () => import('@/views/_shared/login/Login.view.vue'),
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
        component: () => import('@/views/_shared/login/Login.view.vue'),
        meta: {
            title: 'Login',
        },
    },
    {
        path: '/users',
        component: () => import('@/views/admin/admin/Admin.view.vue'),
        meta: {
            title: 'Users',
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
        path: '/:pathMatch(.*)*',
        component: () => import('@/views/_shared/not-found/NotFound.view.vue'),
        meta: {
            title: 'Page Not Found',
        },
    },
];