<template>
    <svg
        xmlns="http://www.w3.org/2000/svg"
        class="back-stitches-layer-component"
        :class="{
            'is-hidden': !isVisible,
        }"
        :width="project.project.pattern.width * baseStitchSize"
        :height="project.project.pattern.height * baseStitchSize"
    >
        <line
            class="jumped-back-stitch"
            v-if="jumpedBackStitch !== null"
            :style="{
                '--back-stitch-width': baseStitchSize / 2,
            }"
            :x1="jumpedBackStitch.startX * baseStitchSize"
            :y1="jumpedBackStitch.startY * baseStitchSize"
            :x2="jumpedBackStitch.endX * baseStitchSize"
            :y2="jumpedBackStitch.endY * baseStitchSize"
        />
        <line
            ref="lines"
            :style="{
                '--back-stitch-colour': backStitch.colour,
                '--back-stitch-width': baseStitchSize / 2,
                '--border-colour': isDark(backStitch.colour) ? '#ccc' : '#222',
            }"
            class="back-stitch-line"
            :class="{
                'is-completed': backStitch.isCompleted,
            }"
            v-for="(backStitch, index) in backStitches"
            :x1="backStitch.startX * baseStitchSize" :y1="backStitch.startY * baseStitchSize"
            :x2="backStitch.endX * baseStitchSize" :y2="backStitch.endY * baseStitchSize"
            :data-index="index"
            @mousemove.stop="onMouseMove(backStitch)"
        />
    </svg>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import { type IBackStitch, useCurrentProject } from '@/views/stitcher/project/use/CurrentProject.use';
import { useLayers } from '@/views/stitcher/project/use/Layers.use';
import { useHammer } from '@/views/stitcher/project/use/Hammer.use';
import { api } from '@/api/api';
import { isDark } from '@/helper/helper';

defineProps<{
    baseStitchSize: number;
}>();

const { project, backStitches, setActiveStitch, jumpedBackStitch } = useCurrentProject();
const layers = useLayers();

const lines = ref<Array<HTMLElement>>([]);

const isVisible = layers.backStitches;

const onMouseMove = function (backStitch: IBackStitch): void {
    setActiveStitch({
        x: backStitch.startX,
        y: backStitch.startY,
        endX: backStitch.endX,
        endY: backStitch.endY,
        threadIndex: backStitch.threadIndex,
    });
};

onMounted(() => {
    for (const line of lines.value) {
        const hammer = useHammer(ref(line));

        hammer.on('double-tap', async e => {
            const backStitch = backStitches.value[e.target.getAttribute('data-index')];

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
        });
    }
});
</script>

<style lang="scss">
.back-stitches-layer-component {
    position: relative;
    z-index: 1;

    .jumped-back-stitch {
        stroke: #ffb400;
        stroke-width: 39;
        stroke-linecap: round;
    }

    .back-stitch-line {
        position: relative;
        stroke: var(--back-stitch-colour);
        stroke-width: var(--back-stitch-width);
        stroke-linecap: round;
        filter: drop-shadow(0 0 3px var(--border-colour));

        &.is-completed {
            stroke: #0f0;
        }

        &.is-jumped {
            z-index: 2;
            filter: drop-shadow(0 0 3px var(--border-colour)) drop-shadow(0 0 6px #ffb400);
        }
    }
}
</style>