import { type NavigationGuardWithThis } from 'vue-router';
import { useAuth } from '@/use/auth/Auth.use';

const auth = useAuth();

export type Subdomain = 'creator' | 'admin';

export const subdomain = (() => {
    const split = window.location.host.split('.');
    if (split.length < 3)
        return null;

    split.pop();
    split.pop();

    return split.join('.') as Subdomain;
})();

export const requireAuth: NavigationGuardWithThis<void> = function (to, from, next): void {
    if (auth.details.value === null) {
        next('/login');
        return;
    }

    next();
};