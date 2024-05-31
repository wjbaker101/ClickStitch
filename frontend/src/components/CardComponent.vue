<template>
    <div
        class="rounded-md border-0 border-solid shadow-xl outline-dashed outline-2 outline-offset-1 outline-transparent card-component bg-background border-primary py-8 px-4
        [&.border-top]:border-t-2 [&.border-top]:rounded-t-none
        [&.border-bottom]:border-b-2 [&.border-bottom]:rounded-b-none
        [&.border-left]:border-l-2 [&.border-left]:rounded-l-none
        [&.border-right]:border-r-2 [&.border-right]:rounded-r-none"
        :class="{
            [`border-${border}`]: border,
            'is-padded': padded,
        }"
    >
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    border?: 'top' | 'right' | 'bottom' | 'left';
    padded?: boolean;
}>();
</script>

<style lang="scss">
.card-component {

    &.is-padded {

        & > *:first-child {
            margin-top: 0;
        }

        & > *:last-child {
            margin-bottom: 0;
        }
    }
}
</style>