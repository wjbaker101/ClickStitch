import { type NavigationGuardWithThis } from 'vue-router';
import { useAuth } from '@/use/auth/Auth.use';

const auth = useAuth();

const getSubdomain = function (hostName: string): Subdomain | null {
    const split = hostName.split('.');

    if (split[0] === 'localhost' || split[0] === 'clickstitch')
        return null;

    return split[0] as Subdomain;
};

export type Subdomain = 'creator' | 'admin';

export const subdomain = getSubdomain(window.location.hostname);

export const requireAuth: NavigationGuardWithThis<void> = function (to, from, next): void {
    if (auth.details.value === null) {
        next('/login');
        return;
    }

    next();
};