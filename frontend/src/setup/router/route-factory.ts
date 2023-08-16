import { type RouteRecordRaw } from 'vue-router';

import { type Subdomain } from './router-helper';

import { adminRoutes } from './routes/admin.routes';
import { creatorRoutes } from './routes/creator.routes';
import { stitcherRoutes } from './routes/stitcher.routes';

export const routeFactory = {

    get(subdomain: Subdomain | null): Array<RouteRecordRaw> {
        switch (subdomain) {
            case 'creator':
                return creatorRoutes;
            case 'admin':
                return adminRoutes;
            default:
                return stitcherRoutes;
        }
    },

};