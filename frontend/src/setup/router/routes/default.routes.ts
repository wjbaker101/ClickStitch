import { type RouteRecordRaw } from 'vue-router';

import AboutView from '@/views/default/about/About.view.vue';
import DashboardView from '@/views/default/dashboard/Dashboard.view.vue';
import BasketView from '@/views/default/basket/Basket.view.vue';
import LoginView from '@/views/_shared/login/Login.view.vue';
import MarketplaceView from '@/views/default/marketplace/Marketplace.view.vue';
import ProjectView from '@/views/default/project/Project.view.vue';
import ToolsView from '@/views/default/tools/Tools.view.vue';
import ProjectAnalyticsView from '@/views/default/project-analytics/ProjectAnalytics.view.vue';
import SettingsView from '@/views/_shared/settings/Settings.view.vue';
import SignupView from '@/views/_shared/signup/Signup.view.vue';
import NotFoundView from '@/views/_shared/not-found/NotFound.view.vue';
import SupportedPatternFormats from '@/views/_shared/supported-pattern-formats/SupportedPatternFormats.view.vue';

import { requireAuth } from '../router-helper';
import { useAuth } from '@/use/auth/Auth.use';

const auth = useAuth();

export const defaultRoutes: Array<RouteRecordRaw> = [
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
        path: '/signup',
        component: SignupView,
        meta: {
            title: 'Signup',
        },
    },
    {
        path: '/dashboard',
        component: DashboardView,
        beforeEnter: [ requireAuth ],
        meta: {
            title: 'Dashboard',
        },
    },
    {
        path: '/marketplace',
        component: MarketplaceView,
        meta: {
            title: 'Marketplace',
        },
    },
    {
        path: '/patterns',
        component: MarketplaceView,
        meta: {
            title: 'Patterns',
        },
    },
    {
        path: '/basket',
        component: BasketView,
        beforeEnter: [ requireAuth ],
        meta: {
            title: 'Basket',
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
        path: '/projects/:patternReference',
        component: ProjectView,
        beforeEnter: [ requireAuth ],
        meta: {
            title: 'Project',
        },
    },
    {
        path: '/projects/:patternReference/analytics',
        component: ProjectAnalyticsView,
        beforeEnter: [ requireAuth ],
        meta: {
            title: 'Project Analytics',
        },
    },
    {
        path: '/about',
        component: AboutView,
        meta: {
            title: 'About',
        },
    },
    {
        path: '/tools',
        component: ToolsView,
        meta: {
            title: 'Tools',
        },
    },
    {
        path: '/supported-pattern-formats',
        component: SupportedPatternFormats,
        meta: {
            title: 'Supported File Formats',
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