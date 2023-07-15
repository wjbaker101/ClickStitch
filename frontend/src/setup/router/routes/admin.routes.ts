import { type RouteRecordRaw } from 'vue-router';

import AdminView from '@/views/admin/admin/Admin.view.vue';
import LoginView from '@/views/_shared/login/Login.view.vue';
import SettingsView from '@/views/_shared/settings/Settings.view.vue';
import NotFoundView from '@/views/_shared/not-found/NotFound.view.vue';
import SupportedPatternFormats from '@/views/_shared/supported-pattern-formats/SupportedPatternFormats.view.vue';

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

            next('/dashboard');
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
        path: '/dashboard',
        component: AdminView,
        meta: {
            title: 'Dashboard',
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
        path: '/supported-pattern-formats',
        component: SupportedPatternFormats,
        meta: {
            title: 'Supported Pattern Formats',
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