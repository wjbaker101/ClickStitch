<template>
    <img width="544" height="363" class="pattern-image-component" :src="image ?? undefined" loading="lazy">
</template>

<script setup lang="ts">
defineProps<{
    image: string | null;
}>();
</script>

<style lang="scss">
@use '@/style/variables' as *;

.pattern-image-component {
    max-width: 100%;
    height: auto;
    border-radius: var(--wjb-border-radius);
    vertical-align: middle;
    animation: pattern-image-component-load 0.5s;

    @include shadow-small();

    @keyframes pattern-image-component-load {
        from {
            opacity: 0;
        }
        to {
            opacity: 1;
        }
    }
}
</style>