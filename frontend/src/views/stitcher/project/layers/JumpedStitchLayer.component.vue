<template>
    <canvas
        class="jumped-stitch-layer-component"
        ref="canvas"
        :width="project.project.pattern.width * baseStitchSize"
        :height="project.project.pattern.height * baseStitchSize"
    >
    </canvas>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import { useCanvasElement } from '@/views/stitcher/project/use/CanvasElement.use';
import { useTransformation } from '@/views/stitcher/project/use/Transformation.use';
import { useCurrentProject } from '@/views/stitcher/project/use/CurrentProject.use';
import { useEvent } from '@/use/events/Events.use';

import { Position } from '@/class/Position.class';
import type { IJumpToStitchEvent } from '@/use/events/types/EventsMap.type';

const props = defineProps<{
    baseStitchSize: number;
}>();

const { width, height, offset, scale } = useTransformation();
const { project, setJumpedBackStitch } = useCurrentProject();

const canvas = ref<HTMLCanvasElement>({} as HTMLCanvasElement);
const { graphics } = useCanvasElement(canvas);

const prevJumpedStitch = ref<Position>(Position.ZERO);

const onJumpToStitch = function (event: IJumpToStitchEvent): void {
    scale.value = 1;
    const scaledStitchSize = scale.value * props.baseStitchSize;

    offset.value = Position
        .at(-event.x * scaledStitchSize, -event.y * scaledStitchSize)
        .translate(width.value / 2, height.value / 2)
        .translate(-scaledStitchSize / 2, -scaledStitchSize / 2);

    setJumpedBackStitch(null);

    if (event.type === 'back-stitch') {
        setJumpedBackStitch({
            startX: event.x,
            startY: event.y,
            endX: event.endX as number,
            endY: event.endY as number,
        });

        return;
    }

    const borderWidth = 6;

    graphics.value.clearRect(
        prevJumpedStitch.value.x * props.baseStitchSize - Math.ceil(borderWidth / 2),
        prevJumpedStitch.value.y * props.baseStitchSize - Math.ceil(borderWidth / 2),
        props.baseStitchSize + borderWidth + 1,
        props.baseStitchSize + borderWidth + 1);

    graphics.value.strokeStyle = '#ffb400';
    graphics.value.lineWidth = borderWidth;
    graphics.value.strokeRect(event.x * props.baseStitchSize, event.y * props.baseStitchSize, props.baseStitchSize, props.baseStitchSize);
    graphics.value.stroke();

    prevJumpedStitch.value = Position.at(event.x, event.y);
};

const onEndJumpToStitches = function (): void {
    const borderWidth = 6;

    graphics.value.clearRect(
        prevJumpedStitch.value.x * props.baseStitchSize - Math.ceil(borderWidth / 2),
        prevJumpedStitch.value.y * props.baseStitchSize - Math.ceil(borderWidth / 2),
        props.baseStitchSize + borderWidth + 1,
        props.baseStitchSize + borderWidth + 1);

    setJumpedBackStitch(null);
};

useEvent('JumpToStitch', onJumpToStitch);
useEvent('EndJumpToStitches', onEndJumpToStitches);
</script>

<style lang="scss">
</style>