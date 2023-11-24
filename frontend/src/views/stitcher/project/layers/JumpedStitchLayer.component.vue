<template>
    <canvas
        ref="canvas"
        :width="project.project.pattern.width * baseStitchSize"
        :height="project.project.pattern.height * baseStitchSize"
    >
    </canvas>
</template>

<script setup lang="ts">
import { onMounted, onUnmounted, ref } from 'vue';

import { useCanvasElement } from '@/views/stitcher/project/use/CanvasElement.use';
import { useTransformation } from '@/views/stitcher/project/use/Transformation.use';
import { useCurrentProject } from '@/views/stitcher/project/use/CurrentProject.use';
import { useEvents } from '@/use/events/Events.use';

import { Position } from '@/class/Position.class';
import type { IJumpToStitchEvent } from '@/use/events/types/EventsMap.type';

const props = defineProps<{
    baseStitchSize: number;
}>();

const events = useEvents();
const { width, height, offset, scale } = useTransformation();
const { project } = useCurrentProject();

const canvas = ref<HTMLCanvasElement>({} as HTMLCanvasElement);
const { graphics } = useCanvasElement(canvas);

const prevJumpedStitch = ref<Position>(Position.ZERO);

const onJumpToStitch = function (event: IJumpToStitchEvent): void {
    const borderWidth = 6;
    scale.value = 1;

    const scaledStitchSize = scale.value * props.baseStitchSize;

    graphics.value.clearRect(
        prevJumpedStitch.value.x * props.baseStitchSize - Math.ceil(borderWidth / 2),
        prevJumpedStitch.value.y * props.baseStitchSize - Math.ceil(borderWidth / 2),
        props.baseStitchSize + borderWidth + 1,
        props.baseStitchSize + borderWidth + 1);

    offset.value = Position
        .at(-event.x * scaledStitchSize, -event.y * scaledStitchSize)
        .translate(width.value / 2, height.value / 2)
        .translate(-scaledStitchSize / 2, -scaledStitchSize / 2);

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
};

onMounted(() => {
    events.subscribe('JumpToStitch', onJumpToStitch);
    events.subscribe('EndJumpToStitches', onEndJumpToStitches);
});

onUnmounted(() => {
    events.unsubscribe('JumpToStitch', onJumpToStitch);
    events.unsubscribe('EndJumpToStitches', onEndJumpToStitches);
});
</script>

<style lang="scss">
</style>