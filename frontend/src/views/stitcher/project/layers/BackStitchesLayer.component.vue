<template>
    <canvas
        class="back-stitches-layer-component"
        ref="canvas"
        :width="project.project.pattern.width * baseStitchSize"
        :height="project.project.pattern.height * baseStitchSize"
    >
    </canvas>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import { useCurrentProject } from '@/views/stitcher/project/use/CurrentProject.use';
import { useCanvasElement } from '@/views/stitcher/project/use/CanvasElement.use';

const props = defineProps<{
    baseStitchSize: number;
}>();

const { project } = useCurrentProject();

const canvas = ref<HTMLCanvasElement>({} as HTMLCanvasElement);
const { graphics } = useCanvasElement(canvas);

const render = function () {
    const threads = project.value.threads;

    graphics.value.lineWidth = props.baseStitchSize / 2;
    graphics.value.lineCap = 'round';
    graphics.value.shadowBlur = 3;

    for (const thread of threads) {
        graphics.value.strokeStyle = thread.thread.colour;
        graphics.value.shadowColor = thread.thread.colour;

        for (const backStitch of thread.backStitches) {
            graphics.value.beginPath();
            graphics.value.moveTo(backStitch[0] * props.baseStitchSize, backStitch[1] * props.baseStitchSize);
            graphics.value.lineTo(backStitch[2] * props.baseStitchSize, backStitch[3] * props.baseStitchSize);
            graphics.value.stroke();
        }
    }

};

onMounted(() => {
    render();
});
</script>

<style lang="scss">
</style>