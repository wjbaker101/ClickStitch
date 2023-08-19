<template>
    <ViewComponent class="threads-view">
        <template #nav>
            <strong>Threads</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <div class="flex gap align-items-center">
                        <h2>Threads</h2>
                        <div class="flex-auto">
                            <ButtonComponent @click="onAddThread">
                                <IconComponent icon="plus" gap="right" />
                                <span>Add Thread</span>
                            </ButtonComponent>
                        </div>
                    </div>
                    <LoadingComponent v-if="threads === null" />
                    <div v-else>
                        <ListItemComponent
                            :key="thread.thread.reference"
                            v-for="thread in threads"
                        >
                            {{ thread.thread.description }}
                        </ListItemComponent>
                    </div>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import ListItemComponent from '@/components/ListItem.component.vue';
import AddThreadModalComponent from '@/views/admin/threads/modals/AddThreadModal.component.vue';

import { api } from '@/api/api';
import { useModal } from '@wjb/vue/use/modal.use';

import type { IInventoryThread } from '@/models/Inventory.model';

const modal = useModal();

const threads = ref<Array<IInventoryThread> | null>(null);

onMounted(async () => {
    const threadsResult = await api.inventory.getThreads();
    if (threadsResult instanceof Error)
        return;

    threads.value = threadsResult;
});

const onAddThread = function (): void {
    modal.show({
        component: AddThreadModalComponent,
        componentProps: {},
    });
};
</script>

<style lang="scss">
.threads-view {
}
</style>