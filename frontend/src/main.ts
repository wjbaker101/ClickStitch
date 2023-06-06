import { createApp } from 'vue';

import '@/setup/dayjs.setup';

import App from '@/App.vue';

import { router } from '@/setup/router.setup';
import { components } from '@/setup/components.setup';

const app = createApp(App);

app.use(router);
app.use(components);

app.mount('#app');