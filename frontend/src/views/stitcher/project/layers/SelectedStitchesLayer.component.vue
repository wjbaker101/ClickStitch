<template>
    <div class="layer selected-stitches-layer-component">
        <div
            class="selected-stitches"
            :style="{
                'width': `${stitchSize * (stitchSelectEnd.x - stitchSelectStart.x + 1)}px`,
                'height': `${stitchSize * (stitchSelectEnd.y - stitchSelectStart.y + 1)}px`,
                'transform': `translate(${stitchSelectStart.x * stitchSize}px, ${stitchSelectStart.y * stitchSize}px)`,
            }"
        >
            <div class="top-axis">
                {{ stitchSelectEnd.x - stitchSelectStart.x + 1 }}
            </div>
            <div class="left-axis">
                {{ stitchSelectEnd.y - stitchSelectStart.y + 1 }}
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { Position } from '@/class/Position.class';

defineProps<{
    stitchSelectStart: Position;
    stitchSelectEnd: Position;
    stitchSize: number;
}>();
</script>

<style lang="scss">
@use '@/style/variables' as *;

.selected-stitches-layer-component {

    .selected-stitches {
        position: absolute;
        transform-origin: top left;
        border-radius: 0.2rem;
        box-shadow: 1px 2px 5px rgba(0, 0, 0, 0.5), 1px 2px 10px rgba(0, 0, 0, 0.2);
        background-color: rgba(33, 33, 200, 0.2);
        transition: transform 0.1s;
    }

    .top-axis,
    .left-axis {
        width: 2rem;
        aspect-ratio: 1;
        line-height: 2rem;
        text-align: center;
        background-color: var(--wjb-background-colour);
        border-radius: 50%;

        @include shadow-medium();
    }

    .top-axis {
        position: absolute;
        top: -2.5rem;
        left: 50%;
        transform: translateX(-50%);
    }

    .left-axis {
        position: absolute;
        left: -2.5rem;
        top: 50%;
        transform: translateY(-50%);
    }
}
</style>