<template>
    <ViewComponent class="creator-dashboard-view">
        <template #nav>
            <strong>Creator Dashboard</strong>
        </template>
        <div class="content-width">
            <div v-if="self === null">
                <LoadingComponent itemName="user details" />
            </div>
            <template v-else>
                <section v-if="isCreator">
                    <CardComponent border="top" padded>
                        <EditCreatorComponent />
                    </CardComponent>
                </section>
            </template>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';

import LoadingComponent from '@/components/loading/LoadingComponent.vue';
import EditCreatorComponent from '@/views/creator/dashboard/components/EditCreatorComponent.vue';

import { api } from '@/api/api';

import { type IGetSelf } from '@/models/GetSelf.model';

const self = ref<IGetSelf | null>(null);

const isCreator = computed<boolean>(() => self.value?.permissions.find(x => x.type === 'Creator') !== undefined);

onMounted(async () => {
    const selfResult = await api.users.getSelf();
    if (selfResult instanceof Error)
        return;

    self.value = selfResult;
});
</script>