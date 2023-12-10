<template>
    <ViewComponent class="inventory-view">
        <template #nav>
            <strong>Inventory</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <h2>Manage your Threads</h2>
                    <LoadingComponent v-if="isLoading" itemName="threads" />
                    <template v-else>
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
                                <p v-if="displayThreads.length === 0" class="text-centered">
                                    Enter a thread code above and select how many you have.
                                </p>
                                <ThreadItemComponent :key="thread.thread.reference" v-for="thread in displayThreads" :thread="thread" />
                        </section>
                    </template>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { watchDebounced } from '@vueuse/core';

import LoadingComponent from '@wjb/vue/component/LoadingComponent.vue';
import ThreadItemComponent from '@/views/stitcher/inventory/components/ThreadItem.component.vue';

import { api } from '@/api/api';

import type { IInventoryThread } from '@/models/Inventory.model';

const searchTerm = ref<string>('');
const searchTermSanitised = computed<string>(() => searchTerm.value.trim().toLowerCase());

const isLoading = ref<boolean>(false);
const threads = ref<Array<IInventoryThread>>([]);
const displayThreads = ref<Array<IInventoryThread>>([]);

watchDebounced(searchTerm, () => {
    displayThreads.value = threads.value
        .filter(x => x.thread.code.toLowerCase().indexOf(searchTermSanitised.value) > -1);

    displayThreads.value.sort((a, b) => {
        if (b.thread.code.startsWith(searchTermSanitised.value))
            return 1;
        if (a.thread.code.startsWith(searchTermSanitised.value))
            return -1;

        return 0;
    });

    displayThreads.value.sort((a, b) => a.thread.code.localeCompare(b.thread.code));
}, {
    debounce: 300,
});

onMounted(async () => {
    isLoading.value = true;
    const result = await api.inventory.getThreads();
    isLoading.value = false;

    if (result instanceof Error)
        return;

    threads.value = result;
    displayThreads.value = threads.value;
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