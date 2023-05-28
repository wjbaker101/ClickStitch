import { createRouter, createWebHistory, NavigationGuardWithThis, RouteRecordRaw } from 'vue-router';

import AboutView from '@/views/about/About.view.vue';
import AdminView from '@/views/admin/Admin.view.vue';
import DashboardView from '@/views/dashboard/Dashboard.view.vue';
import BasketView from '@/views/basket/Basket.view.vue';
import LoginView from '@/views/login/Login.view.vue';
import MarketplaceView from '@/views/marketplace/Marketplace.view.vue';
import ProjectView from '@/views/project/Project.view.vue';
import ProjectAnalyticsView from '@/views/project-analytics/ProjectAnalytics.view.vue';
import SettingsView from '@/views/settings/Settings.view.vue';
import SignupView from '@/views/signup/Signup.view.vue';

import { useAuth } from '@/use/auth/Auth.use';
import { setTitle } from '@/helper/helper';

const auth = useAuth();

const requireAuth: NavigationGuardWithThis<void> = function (to, from, next): void {
    if (auth.details.value === null) {
        next('/login');
        return;
    }

    next();
};

const routes: Array<RouteRecordRaw> = [
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
        path: '/admin',
        component: AdminView,
        meta: {
            title: 'Admin',
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
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

router.beforeEach((to) => {
    if (to.meta.title)
        setTitle(to.meta.title as string);
});

export { router };