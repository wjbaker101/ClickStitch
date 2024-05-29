<template>
    <CardComponent padded border="top" class="relative numbered-card-component">
        <div class="absolute top-0 left-1/2 -translate-x-1/2 -translate-y-1/2 rounded-full border-2 border-solid p-4 text-center font-bold number size-16 bg-background text-3xl/none text-secondary border-primary"></div>
        <slot></slot>
    </CardComponent>
</template>

<script setup lang="ts">
</script>

<style lang="scss">
.numbered-card-component {
    .number {
        &::before {
            counter-increment: index;
            content: counter(index);
        }
    }
}
</style>