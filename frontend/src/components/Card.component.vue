<template>
    <div
        class="card-component"
        :class="{
            [`border-${border}`]: true,
            'is-hoverable': hoverable
        }"
    >
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    border?: 'top' | 'right' | 'bottom' | 'left';
    hoverable?: boolean;
}>();
</script>

<style lang="scss">
@use '@/style/variables' as *;

.card-component {
    background-color: var(--wjb-background-colour-dark);
    border-radius: var(--wjb-border-radius);
    border-width: 0;
    border-style: solid;
    border-color: var(--wjb-primary);
    outline: 2px dashed transparent;
    outline-offset: 1px;

    @include shadow-small();

    &.is-hoverable {
        cursor: pointer;

        &:hover {
            outline: 2px dashed var(--wjb-tertiary);
        }
    }

    &.border-top {
        border-top-width: 4px;
    }

    &.border-right {
        border-right-width: 4px;
    }

    &.border-bottom {
        border-bottom-width: 4px;
    }

    &.border-left {
        border-left-width: 4px;
    }
}
</style>