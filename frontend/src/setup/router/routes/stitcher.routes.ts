import { type NavigationGuardNext, type RouteLocationNormalized, type RouteRecordRaw } from 'vue-router';

import AboutView from '@/views/stitcher/about/About.view.vue';
import DashboardView from '@/views/stitcher/dashboard/Dashboard.view.vue';
import BasketView from '@/views/stitcher/basket/Basket.view.vue';
import LoginView from '@/views/_shared/login/Login.view.vue';
import MarketplaceView from '@/views/stitcher/marketplace/Marketplace.view.vue';
import ProjectView from '@/views/stitcher/project/Project.view.vue';
import ToolsView from '@/views/stitcher/tools/Tools.view.vue';
import ProjectAnalyticsView from '@/views/stitcher/project-analytics/ProjectAnalytics.view.vue';
import InventoryView from '@/views/stitcher/inventory/Inventory.view.vue';
import SettingsView from '@/views/_shared/settings/Settings.view.vue';
import SignupView from '@/views/_shared/signup/Signup.view.vue';
import NotFoundView from '@/views/_shared/not-found/NotFound.view.vue';
import SupportedPatternFormats from '@/views/_shared/supported-pattern-formats/SupportedPatternFormats.view.vue';

import { requireAuth } from '../router-helper';
import { useAuth } from '@/use/auth/Auth.use';

const auth = useAuth();

const requireNoAuth = (to: RouteLocationNormalized, from: RouteLocationNormalized, next: NavigationGuardNext) => {
    if (auth.details.value !== null) {
        next('/dashboard');
        return;
    }

    next();
};

export const stitcherRoutes: Array<RouteRecordRaw> = [
    {
        path: '',
        component: AboutView,
        meta: {
            title: 'About',
        },
    },
    {
        path: '/login',
        component: LoginView,
        beforeEnter: [ requireNoAuth ],
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
        path: '/inventory',
        component: InventoryView,
        meta: {
            title: 'Inventory',
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