import { type Plugin } from 'vue';

import IconComponent from '@wjb/vue/component/IconComponent.vue';
import FormComponent from '@wjb/vue/component/form/FormComponent.vue';
import FormInputComponent from '@wjb/vue/component/form/FormInputComponent.vue';
import FormSectionComponent from '@wjb/vue/component/form/FormSectionComponent.vue';

import CardComponent from '@/components/CardComponent.vue';
import LinkComponent from '@/components/LinkComponent.vue';
import ViewComponent from '@/components/ViewComponent.vue';

export const components: Plugin = {

    install(app) {
        app.component('IconComponent', IconComponent);

        app.component('FormComponent', FormComponent);
        app.component('FormInputComponent', FormInputComponent);
        app.component('FormSectionComponent', FormSectionComponent);

        app.component('CardComponent', CardComponent);
        app.component('LinkComponent', LinkComponent);
        app.component('ViewComponent', ViewComponent);
    },
};