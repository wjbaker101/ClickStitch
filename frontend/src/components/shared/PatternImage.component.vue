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
    max-height: 350px;
    width: auto;
    border-radius: var(--wjb-border-radius);
    box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);
    vertical-align: middle;
    animation: pattern-image-component-load 0.5s;
    image-rendering: pixelated;
    object-fit: contain;

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