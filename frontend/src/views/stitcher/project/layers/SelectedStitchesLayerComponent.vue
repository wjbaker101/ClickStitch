<template>
    <div class="layer selected-stitches-layer-component">
        <div
            class="absolute origin-top-left rounded-md shadow-md selected-stitches bg-[rgba(33,33,200,.2)]"
            :style="{
                'width': `${stitchSize * lengthX}px`,
                'height': `${stitchSize * lengthY}px`,
                'transform': `translate(${stitchSelectStart.x * stitchSize}px, ${stitchSelectStart.y * stitchSize}px)`,
            }"
        >
            <div class="absolute -top-10 left-1/2 grid -translate-x-1/2 place-content-center rounded-full text-center shadow-md size-8 bg-background">{{ lengthX }}</div>
            <div class="absolute top-1/2 -left-10 grid -translate-y-1/2 place-content-center rounded-full text-center shadow-md size-8 bg-background">{{ lengthY }}</div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

import { Position } from '@/class/Position.class';

const props = defineProps<{
    stitchSelectStart: Position;
    stitchSelectEnd: Position;
    stitchSize: number;
}>();

const lengthX = computed<number>(() => props.stitchSelectEnd.x - props.stitchSelectStart.x + 1);
const lengthY = computed<number>(() => props.stitchSelectEnd.y - props.stitchSelectStart.y + 1);
</script>