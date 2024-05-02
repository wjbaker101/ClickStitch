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

import { useCurrentProject } from '@/views/stitcher/project/use/CurrentProject.use';
import { useStitch } from '@/views/stitcher/project/use/Stitch.use';
import { useCanvasElement } from '@/views/stitcher/project/use/CanvasElement.use';

const { project } = useCurrentProject();
const { baseStitchSize } = useStitch();

const canvas = ref<HTMLCanvasElement>({} as HTMLCanvasElement);
const { graphics } = useCanvasElement(canvas);

const render = function (): void {
    graphics.value.strokeStyle = '#666';
    graphics.value.lineWidth = 2;

    for (let x = 1; x < project.value.project.pattern.width; ++x) {
        graphics.value.beginPath();
        graphics.value.moveTo(x * baseStitchSize, 0);
        graphics.value.lineTo(x * baseStitchSize, project.value.project.pattern.height * baseStitchSize);
        graphics.value.closePath();
        graphics.value.stroke();
    }

    for (let y = 1; y < project.value.project.pattern.height; ++y) {
        graphics.value.beginPath();
        graphics.value.moveTo(0, y * baseStitchSize);
        graphics.value.lineTo(project.value.project.pattern.width * baseStitchSize, y * baseStitchSize);
        graphics.value.closePath();
        graphics.value.stroke();
    }

    graphics.value.lineWidth = 6;
    graphics.value.strokeStyle = '#f00';

    graphics.value.beginPath();
    graphics.value.moveTo(0, project.value.project.pattern.width / 2 * baseStitchSize);
    graphics.value.lineTo(project.value.project.pattern.width * baseStitchSize, project.value.project.pattern.width / 2 * baseStitchSize);
    graphics.value.closePath();
    graphics.value.stroke();

    graphics.value.beginPath();
    graphics.value.moveTo(project.value.project.pattern.height / 2 * baseStitchSize, 0);
    graphics.value.lineTo(project.value.project.pattern.height / 2 * baseStitchSize, project.value.project.pattern.height * baseStitchSize);
    graphics.value.closePath();
    graphics.value.stroke();
};

onMounted(() => {
    render();
});
</script>

<style lang="scss">
</style>