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
                                    <div>
                                        <FormInputComponent label="Search">
                                            <input type="search" placeholder="DMC 814" v-model="searchTerm">
                                        </FormInputComponent>
                                    </div>
                                    <div class="flex-auto">
                                        <FormInputComponent label="Brand">
                                            <select v-model="searchBrand">
                                                <option :value="null">All</option>
                                                <option value="Anchor">Anchor</option>
                                                <option value="DMC">DMC</option>
                                            </select>
                                        </FormInputComponent>
                                    </div>
                                </FormSectionComponent>
                            </FormComponent>
                        </section>
                        <section>
                            <p v-if="inventoryThreads.length === 0" class="text-centered">
                                Enter a thread code above and select how many you have.
                            </p>
                            <ThreadItemComponent :key="thread.thread.reference" v-for="thread in inventoryThreads" :thread="thread" @update="onThreadUpdate" />
                            <p v-if="availableThreads.length > 0" class="text-centered">
                                Looking for something else?
                            </p>
                            <ThreadItemComponent :key="thread.thread.reference" v-for="thread in availableThreads" :thread="thread" @update="onThreadUpdate" />
                        </section>
                    </template>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { watchDebounced } from '@vueuse/core';

import LoadingComponent from '@wjb/vue/component/LoadingComponent.vue';
import ThreadItemComponent from '@/views/stitcher/inventory/components/ThreadItem.component.vue';

import { api } from '@/api/api';
import { threadMapper } from '@/api/mappers/Thread.mapper';

import type { IInventoryThread } from '@/models/Inventory.model';

const searchTerm = ref<string>('');
const searchBrand = ref<string | null>(null);

const isLoading = ref<boolean>(false);
const inventoryThreads = ref<Array<IInventoryThread>>([]);
const availableThreads = ref<Array<IInventoryThread>>([]);

const loadThreads = async function () {
    const result = await api.inventory.searchThreads(searchTerm.value, searchBrand.value);

    if (result instanceof Error)
        return;

    inventoryThreads.value = result.inventoryThreads.map(x => ({
        thread: threadMapper.map(x.thread),
        count: x.count,
    }));

    availableThreads.value = result.availableThreads.map(x => ({
        thread: threadMapper.map(x),
        count: 0,
    }));
};

const onThreadUpdate = async function () {
    await loadThreads();
};

watchDebounced(searchTerm, async () => {
    await loadThreads();
},
{ debounce: 300 });

watchDebounced(searchBrand, async () => {
    await loadThreads();
},
{ debounce: 300 });

onMounted(async () => {
    isLoading.value = true;
    await loadThreads();
    isLoading.value = false;
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