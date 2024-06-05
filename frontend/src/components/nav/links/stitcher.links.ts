import { computed } from 'vue';
import { HomeIcon, DownloadIcon, InfoIcon, SettingsIcon, UserIcon } from 'lucide-vue-next';
import SkeinIcon from '@/components/icons/SkeinIcon.vue';

import { useAuth } from '@/use/auth/Auth.use';

import { type ILink } from '@/components/nav/types/Link.type';

const auth = useAuth();

const authDetails = auth.details;

export const stitcherLinks = computed<Array<ILink>>(() => [
    {
        path: '/dashboard',
        iconComponent: HomeIcon,
        title: 'Dashboard',
        isVisible: authDetails.value !== null,
    },
    {
        path: '/patterns',
        iconComponent: DownloadIcon,
        title: 'Patterns',
        isVisible: true,
    },
    {
        path: '/inventory',
        iconComponent: SkeinIcon,
        title: 'Inventory',
        isVisible: authDetails.value !== null,
    },
    {
        path: '/about',
        iconComponent: InfoIcon,
        title: 'About',
        isVisible: true,
    },
    {
        path: '/settings',
        iconComponent: SettingsIcon,
        title: 'Settings',
        isVisible: authDetails.value !== null,
    },
    {
        path: '/login',
        iconComponent: UserIcon,
        title: 'Login/Signup',
        isVisible: authDetails.value === null,
    },
]);