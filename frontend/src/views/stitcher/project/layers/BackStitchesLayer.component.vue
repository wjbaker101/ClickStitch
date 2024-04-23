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

    for (const thread of threads) {
        console.log(thread.backStitches)
    }
};

onMounted(() => {
    render();
});
</script>

<style lang="scss">
</style>