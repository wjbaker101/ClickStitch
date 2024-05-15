<template>
    <canvas
        class="completed-stitches-layer-component"
        :class="{
            'is-hidden': !isVisible,
        }"
        ref="canvas"
        :width="project.project.pattern.width * baseStitchSize"
        :height="project.project.pattern.height * baseStitchSize"
    >
    </canvas>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import dayjs from 'dayjs';

import { api } from '@/api/api';
import { useCanvasElement } from '../use/CanvasElement.use';
import { useCurrentProject } from '../use/CurrentProject.use';
import { useEvent } from '@/use/events/Events.use';
import { useSharedStitch } from '../use/SharedStitch';
import { useStitch } from '../use/Stitch.use';
import { useInput } from '@/use/input/input.use';
import { useHighlightedThread } from '../use/HighlightedThread.use';
import { useLayers } from '@/views/stitcher/project/use/Layers.use';

import type { IStitch } from '@/models/Pattern.model';
import type { IPosition } from '@/api/parts/projects/types/CompleteStitches.type';

const props = defineProps<{
    baseStitchSize: number;
}>();

const { project, stitches, stitchPositionLookup } = useCurrentProject();
const sharedStitch = useSharedStitch();
const { baseStitchSize, mouseStitchPosition, stitchSelectStart, stitchSelectEnd } = useStitch();
const highlightedThread = useHighlightedThread();
const layers = useLayers();

const canvas = ref<HTMLCanvasElement>({} as HTMLCanvasElement);
const { graphics } = useCanvasElement(canvas);

const isVisible = layers.stitches;
const hoveredStitch = sharedStitch.hoveredStitch;
const highlightedThreadIndex = highlightedThread.threadIndex;

const onPatternDoubleClick = async function () {
    if (hoveredStitch.value === null)
        return;

    const stitch = hoveredStitch.value;

    if (stitch.stitchedAt === null) {
        graphics.value.fillRect(
            mouseStitchPosition.value.x * baseStitchSize,
            mouseStitchPosition.value.y * baseStitchSize,
            baseStitchSize,
            baseStitchSize);

        stitch.stitchedAt = dayjs();

        await api.projects.completeStitches(project.value.project.pattern.reference, {
            stitchesByThread: {
                [stitch.threadIndex]: [
                    {
                        x: mouseStitchPosition.value.x,
                        y: mouseStitchPosition.value.y,
                    },
                ],
            },
        });

        const thread = project.value.threads.find(x => x.thread.index === stitch.threadIndex);
        if (thread === undefined)
            return;

        stitch.stitchedAt = dayjs();
    }
    else {
        graphics.value.clearRect(
            mouseStitchPosition.value.x * baseStitchSize,
            mouseStitchPosition.value.y * baseStitchSize,
            baseStitchSize,
            baseStitchSize);

        stitch.stitchedAt = null;

        await api.projects.unCompleteStitches(project.value.project.pattern.reference, {
            stitchesByThread: {
                [stitch.threadIndex]: [
                    {
                        x: mouseStitchPosition.value.x,
                        y: mouseStitchPosition.value.y,
                    },
                ],
            },
        });

        const thread = project.value.threads.find(x => x.thread.index === stitch.threadIndex);
        if (thread === undefined)
            return;

        stitch.stitchedAt = null;
    }
};

const render = function () {
    graphics.value.reset();
    graphics.value.fillStyle = '#0f0';

    const completedStitches = stitches.value
        .filter(x => highlightedThreadIndex.value === null || x.threadIndex === highlightedThreadIndex.value)
        .filter(x => x.stitchedAt !== null);

    for (let index = 0; index < completedStitches.length; ++index) {
        const stitch = completedStitches[index];

        graphics.value.fillRect(stitch.x * props.baseStitchSize, stitch.y * props.baseStitchSize, props.baseStitchSize, props.baseStitchSize);
    }
};

onMounted(() => {
    render();
});

useInput('keypress', async (event) => {
    if (event.key !== ' ')
        return;

    if (stitchSelectStart.value === null || stitchSelectEnd.value === null)
        return;

    const stitches: Array<IStitch> = [];

    const width = stitchSelectEnd.value.x - stitchSelectStart.value.x + 1;
    const height = stitchSelectEnd.value.y - stitchSelectStart.value.y + 1;

    for (let x = 0; x < width; ++x) {
        for (let y = 0; y < height; ++y) {
            const position = stitchSelectStart.value.translate(x, y);

            const stitch = stitchPositionLookup.value.get(position.x, position.y);
            if (!stitch)
                continue;

            if (event.shiftKey && stitch.stitchedAt === null)
                continue;
            if (!event.shiftKey && stitch.stitchedAt !== null)
                continue;

            stitches.push(stitch);
        }
    }

    if (stitches.length > 100)
        return;

    const positions: Record<number, Array<IPosition>> = {};

    for (const stitch of stitches) {
        if (positions[stitch.threadIndex] === undefined)
            positions[stitch.threadIndex] = [];

        positions[stitch.threadIndex].push({
            x: stitch.x,
            y: stitch.y,
        });
    }

    if (event.shiftKey) {
        for (const stitch of stitches) {
            graphics.value.clearRect(
                stitch.x * baseStitchSize,
                stitch.y * baseStitchSize,
                baseStitchSize,
                baseStitchSize);

            stitch.stitchedAt = null;
        }

        await api.projects.unCompleteStitches(project.value.project.pattern.reference, {
            stitchesByThread: positions,
        });
    }
    else {
        for (const stitch of stitches) {
            graphics.value.fillRect(
                stitch.x * baseStitchSize,
                stitch.y * baseStitchSize,
                baseStitchSize,
                baseStitchSize);

            stitch.stitchedAt = dayjs();
        }

        await api.projects.completeStitches(project.value.project.pattern.reference, {
            stitchesByThread: positions,
        });
    }
});

useEvent('PatternDoubleClick', onPatternDoubleClick);
useEvent('HighlightThread', () => render());
</script>

<style lang="scss">
</style>