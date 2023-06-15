import { RouteRecordRaw } from 'vue-router';

import { Subdomain } from './router-helper';

import { creatorRoutes } from './routes/creator.routes';
import { defaultRoutes } from './routes/default.routes';

export const routeFactory = {

    get(subdomain: Subdomain | null): Array<RouteRecordRaw> {
        switch (subdomain) {
            case 'creator':
                return creatorRoutes;
            default:
                return defaultRoutes;
        }
    },

};