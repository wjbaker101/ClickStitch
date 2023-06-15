<template>
    <ViewComponent class="settings-view">
        <template #nav>
            <strong>Settings</strong>
        </template>
        <div v-if="self === null">
            <LoadingComponent itemName="user details" />
        </div>
        <div v-else class="content-width">
            <section>
                <CardComponent border="top" padded v-if="authDetails !== null">
                    <h2>Authentication</h2>
                    <p><strong>Currently logged in as: </strong>{{ authDetails.email }}</p>
                    <p><strong>Session started at:</strong> {{ authDetails.loggedInAt }}</p>
                    <p class="text-centered">
                        <ButtonComponent @click="onLogOut">Log Out</ButtonComponent>
                    </p>
                </CardComponent>
            </section>
            <section v-if="isCreator">
                <CardComponent border="top" padded>
                    <EditCreatorComponent />
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';

import LoadingComponent from '@wjb/vue/component/LoadingComponent.vue';
import EditCreatorComponent from '@/views/settings/components/EditCreator.component.vue';

import { api } from '@/api/api';
import { useAuth } from '@/use/auth/Auth.use';

import { IGetSelf } from '@/models/GetSelf.model';

const auth = useAuth();
const router = useRouter();

const authDetails = auth.details;

const self = ref<IGetSelf | null>(null);

const onLogOut = function (): void {
    auth.clear();
    router.push({ path: '/login' });
};

const isCreator = computed<boolean>(() => self.value?.permissions.find(x => x.type === 'Creator') !== undefined);

onMounted(async () => {
    const selfResult = await api.users.getSelf();
    if (selfResult instanceof Error)
        return;

    self.value = selfResult;
});
</script>

<style lang="scss">
.settings-view {

    .creator-name {
        width: 350px;
        max-width: 100%;
    }
}
</style>