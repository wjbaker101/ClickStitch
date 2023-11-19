<template>
    <canvas
        ref="jumpedStitchCanvas"
        :width="project.pattern.width * baseStitchSize"
        :height="project.pattern.height * baseStitchSize"
    >
    </canvas>
</template>

<script setup lang="ts">
import { onMounted, onUnmounted, ref } from 'vue';

import { useCanvasElement } from '@/views/stitcher/project/use/CanvasElement.use';
import { useTransformation } from '@/views/stitcher/project/use/Transformation.use';
import { useEvents } from '@/use/events/Events.use';

import { Position } from '@/class/Position.class';
import type { IProject } from '@/models/Project.model';
import type { IJumpToStitchEvent } from '@/use/events/types/EventsMap.type';

const props = defineProps<{
    project: IProject;
    baseStitchSize: number;
}>();

const events = useEvents();
const { width, height, offset, scale } = useTransformation();

const jumpedStitchCanvas = ref<HTMLCanvasElement>({} as HTMLCanvasElement);
const { graphics: jumpedStitchGraphics } = useCanvasElement(jumpedStitchCanvas);

const prevJumpedStitch = ref<Position>(Position.ZERO);

const onJumpToStitch = function (event: IJumpToStitchEvent): void {
    const borderWidth = 6;
    scale.value = 1;

    const scaledStitchSize = scale.value * props.baseStitchSize;

    jumpedStitchGraphics.value.clearRect(
        prevJumpedStitch.value.x * props.baseStitchSize - Math.ceil(borderWidth / 2),
        prevJumpedStitch.value.y * props.baseStitchSize - Math.ceil(borderWidth / 2),
        props.baseStitchSize + borderWidth + 1,
        props.baseStitchSize + borderWidth + 1);

    offset.value = Position
        .at(-event.x * scaledStitchSize, -event.y * scaledStitchSize)
        .translate(width.value / 2, height.value / 2)
        .translate(-scaledStitchSize / 2, -scaledStitchSize / 2);

    jumpedStitchGraphics.value.strokeStyle = '#ffb400';
    jumpedStitchGraphics.value.lineWidth = borderWidth;
    jumpedStitchGraphics.value.strokeRect(event.x * props.baseStitchSize, event.y * props.baseStitchSize, props.baseStitchSize, props.baseStitchSize);
    jumpedStitchGraphics.value.stroke();

    prevJumpedStitch.value = Position.at(event.x, event.y);
};

const onEndJumpToStitches = function (): void {
    const borderWidth = 6;

    jumpedStitchGraphics.value.clearRect(
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