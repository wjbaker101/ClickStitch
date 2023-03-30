import { createApp } from 'vue';

import App from '@/App.vue';

import { router } from '@/setup/router.setup';
import { appComponents } from '@/setup/components.setup';
import { components } from '@wjb/vue/setup/components';

const app = createApp(App);

app.use(router);
app.use(appComponents);
app.use(components);

app.mount('#app');