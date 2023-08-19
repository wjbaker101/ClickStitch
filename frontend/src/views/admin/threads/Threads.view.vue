<template>
    <ViewComponent class="threads-view">
        <template #nav>
            <strong>Threads</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <h2>Threads</h2>
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

import { api } from '@/api/api';

import type { IInventoryThread } from '@/models/Inventory.model';

const threads = ref<Array<IInventoryThread> | null>(null);

onMounted(async () => {
    const threadsResult = await api.inventory.getThreads();
    if (threadsResult instanceof Error)
        return;

    threads.value = threadsResult;
});
</script>

<style lang="scss">
.threads-view {
}
</style>