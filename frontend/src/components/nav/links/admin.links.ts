import { computed } from 'vue';

import { type ILink } from '@/components/nav/types/Link.type';

export const adminLinks = computed<Array<ILink>>(() => [
    {
        path: '/users',
        iconName: 'user',
        title: 'Users',
        isVisible: true,
    },
    {
        path: '/threads',
        iconName: 'suitcase',
        title: 'Threads',
        isVisible: true,
    },
    {
        path: '/settings',
        iconName: 'settings',
        title: 'Settings',
        isVisible: true,
    },
]);