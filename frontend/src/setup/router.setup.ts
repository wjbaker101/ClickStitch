import { createRouter, createWebHistory, NavigationGuardWithThis, RouteRecordRaw } from 'vue-router';

import DashboardView from '@/views/dashboard/Dashboard.view.vue';
import BasketView from '@/views/basket/Basket.view.vue';
import LoginView from '@/views/login/Login.view.vue';
import MarketplaceView from '@/views/marketplace/Marketplace.view.vue';
import ProjectView from '@/views/project/Project.view.vue';
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
        beforeEnter: [ requireAuth ],
        meta: {
            title: 'Marketplace',
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
        path: '/project/:patternReference',
        component: ProjectView,
        beforeEnter: [ requireAuth ],
        meta: {
            title: 'Project',
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