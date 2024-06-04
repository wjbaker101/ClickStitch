<template>
    <div class="my-4 flex items-center gap-4 overflow-hidden rounded-md shadow-md bg-background">
        <div @click="onIncrement(-1)" class="grow cursor-pointer p-4 text-center hover:bg-background-dark">
            <IconComponent icon="arrow-left" />
        </div>
        <div class="basis-1/2 p-4 text-center">
            {{ pagination.pageNumber }} <small>/ {{ pagination.pageCount }}</small>
        </div>
        <div @click="onIncrement(1)" class="grow cursor-pointer p-4 text-center hover:bg-background-dark">
            <IconComponent icon="arrow-right" />
        </div>
    </div>
</template>

<script setup lang="ts">
import { type IPagination } from '@/models/Pagination.model';
import { type IPaginationEvent } from '@/components/paginated-content/PaginationEvent';

const props = defineProps<{
    pagination: IPagination;
}>();

const emit = defineEmits(['update']);

const onIncrement = function (increment: number): void {
    const newPageNumber = props.pagination.pageNumber + increment;

    if (newPageNumber < 1 || newPageNumber > props.pagination.pageCount)
        return;

    const event: IPaginationEvent = {
        pageNumber: newPageNumber,
        pageSize: props.pagination.pageSize,
    };

    emit('update', event);
};
</script>