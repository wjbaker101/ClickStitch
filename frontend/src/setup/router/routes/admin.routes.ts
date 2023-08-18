import { type RouteRecordRaw } from 'vue-router';

import AdminView from '@/views/admin/admin/Admin.view.vue';
import LoginView from '@/views/_shared/login/Login.view.vue';
import SettingsView from '@/views/_shared/settings/Settings.view.vue';
import NotFoundView from '@/views/_shared/not-found/NotFound.view.vue';

import { requireAuth } from '../router-helper';
import { useAuth } from '@/use/auth/Auth.use';

const auth = useAuth();

export const adminRoutes: Array<RouteRecordRaw> = [
    {
        path: '',
        component: LoginView,
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
        component: LoginView,
        meta: {
            title: 'Login',
        },
    },
    {
        path: '/users',
        component: AdminView,
        meta: {
            title: 'Users',
        },
    },
    {
        path: '/settings',
        component: SettingsView,
        beforeEnter: [ requireAuth ],
        meta: {
            title: 'Settings',
        },
    },
    {
        path: '/:pathMatch(.*)*',
        component: NotFoundView,
        meta: {
            title: 'Page Not Found',
        },
    },
];