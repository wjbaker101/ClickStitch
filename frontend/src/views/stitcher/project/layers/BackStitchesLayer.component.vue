<template>
    <svg
        v-if="isVisible"
        xmlns="http://www.w3.org/2000/svg"
        class="back-stitches-layer-component"
        :width="project.project.pattern.width * baseStitchSize"
        :height="project.project.pattern.height * baseStitchSize"
    >
        <line
            :style="{
                '--back-stitch-colour': backStitch.colour,
                '--back-stitch-width': baseStitchSize / 2,
            }"
            class="back-stitch-line"
            :class="{
                'is-completed': backStitch.isCompleted,
            }"
            v-for="backStitch in backStitches"
            :x1="backStitch.startX * baseStitchSize" :y1="backStitch.startY * baseStitchSize"
            :x2="backStitch.endX * baseStitchSize" :y2="backStitch.endY * baseStitchSize"
            @dblclick="toggleCompleted(backStitch)"
        />
    </svg>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import { useCurrentProject } from '@/views/stitcher/project/use/CurrentProject.use';
import { useLayers } from '@/views/stitcher/project/use/Layers.use';
import { api } from '@/api/api';

const props = defineProps<{
    baseStitchSize: number;
}>();

const { project } = useCurrentProject();
const layers = useLayers();

const isVisible = layers.backStitches;

interface IBackStitch {
    readonly threadIndex: number;
    readonly colour: string;
    readonly startX: number;
    readonly startY: number;
    readonly endX: number;
    readonly endY: number;
    isCompleted: boolean;
}

const inCompleted = project.value.threads.flatMap<IBackStitch>(thread => thread.backStitches.map(x => ({
    threadIndex: thread.thread.index,
    colour: thread.thread.colour,
    startX: x[0],
    startY: x[1],
    endX: x[2],
    endY: x[3],
    isCompleted: false,
})));

const completed = project.value.threads.flatMap<IBackStitch>(thread => thread.completedBackStitches.map(x => ({
    threadIndex: thread.thread.index,
    colour: thread.thread.colour,
    startX: x[0],
    startY: x[1],
    endX: x[2],
    endY: x[3],
    isCompleted: true,
})));

const backStitches = ref<Array<IBackStitch>>(inCompleted.concat(completed));

const toggleCompleted = async function (backStitch: IBackStitch): Promise<void> {
    backStitch.isCompleted = !backStitch.isCompleted;

    if (backStitch.isCompleted) {
        await api.projects.completeBackStitches(project.value.project.pattern.reference, {
            backStitchesByThread: {
                [backStitch.threadIndex]: [
                    {
                        startX: backStitch.startX,
                        startY: backStitch.startY,
                        endX: backStitch.endX,
                        endY: backStitch.endY,
                    },
                ],
            },
        });
    }
    else {
        await api.projects.unCompleteBackStitches(project.value.project.pattern.reference, {
            backStitchesByThread: {
                [backStitch.threadIndex]: [
                    {
                        startX: backStitch.startX,
                        startY: backStitch.startY,
                        endX: backStitch.endX,
                        endY: backStitch.endY,
                    },
                ],
            },
        });
    }
};
</script>

<style lang="scss">
.back-stitches-layer-component {
    position: relative;
    z-index: 1;

    .back-stitch-line {
        stroke: var(--back-stitch-colour);
        stroke-width: var(--back-stitch-width);
        stroke-linecap: round;

        &.is-completed {
            stroke: #0f0;
        }
    }
}
</style>