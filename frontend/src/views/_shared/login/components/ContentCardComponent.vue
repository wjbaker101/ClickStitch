<template>
    <CardComponent padded class="relative content-card-component text-light">
        <img width="90" height="90" src="@/assets/logo-text.jpg" class="absolute top-0 left-1/2 logo p-2 bg-[#efefef] rounded-full shadow-md border-2 border-solid border-primary -translate-x-1/2 -translate-y-1/2">
        <slot></slot>
    </CardComponent>
</template>

<script setup lang="ts">
</script>

<style lang="scss">
.content-card-component {
    background-color: var(--wjb-primary);
    background: linear-gradient(
        -25deg,
        var(--wjb-primary-dark),
        var(--wjb-primary),
    );
}
</style>