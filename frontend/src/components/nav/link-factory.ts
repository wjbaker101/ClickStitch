import { type ComputedRef } from 'vue';

import { adminLinks } from '@/components/nav/links/admin.links';
import { creatorLinks } from '@/components/nav/links/creator.links';
import { stitcherLinks } from '@/components/nav/links/stitcher.links';

import { type Subdomain } from '@/setup/router/router-helper';

import { type ILink } from '@/components/nav/types/Link.type';

export const linkFactory = {

    get(subdomain: Subdomain | null): ComputedRef<Array<ILink>> {
        switch (subdomain) {
            case 'admin':
                return adminLinks;
            case 'creator':
                return creatorLinks;
            default:
                return stitcherLinks;
        }
    },

};