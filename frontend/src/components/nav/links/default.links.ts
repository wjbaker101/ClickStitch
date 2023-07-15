import { computed } from 'vue';

import { useAuth } from '@/use/auth/Auth.use';

import { type ILink } from '@/components/nav/types/Link.type';

const auth = useAuth();

const authDetails = auth.details;

export const defaultLinks = computed<Array<ILink>>(() => [
    {
        path: '/dashboard',
        iconName: 'home',
        title: 'Dashboard',
        isVisible: authDetails.value !== null,
    },
    {
        path: '/patterns',
        iconName: 'download',
        title: 'Patterns',
        isVisible: true,
    },
    {
        path: '/about',
        iconName: 'info',
        title: 'About',
        isVisible: true,
    },
    {
        path: '/settings',
        iconName: 'settings',
        title: 'Settings',
        isVisible: authDetails.value !== null,
    },
    {
        path: '/login',
        iconName: 'user',
        title: 'Login/Signup',
        isVisible: authDetails.value === null,
    },
]);