import { ComputedRef } from 'vue';

import { defaultLinks } from './links/default.links';

import { Subdomain } from '@/setup/router/router-helper';

import { ILink } from '@/components/nav/types/Link.type';

export const linkFactory = {

    get(subdomain: Subdomain | null): ComputedRef<Array<ILink>> {
        switch (subdomain) {
            default:
                return defaultLinks;
        }
    },

};