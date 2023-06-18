<template>
    <div class="pagination-component flex align-items-center gap">
        <div class="backwards text-centered" @click="onIncrement(-1)">
            <IconComponent icon="arrow-left" />
        </div>
        <div class="flex-2 text-centered">
            {{ pagination.pageNumber }} <small>/ {{ pagination.pageCount }}</small>
        </div>
        <div class="forwards text-centered" @click="onIncrement(1)">
            <IconComponent icon="arrow-right" />
        </div>
    </div>
</template>

<script setup lang="ts">
import { IPagination } from '@/models/Pagination.model';
import { IPaginationEvent } from '@/components/paginated-content/PaginationEvent';

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

<style lang="scss">
.pagination-component {
    margin: 1rem 0;
    background-color: var(--wjb-background-colour);
    border-radius: var(--wjb-border-radius);
    box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);
    overflow: hidden;

    & > * {
        padding: 1rem;
    }
}

.backwards,
.forwards {
    cursor: pointer;

    &:hover {
        background-color: var(--wjb-background-colour-dark);
    }
}
</style>