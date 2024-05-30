<template>
    <div v-if="pausePosition !== null" class="layer pause-position-layer-component">
        <div
            class="rounded-full border-4 border-solid border-[#ffb400] bg-transparent shadow-lg outline-4 outline-black outline-offset-0 outline"
            :style="{
                'width': `${stitchSize}px`,
                'height': `${stitchSize}px`,
                'transform': `translate(${pausePosition.x * stitchSize}px, ${pausePosition.y * stitchSize}px)`,
            }"
        ></div>
    </div>
</template>

<script setup lang="ts">
import { useCurrentProject } from '../use/CurrentProject.use';
import { useTransformation } from '../use/Transformation.use';
import { useStitch } from '../use/Stitch.use';

import { Position } from '@/class/Position.class';
import { useEvent } from '@/use/events/Events.use';

defineProps<{
    stitchSize: number;
}>();

const { pausePosition } = useCurrentProject();
const { width, height, offset, scale } = useTransformation();
const { scaledStitchSize } = useStitch();

useEvent('GoToPausePosition', () => {
    if (pausePosition.value === null)
        return;
    scale.value = 0.5;

    offset.value = Position.at(width.value / 2 - pausePosition.value.x * scaledStitchSize.value, height.value / 2 - pausePosition.value.y * scaledStitchSize.value);
});
</script>

<style lang="scss">
</style>