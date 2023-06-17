<template>
    <div
        ref="component"
        class="canvas-component"
        :class="{
            'is-drag-moving': isDragMoving,
            'is-drag-selecting': isDragSelecting,
        }"
        @click="onClick"
        @dblclick="onDoubleClick"
        @pointerdown="onMouseDown"
        @pointerup="onMouseUp"
        @pointermove="onMouseMove"
        @pointerleave="onMouseLeave"
        @wheel="onMouseWheel"
        @touchstart="onTouchStart"
    >
        <div class="canvas-wrapper"
            :style="{
                'transform': `translate(${offset.x}px, ${offset.y}px) scale(${scale})`,
            }"
        >
            <canvas
                ref="canvasElement"
                :width="project.project.pattern.width * baseStitchSize"
                :height="project.project.pattern.height * baseStitchSize"
                :style="{
                }"
            >
            </canvas>
            <canvas
                ref="completedStitchesCanvas"
                :width="project.project.pattern.width * baseStitchSize"
                :height="project.project.pattern.height * baseStitchSize"
            >
            </canvas>
            <div
                v-if="stitchSelectStart && stitchSelectEnd"
                class="selected-stitches-wrapper"
                :style="{
                }"
            >
                <div
                    class="selected-stitches"
                    :style="{
                        'width': `${baseStitchSize * (stitchSelectEnd.x - stitchSelectStart.x + 1)}px`,
                        'height': `${baseStitchSize * (stitchSelectEnd.y - stitchSelectStart.y + 1)}px`,
                        'transform': `translate(${stitchSelectStart.x * stitchSize / scale}px, ${stitchSelectStart.y * stitchSize / scale}px)`,
                    }"
                >
                    <div class="top-axis">
                        {{ stitchSelectEnd.x - stitchSelectStart.x + 1 }}
                    </div>
                    <div class="left-axis">
                        {{ stitchSelectEnd.y - stitchSelectStart.y + 1 }}
                    </div>
                </div>
            </div>
        </div>
        <div
            class="controls flex gap-small"
            @click.stop=""
            @dblclick.stop=""
            @pointerdown.stop=""
            @pointerup.stop=""
            @pointermove.stop=""
            @pointerleave.stop=""
            @wheel.stop=""
            @touchstart.stop=""
        >
            <ButtonComponent title="Zoom in" @click="onZoomIn">
                <IconComponent icon="plus" />
            </ButtonComponent>
            <ButtonComponent title="Zoom out" @click="onZoomOut">&mdash;</ButtonComponent>
            <ButtonComponent title="Show options" @click="onShowOptions">
                <IconComponent icon="menu" />
            </ButtonComponent>
        </div>
        <!-- <div class="debug">
            <div>w: {{ width.toFixed(0) }} h: {{ height.toFixed(0) }}</div>
            <div>mouse | x: {{ mousePosition.x }} y: {{ mousePosition.y }}</div>
            <div>scale {{ scale.toFixed(1) }}</div>
            <div>offset | x: {{ offset.x.toFixed(2) }} y: {{ offset.y.toFixed(2) }}</div>
            <div>stitch | x: {{ mouseStitchPosition.x }} y: {{ mouseStitchPosition.y }}</div>
            <div>stitchSize | {{ stitchSize.toFixed(2) }}</div>
            <div>mouseOverPattern: {{ isMouseOverPattern }}</div>
            <div v-if="selectStart !== null">selectStart | x {{ selectStart.x }} y: {{ selectStart.y }}</div>
            <div v-if="selectEnd !== null">selectEnd | x {{ selectEnd.x }} y: {{ selectEnd.y }}</div>
            <div v-if="stitchSelectStart !== null">stitchSelectStart | x {{ stitchSelectStart.x }} y: {{ stitchSelectStart.y }}</div>
            <div v-if="stitchSelectEnd !== null">stitchSelectEnd | x {{ stitchSelectEnd.x }} y: {{ stitchSelectEnd.y }}</div>
            <div>{{ hoveredStitch }}</div>
        </div> -->
    </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import dayjs from 'dayjs';

import { api } from '@/api/api';
import { isDark } from '@/helper/helper';
import { Position } from '@/class/Position.class';
import { useMouse } from '@/views/default/project/use/Mouse.use';
import { useSharedStitch } from '@/views/default/project/use/SharedStitch';
import { useStitch } from '@/views/default/project/use/Stitch.use';
import { useInput } from '@/use/input/input.use';
import { useTransformation } from '@/views/default/project/use/Transformation.use';

import { IGetProject } from '@/models/GetProject.model';
import { IStitch, IThread } from '@/models/Pattern.model';

const props = defineProps<{
    project: IGetProject;
}>();

const emit = defineEmits(['openInformation']);

const palette = new Map<number, IThread>();
for (const thread of props.project.threads) {
    palette.set(thread.index, thread);
}

const pattern = new Map<string, IStitch>();
for (const stitch of props.project.stitches) {
    pattern.set(`${stitch.x}:${stitch.y}`, stitch);
}

const component = ref<HTMLDivElement>({} as HTMLDivElement);

const sharedStitch = useSharedStitch();
const { mousePosition, prevMousePosition, isDragMoving, isDragSelecting, selectStart, selectEnd } = useMouse();
const { width, height, offset, scale } = useTransformation(component);

const { baseStitchSize, stitchSize, mouseStitchPosition, isMouseOverPattern, stitchSelectStart, stitchSelectEnd } = useStitch({
    pattern: props.project.project.pattern,
    scale,
    mousePosition,
    offset,
    selectStart,
    selectEnd,
});

const canvasElement = ref<HTMLCanvasElement>({} as HTMLCanvasElement);
const completedStitchesCanvas = ref<HTMLCanvasElement>({} as HTMLCanvasElement);

const graphics = computed<CanvasRenderingContext2D>(() => canvasElement.value.getContext('2d') as CanvasRenderingContext2D);
const completedStitchesGraphics = computed<CanvasRenderingContext2D>(() => completedStitchesCanvas.value.getContext('2d') as CanvasRenderingContext2D);

const hoveredStitch = sharedStitch.hoveredStitch;

const canvasWidth = computed<number>(() => props.project.project.pattern.width * stitchSize.value);
const canvasHeight = computed<number>(() => props.project.project.pattern.height * stitchSize.value);

onMounted(() => {
    offset.value = Position.at(width.value / 2 - canvasWidth.value / 2, height.value / 2 - canvasHeight.value / 2);

    graphics.value.fillStyle = '#eef';
    graphics.value.textAlign = 'center';
    graphics.value.font = 'normal 28px sans-serif';

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

        graphics.value.fillStyle = isDark(thread.colour) ? '#ddd' :  '#111';
        graphics.value.fillText(stitch.threadIndex.toString(), stitch.x * baseStitchSize + (baseStitchSize / 2), (stitch.y + 1) * baseStitchSize - (baseStitchSize / 2) + 10);

        completedStitchesGraphics.value.fillStyle = '#0f0';

        if (stitch.stitchedAt !== null)
            completedStitchesGraphics.value.fillRect(stitch.x * baseStitchSize, stitch.y * baseStitchSize, baseStitchSize, baseStitchSize);
    }

    graphics.value.strokeStyle = '#666';
    graphics.value.lineWidth = 2;

    for (let x = 1; x < props.project.project.pattern.width; ++x) {
        graphics.value.beginPath();
        graphics.value.moveTo(x * baseStitchSize, 0);
        graphics.value.lineTo(x * baseStitchSize, props.project.project.pattern.height * baseStitchSize);
        graphics.value.closePath();
        graphics.value.stroke();
    }

    for (let y = 1; y < props.project.project.pattern.height; ++y) {
        graphics.value.beginPath();
        graphics.value.moveTo(0, y * baseStitchSize);
        graphics.value.lineTo(props.project.project.pattern.width * baseStitchSize, y * baseStitchSize);
        graphics.value.closePath();
        graphics.value.stroke();
    }

    graphics.value.lineWidth = 6;
    graphics.value.strokeStyle = '#f00';

    graphics.value.beginPath();
    graphics.value.moveTo(0, props.project.project.pattern.width / 2 * baseStitchSize);
    graphics.value.lineTo(props.project.project.pattern.width * baseStitchSize, props.project.project.pattern.width / 2 * baseStitchSize);
    graphics.value.closePath();
    graphics.value.stroke();

    graphics.value.beginPath();
    graphics.value.moveTo(props.project.project.pattern.height / 2 * baseStitchSize, 0);
    graphics.value.lineTo(props.project.project.pattern.height / 2 * baseStitchSize, props.project.project.pattern.height * baseStitchSize);
    graphics.value.closePath();
    graphics.value.stroke();
});

const onClick = function (): void {
    selectStart.value = null;
    selectEnd.value = null;
};

const onDoubleClick = async function (): Promise<void> {
    if (hoveredStitch.value === null)
        return;

    const stitch = hoveredStitch.value;

    if (stitch.stitchedAt === null) {
        completedStitchesGraphics.value.fillRect(
            mouseStitchPosition.value.x * baseStitchSize,
            mouseStitchPosition.value.y * baseStitchSize,
            baseStitchSize,
            baseStitchSize);

        stitch.stitchedAt = dayjs();

        await api.projects.completeStitches(props.project.project.pattern.reference, {
            positions: [
                {
                    x: mouseStitchPosition.value.x,
                    y: mouseStitchPosition.value.y,
                },
            ],
        });
    }
    else {
        completedStitchesGraphics.value.clearRect(
            mouseStitchPosition.value.x * baseStitchSize,
            mouseStitchPosition.value.y * baseStitchSize,
            baseStitchSize,
            baseStitchSize);

        stitch.stitchedAt = null;

        await api.projects.unCompleteStitches(props.project.project.pattern.reference, {
            positions: [
                {
                    x: mouseStitchPosition.value.x,
                    y: mouseStitchPosition.value.y,
                },
            ],
        });
    }
};

let doubleTouched: number | null = null;
const onTouchStart = async function (event: TouchEvent): Promise<void> {
    if (doubleTouched === null) {
        doubleTouched = setTimeout(() => {
            doubleTouched = null;
        }, 300);

        return;
    }

    clearTimeout(doubleTouched);
    doubleTouched = null;

    handleHoveredStitch();
    await onDoubleClick();

    event.preventDefault();
};

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

            const stitch = pattern.get(`${position.x}:${position.y}`);
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

    if (event.shiftKey) {
        for (const stitch of stitches) {
            completedStitchesGraphics.value.clearRect(
                stitch.x * baseStitchSize,
                stitch.y * baseStitchSize,
                baseStitchSize,
                baseStitchSize);

            stitch.stitchedAt = null;
        }

        await api.projects.unCompleteStitches(props.project.project.pattern.reference, {
            positions: stitches.map(stitch => ({
                x: stitch.x,
                y: stitch.y,
            })),
        });
    }
    else {
        for (const stitch of stitches) {
            completedStitchesGraphics.value.fillRect(
                stitch.x * baseStitchSize,
                stitch.y * baseStitchSize,
                baseStitchSize,
                baseStitchSize);

            stitch.stitchedAt = dayjs();
        }

        await api.projects.completeStitches(props.project.project.pattern.reference, {
            positions: stitches.map(stitch => ({
                x: stitch.x,
                y: stitch.y,
            })),
        });
    }
});

const onMouseDown = function (event: MouseEvent): void {
    mousePosition.value = Position.at(event.x, event.y).translate(-(component.value.offsetLeft ?? 0), -(component.value.offsetTop ?? 0));
    prevMousePosition.value = Position.copy(mousePosition.value);

    if (event.button === 1 && isMouseOverPattern.value) {
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
    if (!isMouseOverPattern.value)
        return;

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

    hoveredStitch.value = stitch;
};

const onMouseMove = function (event: MouseEvent): void {
    mousePosition.value = Position.at(event.x, event.y).translate(-(component.value.offsetLeft ?? 0), -(component.value.offsetTop ?? 0));

    if (isDragMoving.value) {
        const diff = mousePosition.value.translate(-prevMousePosition.value.x, -prevMousePosition.value.y);

        offset.value = offset.value
            .translate(diff.x, diff.y)
            .min(-canvasWidth.value + width.value * 0.333, -canvasHeight.value + height.value * 0.333)
            .max(width.value * 0.666, height.value * 0.666)
            .floor();
    }

    handleHoveredStitch();

    prevMousePosition.value = Position.copy(mousePosition.value);
};

const onMouseLeave = function (): void {
    isDragMoving.value = false;
    hoveredStitch.value = null;
};

const zoom = function (delta: number, centerX: number, centerY: number): void {
    const factor = delta < 0 ? 1.25 : 0.8;

    const newScale = scale.value * factor;
    if (newScale > 1.1 || newScale < 0.1)
        return;

    scale.value = newScale;

    const dx = (centerX - offset.value.x) * (factor - 1);
    const dy = (centerY - offset.value.y) * (factor - 1);

    offset.value = offset.value.translate(-dx, -dy);
};

const onMouseWheel = function (event: WheelEvent): void {
    if (!isMouseOverPattern.value)
        return;

    zoom(event.deltaY, mousePosition.value.x, mousePosition.value.y);
};

const onZoomIn = function (): void {
    zoom(-1, width.value / 2, height.value / 2);
};

const onZoomOut = function (): void {
    zoom(1, width.value / 2, height.value / 2);
};

const onShowOptions = function (): void {
    emit('openInformation');
};
</script>

<style lang="scss">
@use '@/style/variables' as *;

.canvas-component {
    position: absolute;
    inset: 0;
    overflow: hidden;

    * {
        transition: none;
    }

    &.is-drag-moving {
        .canvas-wrapper {
            transition: transform 0s;
        }
    }

    &.is-drag-selecting {
        .selected-stitches-wrapper .selected-stitches {
            transition: transform 0s;
        }
    }

    .canvas-wrapper {
        position: relative;
        transition: transform 0.1s;
        transform-origin: top left;
    }

    canvas {
        position: absolute;
        inset: 0;
        pointer-events: none;
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
        inset: 0;

        .selected-stitches {
            position: absolute;
            transform-origin: top left;
            border-radius: 0.2rem;
            box-shadow: 1px 2px 5px rgba(0, 0, 0, 0.5), 1px 2px 10px rgba(0, 0, 0, 0.2);
            background-color: rgba(33, 33, 200, 0.2);
            transition: transform 0.1s;
        }

        .top-axis,
        .left-axis {
            width: 2rem;
            aspect-ratio: 1;
            line-height: 2rem;
            text-align: center;
            background-color: var(--wjb-background-colour);
            border-radius: 50%;

            @include shadow-medium();
        }

        .top-axis {
            position: absolute;
            top: -2.5rem;
            left: 50%;
            transform: translateX(-50%);
        }

        .left-axis {
            position: absolute;
            left: -2.5rem;
            top: 50%;
            transform: translateY(-50%);
        }
    }

    .controls {
        position: fixed;
        inset: auto 1rem 1rem auto;
        padding: 1rem;
        line-height: 1em;
        background-color: var(--wjb-primary);
        background: linear-gradient(
            -5deg,
            transparentize($primary-dark, 0.05),
            transparentize($primary, 0.05),
        );
        backdrop-filter: blur(2px);
        border-radius: 2rem;

        button {
            box-shadow: none;
        }

        @include shadow-medium();

        @media screen and (max-width: 720px) {
            left: 50%;
            right: auto;
            transform: translateX(-50%);
        }
    }

    .debug {
        position: fixed;
        inset: 50% 0.25rem auto auto;
        padding: 0.3rem;
        border-radius: var(--wjb-border-radius);
        text-align: right;
        font-size: 0.7rem;
        line-height: 1.3em;
        user-select: none;
        background-color: rgba(0, 0, 0, 0.6);
        color: #fff;
        pointer-events: none;
        transform: translateY(-50%);
    }
}
</style>