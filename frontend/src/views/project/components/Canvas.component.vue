<template>
    <div
        ref="component"
        class="canvas-component"
        @click="onClick"
        @mousedown="onMouseDown"
        @mouseup="onMouseUp"
        @mousemove="onMouseMove"
        @mouseleave="onMouseLeave"
        @wheel="onMouseWheel"
    >
        <div class="canvas-wrapper"
            :style="{
                'transform': `translate(${offset.x}px, ${offset.y}px)`,
            }"
        >
            <canvas
                ref="canvasElement"
                :width="project.canvas.width * baseStitchSize"
                :height="project.canvas.height * baseStitchSize"
                :style="{
                    'transform': `scale(${scale})`,
                }"
            >
            </canvas>
        </div>
        <div
            v-if="selectionStart && selectionEnd"
            class="selected-stitches-wrapper"
            :style="{
                'transform': `translate(${selectionStart.x * stitchSize + offset.x}px, ${selectionStart.y * stitchSize + offset.y}px)`,
            }"
        >
            <div
                class="selected-stitches"
                :style="{
                    'width': `${baseStitchSize * (selectionEnd.x - selectionStart.x)}px`,
                    'height': `${baseStitchSize * (selectionEnd.y - selectionStart.y)}px`,
                    'transform': `scale(${scale})`,
                }"
            ></div>
        </div>
        <div class="debug">
            <div>w: {{ width.toFixed(0) }} h: {{ height.toFixed(0) }}</div>
            <div>mouse | x: {{ mousePosition.x }} y: {{ mousePosition.y }}</div>
            <div>scale {{ scale }}</div>
            <div>offset | x: {{ offset.x }} y: {{ offset.y }}</div>
            <div>stitch | x: {{ mouseStitchPosition.x }} y: {{ mouseStitchPosition.y }}</div>
            <div v-if="selectStart !== null">selectStart | x {{ selectStart.x }} y: {{ selectStart.y }}</div>
            <div v-if="selectEnd !== null">selectEnd | x {{ selectEnd.x }} y: {{ selectEnd.y }}</div>
            <div></div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';

import { useCurrentProject } from '@/use/current-project/CurrentProject.use';
import { useGlobalData } from '@/use/global-data/global-data.use';

import { Position } from '@/class/Position.class';
import { IGetProject } from '@/models/GetProject.model';
import { IThread } from '@/models/Pattern.model';

const props = defineProps<{
    project: IGetProject;
}>();

const palette2 = new Map<number, IThread>();
for (const thread of props.project.threads) {
    palette2.set(thread.index, thread);
}

const component = ref<HTMLDivElement>({} as HTMLDivElement);
const canvasElement = ref<HTMLCanvasElement>({} as HTMLCanvasElement);
const graphics = computed<CanvasRenderingContext2D>(() => canvasElement.value.getContext('2d') as CanvasRenderingContext2D);

const currentProject = useCurrentProject();
const globalData = useGlobalData();

const hoveredStitch = globalData.hoveredStitch;
const project = currentProject.project;
const palette = computed(() => project.value.palette);
const stitches = computed(() => project.value.canvas.stitches);

const baseStitchSize = 15;
const stitchSize = computed<number>(() => Math.round(baseStitchSize * scale.value));

const width = computed<number>(() => component.value.offsetWidth ?? 0);
const height = computed<number>(() => component.value.offsetHeight ?? 0);
const mousePosition = ref<Position>(Position.ZERO);
const prevMousePosition = ref<Position>(Position.ZERO);
const offset = ref<Position>(Position.ZERO);
const isDragMoving = ref<boolean>(false);
const isDragSelecting = ref<boolean>(false);
const selectStart = ref<Position | null>(null);
const selectEnd = ref<Position | null>(null);
const scale = ref<number>(1);

const canvasWidth = computed<number>(() => project.value.canvas.width * stitchSize.value);
const canvasHeight = computed<number>(() => project.value.canvas.height * stitchSize.value);

const mouseStitchPosition = computed<Position>(() => mousePosition.value
    .translate(-offset.value.x, -offset.value.y)
    .scale(1.0 / stitchSize.value, 1.0 / stitchSize.value)
    .floor());

const selectionStart = computed<Position | null>(() => {
    if (selectStart.value === null)
        return null;

    return Position.at(
        Math.min(selectStart.value.x, selectEnd.value?.x ?? mouseStitchPosition.value.x),
        Math.min(selectStart.value.y, selectEnd.value?.y ?? mouseStitchPosition.value.y));
});

const selectionEnd = computed<Position | null>(() => {
    if (selectStart.value === null)
        return null;

    return Position.at(
        Math.max(selectStart.value.x, selectEnd.value?.x ?? mouseStitchPosition.value.x),
        Math.max(selectStart.value.y, selectEnd.value?.y ?? mouseStitchPosition.value.y));
});

const padding = 10;

const maxOffsetX = computed<number>(() => {
    if (canvasWidth.value + 2 * padding > width.value)
        return width.value - (canvasWidth.value + padding);

    return width.value - canvasWidth.value - padding;
});

const maxOffsetY = computed<number>(() => {
    return height.value - canvasHeight.value - padding;
});

onMounted(() => {
    graphics.value.fillStyle = '#eef';

    for (let x = 0; x < props.project.project.pattern.width; ++x) {
        for (let y = 0; y < props.project.project.pattern.height; ++y) {
            graphics.value.fillRect(x * baseStitchSize, y * baseStitchSize, baseStitchSize, baseStitchSize);
        }
    }

    for (let index = 0; index < props.project.stitches.length; ++index) {
        const stitch = props.project.stitches[index];
        const thread = palette2.get(stitch.threadIndex) as IThread;

        graphics.value.fillStyle = thread.colour;
        graphics.value.fillRect(stitch.x * baseStitchSize, stitch.y * baseStitchSize, baseStitchSize, baseStitchSize);
    }
});

const onClick = function (): void {
    selectStart.value = null;
    selectEnd.value = null;
};

const onMouseDown = function (event: MouseEvent): void {
    if (event.button === 1) {
        isDragSelecting.value = true;
        selectStart.value = mouseStitchPosition.value;
        selectEnd.value = null;
    }
    if (event.button === 0)
        isDragMoving.value = true;
};

const onMouseUp = function (event: MouseEvent): void {
    if (event.button === 1) {
        isDragSelecting.value = false;
        selectEnd.value = mouseStitchPosition.value;
    }
    if (event.button === 0)
        isDragMoving.value = false;
};

const onMouseMove = function (event: MouseEvent): void {
    mousePosition.value = Position.at(event.x, event.y).translate(-(component.value.offsetLeft ?? 0), -(component.value.offsetTop ?? 0));

    if (isDragMoving.value) {
        const diff = mousePosition.value.translate(-prevMousePosition.value.x, -prevMousePosition.value.y);

        offset.value = offset.value
            .translate(diff.x, diff.y)
            // .min(10, 10)
            // .max(maxOffsetX.value, maxOffsetY.value)
            .floor();
    }

    if (mouseStitchPosition.value.x > 0 &&
        mouseStitchPosition.value.y > 0 &&
        mouseStitchPosition.value.x < project.value.canvas.width &&
        mouseStitchPosition.value.y < project.value.canvas.height)
    {
        const stitch = project.value.canvas.stitches[mouseStitchPosition.value.x + project.value.canvas.width * mouseStitchPosition.value.y];
        const thread = palette.value.threads.get(stitch.threadIndex);

        if (thread && thread.index !== 0) {
            hoveredStitch.value = {
                x: mouseStitchPosition.value.x,
                y: mouseStitchPosition.value.y,
                thread: {
                    index: thread.index,
                    name: thread.name,
                    description: thread.description,
                    colour: thread.colour,
                },
                isDone: stitch.isDone,
            };
        }
        else {
            hoveredStitch.value = null;
        }
    }

    prevMousePosition.value = Position.copy(mousePosition.value);
};

const onMouseLeave = function (): void {
    isDragMoving.value = false;
    hoveredStitch.value = null;
};

const onMouseWheel = function (event: WheelEvent) {
    if (event.deltaY > 0) {
        scale.value = Math.max(0.4, scale.value - 0.1);
    }

    if (event.deltaY < 0) {
        scale.value = Math.min(2, scale.value + 0.1);
    }

    const relativeMousePosition = mousePosition.value.subtract(offset.value);
    const relativeMousePositionScaled = relativeMousePosition.scale(scale.value, scale.value);

    // offset.value = offset.value
    //     .subtract(relativeMousePositionScaled);
};
</script>

<style lang="scss">
@use '@/style/variables' as *;

.canvas-component {
    position: relative;
    overflow: hidden;
    flex: 1;

    * {
        transition: none;
    }

    .canvas-wrapper {
        position: absolute;
    }

    canvas {
        pointer-events: none;
        transform-origin: top left;
        border-radius: var(--wjb-border-radius);
        image-rendering: crisp-edges;
        image-rendering: -moz-crisp-edges;
        image-rendering: -webkit-optimize-contrast;
        image-rendering: optimize-contrast;
        -ms-interpolation-mode: nearest-neighbor;
        transition: transform 0.1s;

        @include shadow-large();
    }

    .selected-stitches-wrapper {
        position: absolute;

        .selected-stitches {
            position: absolute;
            transform-origin: top left;
            border-radius: 0.2rem;
            box-shadow: 1px 2px 5px rgba(0, 0, 0, 0.5), 1px 2px 10px rgba(0, 0, 0, 0.2);
            background-color: rgba(33, 33, 200, 0.2);
            transition: transform 0.1s;
        }
    }

    .debug {
        position: fixed;
        bottom: 1px;
        right: 1px;
        padding: 0.3rem;
        border-radius: var(--wjb-border-radius);
        text-align: right;
        font-size: 0.7rem;
        line-height: 1.3em;
        user-select: none;
        background-color: rgba(0, 0, 0, 0.6);
        color: #fff;
        pointer-events: none;
    }
}
</style>