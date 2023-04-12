<template>
    <div class="pattern-images-modal-component text-centered">
        <img v-if="pattern.bannerImageUrl" :src="pattern.bannerImageUrl">
    </div>
</template>

<script setup lang="ts">
import { IPattern } from '@/models/Pattern.model';

defineProps<{
    pattern: IPattern;
}>();
</script>

<style lang="scss">
.pattern-images-modal-component {
    max-width: 100%;

    img {
        max-width: 100%;
        height: auto;
        border-radius: var(--wjb-border-radius);
        vertical-align: middle;
    }
}
</style>