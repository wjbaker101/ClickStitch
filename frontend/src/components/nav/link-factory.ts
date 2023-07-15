import { type ComputedRef } from 'vue';

import { creatorLinks } from '@/components/nav/links/creator.links';
import { defaultLinks } from '@/components/nav/links/default.links';

import { type Subdomain } from '@/setup/router/router-helper';

import { type ILink } from '@/components/nav/types/Link.type';

export const linkFactory = {

    get(subdomain: Subdomain | null): ComputedRef<Array<ILink>> {
        switch (subdomain) {
            case 'creator':
                return creatorLinks;
            default:
                return defaultLinks;
        }
    },

};