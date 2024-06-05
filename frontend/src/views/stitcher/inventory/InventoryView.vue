<template>
    <ViewComponent>
        <template #nav>
            <strong>Inventory</strong>
        </template>
        <CardComponent border="top" padded>
            <h2 class="mb-4 text-2xl font-bold">Manage your Skeins</h2>
            <LoadingComponent v-if="isLoading" itemName="threads" />
            <template v-else>
                <FormComponent class="mb-4 flex items-center">
                    <div class="grow">
                        <label>
                            <strong class="block">Search</strong>
                            <TextboxComponent type="search" placeholder="DMC 814" v-model="searchTerm" />
                        </label>
                    </div>
                    <div>
                        <label>
                            <strong class="block">Brand</strong>
                            <select v-model="searchBrand">
                                <option :value="null">All</option>
                                <option value="Anchor">Anchor</option>
                                <option value="DMC">DMC</option>
                            </select>
                        </label>
                    </div>
                </FormComponent>
                <p v-if="inventoryThreads.length === 0" class="text-center">
                    Enter a thread code above and select how many you have.
                </p>
                <ThreadItemComponent :key="thread.thread.reference" v-for="thread in inventoryThreads" :thread="thread" @update="onThreadUpdate" />
                <p v-if="availableThreads.length > 0" class="text-center">
                    Looking for something else?
                </p>
                <ThreadItemComponent :key="thread.thread.reference" v-for="thread in availableThreads" :thread="thread" @update="onThreadUpdate" />
            </template>
        </CardComponent>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { watchDebounced } from '@vueuse/core';

import LoadingComponent from '@/components/loading/LoadingComponent.vue';
import FormComponent from '@/components/form/FormComponent.vue';
import TextboxComponent from '@/components/inputs/InputComponent.vue';
import ThreadItemComponent from '@/views/stitcher/inventory/components/ThreadItemComponent.vue';

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