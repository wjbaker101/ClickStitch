import { RouteRecordRaw } from 'vue-router';

import LoginView from '@/views/_shared/login/Login.view.vue';
import SettingsView from '@/views/_shared/settings/Settings.view.vue';
import CreatorDashboardView from '@/views/creator/dashboard/CreatorDashboard.view.vue';

import { requireAuth } from '../router-helper';
import { useAuth } from '@/use/auth/Auth.use';

const auth = useAuth();

export const creatorRoutes: Array<RouteRecordRaw> = [
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
        component: CreatorDashboardView,
        meta: {
            title: 'Creator Dashboard',
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
];