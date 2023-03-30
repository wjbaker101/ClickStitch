<template>
    <canvas
        ref="canvasElement"
        class="canvas-component flex-1"
        :class="{ 'is-drag-selecting': isDragSelecting }"
        :width="width"
        :height="height"
        @mousedown="onMouseDown"
        @mouseup="onMouseUp"
        @mousemove="onMouseMove"
        @mouseleave="onMouseLeave"
        @wheel="onMouseWheel"
        @contextmenu="onContextMenu"
    >
    </canvas>
    <div class="debug">
        <div>w: {{ width.toFixed(0) }} h: {{ height.toFixed(0) }}</div>
        <div>mouse | x: {{ mousePosition.x }} y: {{ mousePosition.y }}</div>
        <div>offset | x: {{ offset.x }} y: {{ offset.y }}</div>
        <div>stitch | x: {{ mouseStitchPosition.x }} y: {{ mouseStitchPosition.y }}</div>
        <div v-if="selectStart !== null">selectStart | x {{ selectStart.x }} y: {{ selectStart.y }}</div>
        <div v-if="selectEnd !== null">selectEnd | x {{ selectEnd.x }} y: {{ selectEnd.y }}</div>
        <div></div>
    </div>
</template>

<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref } from 'vue';

import { useInput } from '@/use/input/input.use';
import { useCurrentProject } from '@/use/current-project/CurrentProject.use';
import { useGlobalData } from '@/use/global-data/global-data.use';

import { Position } from '@/class/Position.class';
import { ICanvas } from '@/model/canvas.model';

const props = defineProps<{
    canvas: ICanvas;
}>();

const canvasElement = ref<HTMLCanvasElement>({} as HTMLCanvasElement);
const graphics = computed<CanvasRenderingContext2D>(() => canvasElement.value.getContext('2d') as CanvasRenderingContext2D);

const input = useInput();
const currentProject = useCurrentProject();
const globalData = useGlobalData();

const hoveredStitch = globalData.hoveredStitch;

const project = currentProject.project;

const width = ref<number>(canvasElement.value.offsetWidth ?? 0);
const height = ref<number>(canvasElement.value.offsetHeight ?? 0);

const mousePosition = ref<Position>(Position.ZERO);
const prevMousePosition = ref<Position>(Position.ZERO);

const offset = ref<Position>(Position.ZERO);
const isDragMoving = ref<boolean>(false);

const scale = ref<number>(1);

const stitchSize = computed<number>(() => Math.round(15 * scale.value));

const mouseStitchPosition = computed<Position>(() => mousePosition.value
    .translate(offset.value.x, offset.value.y)
    .scale(1.0 / stitchSize.value, 1.0 / stitchSize.value)
    .floor());

const isDragSelecting = ref<boolean>(false);
const selectStart = ref<Position | null>(null);
const selectEnd = ref<Position | null>(null);

const resizeObserver = new ResizeObserver(elements => {
    const element = elements[0];

    width.value = element.contentRect.width;
    height.value = element.contentRect.height;

    requestAnimationFrame(render);
});

const render = function () {
    graphics.value.clearRect(0, 0, width.value, height.value);

    const stitchesX = Math.min(props.canvas.width, width.value / stitchSize.value);
    const stitchesY = Math.min(props.canvas.height, height.value / stitchSize.value);

    const remainderX = offset.value.x % stitchSize.value;
    const remainderY = offset.value.y % stitchSize.value;

    const stitchStartX = Math.floor(offset.value.x / stitchSize.value);
    const stitchStartY = Math.floor(offset.value.y / stitchSize.value);

    for (let x = 0; x < stitchesX; ++x) {
        for (let y = 0; y < stitchesY; ++y) {

            const getX = x + stitchStartX;
            const getY = y + stitchStartY;

            const stitch = props.canvas.stitches[getX + props.canvas.width * getY];

            const colour =  stitch.isDone ? '#0f0' : project.value.palette.threads.get(stitch.threadIndex)?.colour ?? 'transparent'

            graphics.value.fillStyle = colour;
            graphics.value.fillRect(x * stitchSize.value - remainderX, y * stitchSize.value - remainderY, stitchSize.value, stitchSize.value);
        }
    }

    if (selectStart.value !== null) {
        const renderStart = selectStart.value
            .translate(-stitchStartX, -stitchStartY)
            .scale(stitchSize.value, stitchSize.value);

        const renderEnd = (selectEnd.value ?? mouseStitchPosition.value)
            .translate(-stitchStartX + 1, -stitchStartY + 1)
            .scale(stitchSize.value, stitchSize.value);

        const width = renderEnd.x - renderStart.x;
        const height = renderEnd.y - renderStart.y;

        const colour = 'rgba(23, 107, 192, 0.5)';
        graphics.value.fillStyle = colour;
        graphics.value.strokeStyle = colour;

        graphics.value.fillRect(renderStart.x - remainderX, renderStart.y - remainderY, width, height);
        graphics.value.strokeRect(renderStart.x - remainderX, renderStart.y - remainderY, width, height);
    }

    const verticalLineX = props.canvas.width / 2 - stitchStartX;
    const horizontalLineY = props.canvas.height / 2 - stitchStartY;

    graphics.value.strokeStyle = '#000000';
    graphics.value.beginPath();
    graphics.value.moveTo(verticalLineX * stitchSize.value - remainderX + 0.5, 0);
    graphics.value.lineTo(verticalLineX * stitchSize.value - remainderX + 0.5, props.canvas.height * stitchSize.value);
    graphics.value.moveTo(0, horizontalLineY * stitchSize.value - remainderY + 0.5);
    graphics.value.lineTo(props.canvas.width * stitchSize.value, horizontalLineY * stitchSize.value - remainderY + 0.5);
    graphics.value.closePath();
    graphics.value.stroke();
};

const onKeyUp = function (event: KeyboardEvent): void {
    if (event.key === 'Escape') {
        selectStart.value = null;
        selectEnd.value = null;
    }

    if (event.key == ' ') {
        if (selectStart.value !== null && selectEnd.value !== null) {
            for (let y = 0; y < selectEnd.value.y - selectStart.value.y + 1; ++y) {
                for (let x = 0; x < selectEnd.value.x - selectStart.value.x + 1; ++x) {
                    const getX = x + selectStart.value.x;
                    const getY = y + selectStart.value.y;

                    props.canvas.stitches[getX + props.canvas.width * getY].isDone = !input.keysDown.has('Control');
                }
            }

            selectStart.value = null;
            selectEnd.value = null;
        }
    }
};

onMounted(() => {
    document.addEventListener('keyup', onKeyUp);

    requestAnimationFrame(render);

    resizeObserver.observe(canvasElement.value);
});

onUnmounted(() => {
    document.removeEventListener('keyup', onKeyUp);
});

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
    mousePosition.value = Position.at(event.x, event.y).translate(-(canvasElement.value.offsetLeft ?? 0), -(canvasElement.value.offsetTop ?? 0));

    if (isDragMoving.value) {
        const diff = mousePosition.value.translate(-prevMousePosition.value.x, -prevMousePosition.value.y);

        offset.value = offset.value
            .translate(-diff.x, -diff.y)
            .max(props.canvas.width * stitchSize.value - width.value, props.canvas.height * stitchSize.value - height.value)
            .min(0, 0)
            .floor();
    }

    if (mouseStitchPosition.value.x > 0 && mouseStitchPosition.value.y > 0 && mouseStitchPosition.value.x < props.canvas.width && mouseStitchPosition.value.y < props.canvas.height) {
        const stitch = props.canvas.stitches[mouseStitchPosition.value.x + props.canvas.width * mouseStitchPosition.value.y];
        const thread = project.value.palette.threads.get(stitch.threadIndex);

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

    requestAnimationFrame(render);
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
};

const onContextMenu = function (event: Event): void {
    event.preventDefault();
};
</script>

<style lang="scss">
.canvas-component {
    width: 100%;
    height: 100%;
    vertical-align: middle;
    cursor: move;

    &.is-drag-selecting {
        cursor: pointer;
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
</style>