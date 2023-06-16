import { computed } from 'vue';

import { ILink } from '@/components/nav/types/Link.type';

export const creatorLinks = computed<Array<ILink>>(() => [
    {
        path: '/dashboard',
        iconName: 'home',
        title: 'Dashboard',
        isVisible: true,
    },
    {
        path: '/settings',
        iconName: 'settings',
        title: 'Settings',
        isVisible: true,
    },
]);