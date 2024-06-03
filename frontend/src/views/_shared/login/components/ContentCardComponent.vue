<template>
    <CardComponent padded class="relative bg-gradient-to-tl content-card-component text-light from-primary-dark to-primary">
        <img
            width="90"
            height="90"
            src="@/assets/logo-text.jpg"
            class="absolute top-0 left-1/2 logo p-2 bg-[#efefef] rounded-full shadow-md border-2 border-solid border-primary -translate-x-1/2 -translate-y-1/2"
        >
        <slot></slot>
    </CardComponent>
</template>

<script setup lang="ts">
</script>

<style lang="scss">
</style>