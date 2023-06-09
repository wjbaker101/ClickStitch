import { RouteRecordRaw } from 'vue-router';

import LoginView from '@/views/_shared/login/Login.view.vue';
import SettingsView from '@/views/_shared/settings/Settings.view.vue';
import CreatorDashboardView from '@/views/creator/dashboard/CreatorDashboard.view.vue';
import CreatorPatternsView from '@/views/creator/patterns/CreatorPatterns.view.vue';
import NewPatternView from '@/views/creator/new-pattern/NewPattern.view.vue';
import NotFoundView from '@/views/_shared/not-found/NotFound.view.vue';

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
        path: '/patterns',
        component: CreatorPatternsView,
        meta: {
            title: 'Creator Patterns',
        },
    },
    {
        path: '/patterns/new',
        component: NewPatternView,
        meta: {
            title: 'New Pattern',
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