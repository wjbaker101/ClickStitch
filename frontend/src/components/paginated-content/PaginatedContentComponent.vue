<template>
    <div class="py-4">
        <LoadingComponent v-if="isLoading" :itemName="loadingItemName" />
        <div v-else-if="failure !== null">
            {{ failure.message }}
        </div>
        <template v-else-if="logicResult !== null">
            <PaginationControlsComponent :pagination="logicResult" @update="onUpdate" />
            <slot></slot>
            <PaginationControlsComponent :pagination="logicResult" @update="onUpdate" />
        </template>
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import LoadingComponent from '@/components/loading/LoadingComponent.vue';
import PaginationControlsComponent from '@/components/paginated-content/PaginationControlsComponent.vue';

import { type IPagination } from '@/models/Pagination.model';
import { type IPaginationEvent } from '@/components/paginated-content/PaginationEvent';

const props = defineProps<{
    loadingItemName?: string;
    logic: (pageNumber: number, pageSize: number) => Promise<IPagination | Error>;
    pageSize?: number;
}>();

const isLoading = ref<boolean>(false);
const pageNumber = ref<number>(1);
const pageSize = ref<number>(props.pageSize ?? 5);

const logicResult = ref<IPagination | null>(null);
const failure = ref<Error | null>(null);

const executeLogic = async function (): Promise<void> {
    isLoading.value = true;

    const result = await props.logic(pageNumber.value, pageSize.value);
    if (result instanceof Error)
        failure.value = result;
    else
        logicResult.value = result;

    isLoading.value = false;
};

onMounted(async () => {
    await executeLogic();
});

const onUpdate = async function (event: IPaginationEvent): Promise<void> {
    pageNumber.value = event.pageNumber;
    pageSize.value = event.pageSize;

    await executeLogic();
};
</script>

<style lang="scss">
</style>