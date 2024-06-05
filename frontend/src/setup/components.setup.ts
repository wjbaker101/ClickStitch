import { type Plugin } from 'vue';

import CardComponent from '@/components/CardComponent.vue';
import LinkComponent from '@/components/LinkComponent.vue';
import ViewComponent from '@/components/ViewComponent.vue';

export const components: Plugin = {

    install(app) {
        app.component('CardComponent', CardComponent);
        app.component('LinkComponent', LinkComponent);
        app.component('ViewComponent', ViewComponent);
    },
};