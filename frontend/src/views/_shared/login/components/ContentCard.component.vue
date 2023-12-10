<template>
    <CardComponent class="content-card-component" padded border="top">
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
    color: var(--wjb-light);
    border-color: var(--wjb-tertiary);
}
</style>