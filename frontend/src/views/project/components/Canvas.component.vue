<template>
    <div
        ref="component"
        class="canvas-component"
        :class="{
            'is-drag-moving': isDragMoving,
        }"
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
                :width="project.project.pattern.width * baseStitchSize"
                :height="project.project.pattern.height * baseStitchSize"
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

import { useGlobalData } from '@/use/global-data/global-data.use';

import { Position } from '@/class/Position.class';
import { useMouse } from '../use/Mouse.use';
import { useTransformation } from '../use/Transformation.use';

import { IGetProject } from '@/models/GetProject.model';
import { IStitch, IThread } from '@/models/Pattern.model';

const props = defineProps<{
    project: IGetProject;
}>();

const palette = new Map<number, IThread>();
for (const thread of props.project.threads) {
    palette.set(thread.index, thread);
}

const pattern = new Map<string, IStitch>();
for (const stitch of props.project.stitches) {
    pattern.set(`${stitch.x}:${stitch.y}`, stitch);
}

const globalData = useGlobalData();
const { mousePosition, prevMousePosition, isDragMoving, isDragSelecting, selectStart, selectEnd } = useMouse();
const { offset, scale } = useTransformation();

const component = ref<HTMLDivElement>({} as HTMLDivElement);
const canvasElement = ref<HTMLCanvasElement>({} as HTMLCanvasElement);
const graphics = computed<CanvasRenderingContext2D>(() => canvasElement.value.getContext('2d') as CanvasRenderingContext2D);

const hoveredStitch = globalData.hoveredStitch;

const baseStitchSize = 15;
const stitchSize = computed<number>(() => Math.round(baseStitchSize * scale.value));

const width = computed<number>(() => component.value.offsetWidth ?? 0);
const height = computed<number>(() => component.value.offsetHeight ?? 0);

const canvasWidth = computed<number>(() => props.project.project.pattern.width * stitchSize.value);
const canvasHeight = computed<number>(() => props.project.project.pattern.height * stitchSize.value);

const isMouseOverPattern = computed<boolean>(() => {
    return mouseStitchPosition.value.x > 0 &&
        mouseStitchPosition.value.y > 0 &&
        mouseStitchPosition.value.x < props.project.project.pattern.width &&
        mouseStitchPosition.value.y < props.project.project.pattern.height
});

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

onMounted(() => {
    offset.value = Position.at(width.value / 2 - canvasWidth.value / 2, height.value / 2 - canvasHeight.value / 2);

    graphics.value.fillStyle = '#eef';

    for (let x = 0; x < props.project.project.pattern.width; ++x) {
        for (let y = 0; y < props.project.project.pattern.height; ++y) {
            graphics.value.fillRect(x * baseStitchSize, y * baseStitchSize, baseStitchSize, baseStitchSize);
        }
    }

    for (let index = 0; index < props.project.stitches.length; ++index) {
        const stitch = props.project.stitches[index];
        const thread = palette.get(stitch.threadIndex) as IThread;

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

const handleHoveredStitch = function (): void {
    if (mouseStitchPosition.value.x > 0 &&
        mouseStitchPosition.value.y > 0 &&
        mouseStitchPosition.value.x < props.project.project.pattern.width &&
        mouseStitchPosition.value.y < props.project.project.pattern.height)
    {
        const stitch = pattern.get(`${mouseStitchPosition.value.x}:${mouseStitchPosition.value.y}`);
        if (!stitch) {
            hoveredStitch.value = null;
            return;
        }

        const thread = palette.get(stitch.threadIndex);
        if (!thread || thread.index === 0) {
            hoveredStitch.value = null;
            return;
        }

        hoveredStitch.value = {
            x: mouseStitchPosition.value.x,
            y: mouseStitchPosition.value.y,
            thread: {
                index: thread.index,
                name: thread.name,
                description: thread.description,
                colour: thread.colour,
            },
            isDone: false,
        };
    }
};

const onMouseMove = function (event: MouseEvent): void {
    mousePosition.value = Position.at(event.x, event.y).translate(-(component.value.offsetLeft ?? 0), -(component.value.offsetTop ?? 0));

    if (isDragMoving.value) {
        const diff = mousePosition.value.translate(-prevMousePosition.value.x, -prevMousePosition.value.y);

        offset.value = offset.value
            .translate(diff.x, diff.y)
            .floor();
    }

    handleHoveredStitch();

    prevMousePosition.value = Position.copy(mousePosition.value);
};

const onMouseLeave = function (): void {
    isDragMoving.value = false;
    hoveredStitch.value = null;
};

const onMouseWheel = function (event: WheelEvent): void {
    if (!isMouseOverPattern.value)
        return;

    let factor = 0.8;
    if (event.deltaY < 0) {
        factor = 1 / factor;
    }

    const prevScale = scale.value;
    scale.value = Math.max(0.2, Math.min(2, scale.value * factor));

    if (scale.value === prevScale)
        return;

    const dx = (mousePosition.value.x - offset.value.x) * (factor - 1);
    const dy = (mousePosition.value.y - offset.value.y) * (factor - 1);

    offset.value = offset.value.translate(-dx, -dy);
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

    &.is-drag-moving {
        .canvas-wrapper {
            transition: transform 0s;
        }
    }

    .canvas-wrapper {
        position: absolute;
        transition: transform 0.1s;
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