<template>
    <div class="card-component" :class="{ [`border-${border}`]: true }">
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    border?: 'top' | 'right' | 'bottom' | 'left';
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

    @include shadow-small();

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