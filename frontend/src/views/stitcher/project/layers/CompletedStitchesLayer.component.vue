<template>
    <canvas
        class="completed-stitches-layer-component"
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
import { useEvent, useEvents } from '@/use/events/Events.use';
import { useSharedStitch } from '../use/SharedStitch';
import { useStitch } from '../use/Stitch.use';
import { useInput } from '@/use/input/input.use';

import type { IStitch } from '@/models/Pattern.model';
import type { IPosition } from '@/api/types/CompleteStitches.type';

const props = defineProps<{
    baseStitchSize: number;
}>();

const { project, stitchPositionLookup } = useCurrentProject();
const events = useEvents();
const sharedStitch = useSharedStitch();
const { baseStitchSize, mouseStitchPosition, stitchSelectStart, stitchSelectEnd } = useStitch();

const canvas = ref<HTMLCanvasElement>({} as HTMLCanvasElement);
const { graphics } = useCanvasElement(canvas);

const hoveredStitch = sharedStitch.hoveredStitch;

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

        thread.completedStitches.push([stitch.x, stitch.y, dayjs()]);

        const stitchIndex = thread.stitches.findIndex(x => x[0] === stitch.x && x[1] === stitch.y);
        thread.stitches.splice(stitchIndex, 1);
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

        thread.stitches.push([stitch.x, stitch.y]);

        const completedStitchIndex = thread.completedStitches.findIndex(x => x[0] === stitch.x && x[1] === stitch.y);
        thread.completedStitches.splice(completedStitchIndex, 1);
    }
};

onMounted(() => {
    graphics.value.fillStyle = '#0f0';

    const completedStitches = project.value.threads.flatMap(x => x.completedStitches);

    for (let index = 0; index < completedStitches.length; ++index) {
        const stitch = completedStitches[index];

        graphics.value.fillRect(stitch[0] * props.baseStitchSize, stitch[1] * props.baseStitchSize, props.baseStitchSize, props.baseStitchSize);
    }
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

            const stitch = stitchPositionLookup.value.get(`${position.x}:${position.y}`);
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

        for (const stitch of stitches) {
            const thread = project.value.threads.find(x => x.thread.index === stitch.threadIndex);
            if (thread === undefined)
                continue;

            thread.stitches.push([stitch.x, stitch.y]);

            const completedStitchIndex = thread.completedStitches.findIndex(x => x[0] === stitch.x && x[1] === stitch.y);
            thread.completedStitches.splice(completedStitchIndex, 1);
        }
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

        for (const stitch of stitches) {
            const thread = project.value.threads.find(x => x.thread.index === stitch.threadIndex);
            if (thread === undefined)
                return;

            thread.completedStitches.push([stitch.x, stitch.y, dayjs()]);

            const stitchIndex = thread.stitches.findIndex(x => x[0] === stitch.x && x[1] === stitch.y);
            thread.stitches.splice(stitchIndex, 1);
        }
    }
});

useEvent('PatternDoubleClick', onPatternDoubleClick);
</script>

<style lang="scss">
</style>