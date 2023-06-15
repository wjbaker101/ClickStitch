import { createRouter, createWebHistory } from 'vue-router';

import { setTitle } from '@/helper/helper';
import { routerHelper } from './router/router-helper';
import { routeFactory } from './router/route-factory';

const subdomain = routerHelper.subdomain();

const router = createRouter({
    history: createWebHistory(),
    routes: routeFactory.get(subdomain),
});

router.beforeEach((to) => {
    if (to.meta.title)
        setTitle(to.meta.title as string);
});

export { router };