import { Plugin } from 'vue';

import CardComponent from '@/components/Card.component.vue';

export const appComponents: Plugin = {

    install(app) {
        app.component('CardComponent', CardComponent);
    },
};