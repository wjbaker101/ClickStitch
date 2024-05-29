<template>
    <div class="mx-auto grid grid-flow-col gap-12 pt-8 numbered-section-component">
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
</script>

<style lang="scss">
.numbered-section-component {
    counter-reset: index;
}
</style>