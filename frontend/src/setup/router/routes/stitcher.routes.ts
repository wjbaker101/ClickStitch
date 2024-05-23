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
        component: () => import('@/views/stitcher/about/About.view.vue'),
        meta: {
            title: 'About',
        },
    },
    {
        path: '/login',
        component: () => import('@/views/_shared/login/Login.view.vue'),
        beforeEnter: [ requireNoAuth ],
        meta: {
            title: 'Login',
        },
    },
    {
        path: '/signup',
        component: () => import('@/views/_shared/signup/Signup.view.vue'),
        meta: {
            title: 'Signup',
        },
    },
    {
        path: '/dashboard',
        component: () => import('@/views/stitcher/dashboard/Dashboard.view.vue'),
        beforeEnter: [ requireAuth ],
        meta: {
            title: 'Dashboard',
        },
    },
    {
        path: '/patterns',
        component: () => import('@/views/stitcher/patterns/Patterns.view.vue'),
        meta: {
            title: 'Patterns',
        },
    },
    {
        path: '/patterns/new',
        component: () => import('@/views/stitcher/new-pattern/NewPattern.view.vue'),
        meta: {
            title: 'New Pattern',
        },
    },
    {
        path: '/inventory',
        component: () => import('@/views/stitcher/inventory/Inventory.view.vue'),
        meta: {
            title: 'Inventory',
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
        path: '/projects/:patternReference',
        component: () => import('@/views/stitcher/project/Project.view.vue'),
        beforeEnter: [ requireAuth ],
        meta: {
            title: 'Project',
        },
    },
    {
        path: '/projects/:patternReference/edit',
        component: () => import('@/views/stitcher/project-editor/ProjectEditor.view.vue'),
        beforeEnter: [ requireAuth ],
        meta: {
            title: 'Edit Project',
        },
    },
    {
        path: '/projects/:patternReference/analytics',
        component: () => import('@/views/stitcher/project-analytics/ProjectAnalytics.view.vue'),
        beforeEnter: [ requireAuth ],
        meta: {
            title: 'Project Analytics',
        },
    },
    {
        path: '/about',
        component: () => import('@/views/stitcher/about/About.view.vue'),
        meta: {
            title: 'About',
        },
    },
    {
        path: '/tools',
        component: () => import('@/views/stitcher/tools/Tools.view.vue'),
        meta: {
            title: 'Tools',
        },
    },
    {
        path: '/supported-pattern-formats',
        component: () => import('@/views/_shared/supported-pattern-formats/SupportedPatternFormats.view.vue'),
        meta: {
            title: 'Supported Pattern Formats',
        },
    },
    {
        path: '/marketing',
        component: () => import('@/views/_shared/marketing/Marketing.view.vue'),
        meta: {
            title: 'Marketing',
        },
    },
    {
        path: '/creators/:creatorReference',
        component: () => import('@/views/stitcher/creator/Creator.view.vue'),
        meta: {
            title: 'Creator',
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