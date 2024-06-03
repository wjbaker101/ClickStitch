<template>
    <div class="mx-auto grid gap-12 pt-8">
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
</script>

<style lang="scss" scoped>
* {
    counter-reset: index;
    grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
}
</style>