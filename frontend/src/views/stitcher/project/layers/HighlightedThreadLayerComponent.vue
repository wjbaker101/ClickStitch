<template>
    <canvas
        class="highlighted-thread-layer-component"
        ref="canvas"
        :width="project.project.pattern.width * baseStitchSize"
        :height="project.project.pattern.height * baseStitchSize"
    >
    </canvas>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import { useCanvasElement } from '@/views/stitcher/project/use/CanvasElement.use';
import { useCurrentProject } from '@/views/stitcher/project/use/CurrentProject.use';
import { useEvent } from '@/use/events/Events.use';
import { isDark } from '@/helper/helper';

import type { IPatternThread } from '@/models/Pattern.model';
import type { IHighlightThreadEvent } from '@/use/events/types/EventsMap.type';

const props = defineProps<{
    baseStitchSize: number;
}>();

const { project, stitches, palette } = useCurrentProject();

const canvas = ref<HTMLCanvasElement>({} as HTMLCanvasElement);
const { graphics } = useCanvasElement(canvas);

const onHighlightThread = function (event: IHighlightThreadEvent): void {
    graphics.value.reset();

    graphics.value.textAlign = 'center';
    graphics.value.font = 'normal 28px sans-serif';

    for (let index = 0; index < stitches.value.length; ++index) {
        const stitch = stitches.value[index];
        const thread = palette.value.get(stitch.threadIndex) as IPatternThread;

        if (thread.index !== event.threadIndex)
            continue;

        graphics.value.fillStyle = thread.colour;
        graphics.value.fillRect(stitch.x * props.baseStitchSize, stitch.y * props.baseStitchSize, props.baseStitchSize, props.baseStitchSize);

        graphics.value.fillStyle = isDark(thread.colour) ? '#ddd' :  '#111';
        graphics.value.fillText(stitch.threadIndex.toString(), stitch.x * props.baseStitchSize + (props.baseStitchSize / 2), (stitch.y + 1) * props.baseStitchSize - (props.baseStitchSize / 2) + 10);
    }
};

useEvent('HighlightThread', onHighlightThread);
</script>