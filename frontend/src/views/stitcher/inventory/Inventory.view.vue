<template>
    <ViewComponent class="inventory-view">
        <template #nav>
            <strong>Inventory</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <h2>Manage Your Threads</h2>
                    <section>
                        <FormComponent>
                            <FormSectionComponent class="flex align-items-center">
                                <FormInputComponent label="Search">
                                    <input type="search" placeholder="DMC 814" v-model="searchTerm">
                                </FormInputComponent>
                            </FormSectionComponent>
                        </FormComponent>
                    </section>
                    <section>
                        <p v-if="threads.length === 0" class="text-centered">
                            Enter a thread code above and select how many you have.
                        </p>
                        <ThreadItemComponent v-for="thread in threads" :thread="thread" />
                    </section>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';

import ThreadItemComponent from '@/views/stitcher/inventory/components/ThreadItem.component.vue';

import { api } from '@/api/api';

import type { IInventoryThread } from '@/models/Inventory.model';

const searchTerm = ref<string>('');
const searchTermSanitised = computed<string>(() => searchTerm.value.trim().toLowerCase());

const threads = ref<Array<IInventoryThread>>([]);


watch(searchTerm, () => {
    if (searchTermSanitised.value.length < 1)
        return;
});

onMounted(async () => {
    const result = await api.inventory.getThreads();
    if (result instanceof Error)
        return;

    threads.value = result;
});
</script>

<style lang="scss">
.inventory-view {

    .thread-image {
        width: 4rem;
        aspect-ratio: 1;
        border-radius: 50%;
        vertical-align: middle;
        box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);
    }
}
</style>