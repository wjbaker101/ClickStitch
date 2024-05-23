import { type NavigationGuardNext, type RouteLocationNormalized, type RouteRecordRaw } from 'vue-router';

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
        component: () => import('@/views/stitcher/about/AboutView.vue'),
        meta: {
            title: 'About',
        },
    },
    {
        path: '/login',
        component: () => import('@/views/_shared/login/LoginView.vue'),
        beforeEnter: [ requireNoAuth ],
        meta: {
            title: 'Login',
        },
    },
    {
        path: '/signup',
        component: () => import('@/views/_shared/signup/SignupView.vue'),
        meta: {
            title: 'Signup',
        },
    },
    {
        path: '/dashboard',
        component: () => import('@/views/stitcher/dashboard/DashboardView.vue'),
        beforeEnter: [ requireAuth ],
        meta: {
            title: 'Dashboard',
        },
    },
    {
        path: '/patterns',
        component: () => import('@/views/stitcher/patterns/PatternsView.vue'),
        meta: {
            title: 'Patterns',
        },
    },
    {
        path: '/patterns/new',
        component: () => import('@/views/stitcher/new-pattern/NewPatternView.vue'),
        meta: {
            title: 'New Pattern',
        },
    },
    {
        path: '/inventory',
        component: () => import('@/views/stitcher/inventory/InventoryView.vue'),
        meta: {
            title: 'Inventory',
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
        path: '/projects/:patternReference',
        component: () => import('@/views/stitcher/project/ProjectView.vue'),
        beforeEnter: [ requireAuth ],
        meta: {
            title: 'Project',
        },
    },
    {
        path: '/projects/:patternReference/edit',
        component: () => import('@/views/stitcher/project-editor/ProjectEditorView.vue'),
        beforeEnter: [ requireAuth ],
        meta: {
            title: 'Edit Project',
        },
    },
    {
        path: '/projects/:patternReference/analytics',
        component: () => import('@/views/stitcher/project-analytics/ProjectAnalyticsView.vue'),
        beforeEnter: [ requireAuth ],
        meta: {
            title: 'Project Analytics',
        },
    },
    {
        path: '/about',
        component: () => import('@/views/stitcher/about/AboutView.vue'),
        meta: {
            title: 'About',
        },
    },
    {
        path: '/tools',
        component: () => import('@/views/stitcher/tools/ToolsView.vue'),
        meta: {
            title: 'Tools',
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
        path: '/marketing',
        component: () => import('@/views/_shared/marketing/MarketingView.vue'),
        meta: {
            title: 'Marketing',
        },
    },
    {
        path: '/creators/:creatorReference',
        component: () => import('@/views/stitcher/creator/CreatorView.vue'),
        meta: {
            title: 'Creator',
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