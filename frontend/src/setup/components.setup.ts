import { Plugin } from 'vue';

import CardComponent from '@/components/Card.component.vue';
import LinkComponent from '@/components/Link.component.vue';
import ViewComponent from '@/components/View.component.vue';

export const appComponents: Plugin = {

    install(app) {
        app.component('CardComponent', CardComponent);
        app.component('LinkComponent', LinkComponent);
        app.component('ViewComponent', ViewComponent);
    },
};