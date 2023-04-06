<template>
    <img width="150" height="150" class="pattern-image-component" :src="pattern.thumbnailUrl" loading="lazy">
</template>

<script setup lang="ts">
import { IPattern } from '@/models/Pattern.model';

defineProps<{
    pattern: IPattern;
}>();
</script>

<style lang="scss">
@use '@/style/variables' as *;

.pattern-image-component {
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