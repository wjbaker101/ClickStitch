<template>
    <div class="paginated-content-component">
        <LoadingComponent v-if="isLoading" :itemName="loadingItemName" />
        <div v-else-if="failure !== null">
            {{ failure.message }}
        </div>
        <template v-else-if="logicResult !== null">
            <PaginationComponent :pagination="logicResult" @update="onUpdate" />
            <slot></slot>
            <PaginationComponent :pagination="logicResult" @update="onUpdate" />
        </template>
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import LoadingComponent from '@wjb/vue/component/LoadingComponent.vue';
import PaginationComponent from '@/components/paginated-content/Pagination.component.vue';

import { type IPagination } from '@/models/Pagination.model';
import { type IPaginationEvent } from '@/components/paginated-content/PaginationEvent';

const props = defineProps<{
    loadingItemName?: string;
    logic: (pageNumber: number, pageSize: number) => Promise<IPagination | Error>;
}>();

const isLoading = ref<boolean>(false);
const pageNumber = ref<number>(1);
const pageSize = ref<number>(5);

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
.paginated-content-component {
    margin: 1rem 0;
}
</style>