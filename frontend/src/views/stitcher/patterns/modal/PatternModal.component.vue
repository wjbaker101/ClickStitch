<template>
    <div class="pattern-modal-component">
        <h2>{{ pattern.title }}</h2>
        <div class="flex gap align-items-center">
            <div class="flex-auto">
                <img :src="pattern.thumbnailUrl">
            </div>
            <div>
                <strong>Size:</strong> {{ pattern.width }}&times;{{ pattern.height }}
                <br>
                <strong>Threads:</strong> {{ pattern.threadCount }}
                <br>
                <strong>Stitches:</strong> {{ formatNumber(pattern.stitchCount) }}
            </div>
            <div>
                <p><strong>{{ currency(pattern.price) }}</strong></p>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { currency, formatNumber } from '@/helper/helper';

import { type IPattern } from '@/models/Pattern.model';

defineProps<{
    pattern: IPattern;
}>();
</script>

<style lang="scss">
@use '@/style/variables' as *;

.pattern-modal-component {
    width: 720px;
    max-width: 100%;

    img {
        border-radius: var(--wjb-border-radius);
        vertical-align: middle;

        @include shadow-small();
    }
}
</style>