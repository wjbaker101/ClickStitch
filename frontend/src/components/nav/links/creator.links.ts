import { computed } from 'vue';
import { HomeIcon, PencilIcon, SettingsIcon } from 'lucide-vue-next';

import { type ILink } from '@/components/nav/types/Link.type';

export const creatorLinks = computed<Array<ILink>>(() => [
    {
        path: '/dashboard',
        iconComponent: HomeIcon,
        title: 'Dashboard',
        isVisible: true,
    },
    {
        path: '/patterns',
        iconComponent: PencilIcon,
        title: 'Patterns',
        isVisible: true,
    },
    {
        path: '/settings',
        iconComponent: SettingsIcon,
        title: 'Settings',
        isVisible: true,
    },
]);