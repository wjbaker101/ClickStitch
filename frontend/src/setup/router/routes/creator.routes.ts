import { RouteRecordRaw } from 'vue-router';

import LoginView from '@/views/login/Login.view.vue';
import SettingsView from '@/views/settings/Settings.view.vue';

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
        path: '/settings',
        component: SettingsView,
        beforeEnter: [ requireAuth ],
        meta: {
            title: 'Settings',
        },
    },
];