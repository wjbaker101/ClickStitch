import { computed } from 'vue';
import { UserIcon, SettingsIcon } from 'lucide-vue-next';

import { type ILink } from '@/components/nav/types/Link.type';

export const adminLinks = computed<Array<ILink>>(() => [
    {
        path: '/users',
        iconComponent: UserIcon,
        title: 'Users',
        isVisible: true,
    },
    {
        path: '/settings',
        iconComponent: SettingsIcon,
        title: 'Settings',
        isVisible: true,
    },
]);