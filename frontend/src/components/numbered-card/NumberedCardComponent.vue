<template>
    <CardComponent padded border="top" class="relative">
        <div class="absolute top-0 left-1/2 grid -translate-x-1/2 -translate-y-1/2 place-items-center rounded-full border-2 border-solid p-4 font-bold size-16 bg-background text-3xl/none text-secondary border-primary"></div>
        <slot></slot>
    </CardComponent>
</template>

<script setup lang="ts">
</script>

<style lang="scss" scoped>
* {
    & > :first-child::before {
        counter-increment: index;
        content: counter(index);
    }
}
</style>