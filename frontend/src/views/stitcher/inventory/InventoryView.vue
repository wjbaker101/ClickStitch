<template>
    <ViewComponent>
        <template #nav>
            <strong>Inventory</strong>
        </template>
        <CardComponent border="top" padded class="mb-4">
            <h2 class="mb-4 text-2xl font-bold">Manage your Skeins</h2>
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
                        <SelectComponent
                            v-model="searchBrand"
                            :options="[
                                { value: null, description: 'All' },
                                { value: 'Anchor', description: 'Anchor' },
                                { value: 'DMC', description: 'DMC' },
                            ]"
                        />
                    </label>
                </div>
            </FormComponent>
        </CardComponent>
        <LoadingComponent v-if="isLoading" itemName="threads" />
        <template v-else>
            <p v-if="inventoryThreads.length === 0 && availableThreads.length === 0" class="text-center">
                Enter a thread code above and select how many you have.
            </p>
            <div class="grid grid-cols-[repeat(auto-fill,minmax(240px,1fr))] gap-4">
                <InventoryThreadComponent :key="thread.thread.reference" v-for="thread in inventoryThreads" :thread="thread" />
                <InventoryThreadComponent :key="thread.thread.reference" v-for="thread in availableThreads" :thread="thread"/>
            </div>
        </template>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { watchDebounced } from '@vueuse/core';

import LoadingComponent from '@/components/loading/LoadingComponent.vue';
import FormComponent from '@/components/form/FormComponent.vue';
import TextboxComponent from '@/components/inputs/InputComponent.vue';
import SelectComponent from '@/components/inputs/SelectComponent.vue';
import InventoryThreadComponent from '@/views/stitcher/inventory/components/InventoryThreadComponent.vue';

import { api } from '@/api/api';
import { threadMapper } from '@/api/mappers/Thread.mapper';

import type { IInventoryThread } from '@/models/Inventory.model';

const searchTerm = ref<string>('');
const searchBrand = ref<string | null>(null);

const isLoading = ref<boolean>(false);
const inventoryThreads = ref<Array<IInventoryThread>>([]);
const availableThreads = ref<Array<IInventoryThread>>([]);

const loadThreads = async function () {
    isLoading.value = true;
    const result = await api.inventory.searchThreads(searchTerm.value, searchBrand.value);
    isLoading.value = false;

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

watchDebounced(searchTerm, async () => {
    await loadThreads();
},
{ debounce: 300 });

watchDebounced(searchBrand, async () => {
    await loadThreads();
},
{ debounce: 300 });

onMounted(async () => {
    await loadThreads();
});
</script>