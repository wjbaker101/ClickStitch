<template>
    <div
        ref="component"
        class="canvas-component"
        :class="{
            'is-drag-moving': isDragMoving,
            'is-drag-selecting': isDragSelecting,
        }"
        @click="onClick"
        @pointerdown="onMouseDown"
        @pointerup="onMouseUp"
        @pointermove="onMouseMove"
        @pointerleave="onMouseLeave"
        @wheel="onMouseWheel"
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
            <canvas
                ref="jumpedStitchCanvas"
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
            <div>Pinch: {{ pinchStart.toFixed(2) }} / {{ pinchDiff.toFixed(2) }}</div>
            <div>{{ hoveredStitch }}</div>
        </div> -->
    </div>
</template>

<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref } from 'vue';
import dayjs from 'dayjs';

import { api } from '@/api/api';
import { isDark } from '@/helper/helper';
import { Position } from '@/class/Position.class';
import { useCurrentProject } from '@/views/stitcher/project/use/CurrentProject.use';
import { useHammer } from '@/views/stitcher/project/use/Hammer.use';
import { useMouse } from '@/views/stitcher/project/use/Mouse.use';
import { useSharedStitch } from '@/views/stitcher/project/use/SharedStitch';
import { useStitch } from '@/views/stitcher/project/use/Stitch.use';
import { useInput } from '@/use/input/input.use';
import { useTransformation } from '@/views/stitcher/project/use/Transformation.use';
import { useEvents } from '@/use/events/Events.use';

import { type IGetProject } from '@/models/GetProject.model';
import { type IStitch, type IPatternThread } from '@/models/Pattern.model';
import { type IPosition } from '@/api/types/CompleteStitches.type';
import type { IJumpToStitchEvent } from '@/use/events/types/EventsMap.type';

const props = defineProps<{
    project: IGetProject;
}>();

const component = ref<HTMLDivElement>({} as HTMLDivElement);

const currentProject = useCurrentProject();
const events = useEvents();
const sharedStitch = useSharedStitch();
const { mousePosition, prevMousePosition, isDragMoving, isDragSelecting, selectStart, selectEnd } = useMouse();
const { width, height, offset, scale, zoom } = useTransformation();
const pinchStart = ref<number>(1);
const pinchDiff = ref<number>(1);

const { baseStitchSize, stitchSize, mouseStitchPosition, isMouseOverPattern, stitchSelectStart, stitchSelectEnd } = useStitch({
    pattern: props.project.project.pattern,
    scale,
    mousePosition,
    offset,
    selectStart,
    selectEnd,
});

const pattern = new Map<string, IStitch>();
for (const stitch of currentProject.stitches.value) {
    pattern.set(`${stitch.x}:${stitch.y}`, stitch);
}

const canvasElement = ref<HTMLCanvasElement>({} as HTMLCanvasElement);
const completedStitchesCanvas = ref<HTMLCanvasElement>({} as HTMLCanvasElement);
const jumpedStitchCanvas = ref<HTMLCanvasElement>({} as HTMLCanvasElement);

const graphics = computed<CanvasRenderingContext2D>(() => canvasElement.value.getContext('2d') as CanvasRenderingContext2D);
const completedStitchesGraphics = computed<CanvasRenderingContext2D>(() => completedStitchesCanvas.value.getContext('2d') as CanvasRenderingContext2D);
const jumpedStitchGraphics = computed<CanvasRenderingContext2D>(() => jumpedStitchCanvas.value.getContext('2d') as CanvasRenderingContext2D);

const hoveredStitch = sharedStitch.hoveredStitch;

const canvasWidth = computed<number>(() => props.project.project.pattern.width * stitchSize.value);
const canvasHeight = computed<number>(() => props.project.project.pattern.height * stitchSize.value);

const onResize = function (): void {
    width.value = component.value.offsetWidth;
    height.value = component.value.offsetHeight;
};

const onJumpToStitch = function (event: IJumpToStitchEvent): void {
    offset.value = Position
        .at(-event.x * stitchSize.value, -event.y * stitchSize.value)
        .translate(width.value / 2, height.value / 2);

    jumpedStitchGraphics.value.strokeStyle = '#ffb400';
    jumpedStitchGraphics.value.lineWidth = 5;
    jumpedStitchGraphics.value.strokeRect(event.x * baseStitchSize, event.y * baseStitchSize, baseStitchSize, baseStitchSize);
    jumpedStitchGraphics.value.stroke();
};

onMounted(() => {
    onResize();
    window.addEventListener('resize', onResize);

    offset.value = Position.at(width.value / 2 - canvasWidth.value / 2, height.value / 2 - canvasHeight.value / 2);

    graphics.value.fillStyle = '#eef';
    graphics.value.textAlign = 'center';
    graphics.value.font = 'normal 28px sans-serif';

    for (let x = 0; x < props.project.project.pattern.width; ++x) {
        for (let y = 0; y < props.project.project.pattern.height; ++y) {
            graphics.value.fillRect(x * baseStitchSize, y * baseStitchSize, baseStitchSize, baseStitchSize);
        }
    }

    for (let index = 0; index < currentProject.stitches.value.length; ++index) {
        const stitch = currentProject.stitches.value[index];
        const thread = currentProject.palette.value.get(stitch.threadIndex) as IPatternThread;

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

    const hammer = useHammer(component);

    hammer.on('double-tap', () => {
        handleHoveredStitch();
        onDoubleClick();
    });

    hammer.on('pan', (e) => {
        if (e.maxPointers > 1)
            return;

        const diff = mousePosition.value.translate(-prevMousePosition.value.x, -prevMousePosition.value.y);

        offset.value = offset.value
            .translate(diff.x, diff.y)
            .min(-canvasWidth.value + width.value * 0.333, -canvasHeight.value + height.value * 0.333)
            .max(width.value * 0.666, height.value * 0.666)
            .floor();

        prevMousePosition.value = Position.copy(mousePosition.value);
    });

    hammer.on('pinch', (e) => {
        mousePosition.value = Position.at(e.center.x, e.center.y);

        pinchDiff.value = e.scale - pinchStart.value;
        const diff = pinchDiff.value / 2;
        const newScale = scale.value + diff;

        if (newScale > 1.1 || newScale < 0.1)
            return;

        const factor = newScale / scale.value;

        scale.value = newScale;

        const dx = (mousePosition.value.x - offset.value.x) * (factor - 1);
        const dy = (mousePosition.value.y - offset.value.y) * (factor - 1);

        offset.value = offset.value.translate(-dx, -dy);
        pinchStart.value = e.scale;
    });

    hammer.on('pinchstart', (e) => {
        pinchStart.value = e.scale;
    });

    events.subscribe('JumpToStitch', onJumpToStitch);
});

onUnmounted(() => {
    window.removeEventListener('resize', onResize);
    events.unsubscribe('JumpToStitch', onJumpToStitch);
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
            stitchesByThread: {
                [stitch.threadIndex]: [
                    {
                        x: mouseStitchPosition.value.x,
                        y: mouseStitchPosition.value.y,
                    },
                ],
            },
        });

        const thread = props.project.threads.find(x => x.thread.index === stitch.threadIndex);
        if (thread === undefined)
            return;

        thread.completedStitches.push([stitch.x, stitch.y, dayjs()]);

        const stitchIndex = thread.stitches.findIndex(x => x[0] === stitch.x && x[1] === stitch.y);
        thread.stitches.splice(stitchIndex, 1);
    }
    else {
        completedStitchesGraphics.value.clearRect(
            mouseStitchPosition.value.x * baseStitchSize,
            mouseStitchPosition.value.y * baseStitchSize,
            baseStitchSize,
            baseStitchSize);

        stitch.stitchedAt = null;

        await api.projects.unCompleteStitches(props.project.project.pattern.reference, {
            stitchesByThread: {
                [stitch.threadIndex]: [
                    {
                        x: mouseStitchPosition.value.x,
                        y: mouseStitchPosition.value.y,
                    },
                ],
            },
        });

        const thread = props.project.threads.find(x => x.thread.index === stitch.threadIndex);
        if (thread === undefined)
            return;

        thread.stitches.push([stitch.x, stitch.y]);

        const completedStitchIndex = thread.completedStitches.findIndex(x => x[0] === stitch.x && x[1] === stitch.y);
        thread.completedStitches.splice(completedStitchIndex, 1);
    }
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

    const positions: Record<number, Array<IPosition>> = {};

    for (const stitch of stitches) {
        if (positions[stitch.threadIndex] === undefined)
            positions[stitch.threadIndex] = [];

        positions[stitch.threadIndex].push({
            x: stitch.x,
            y: stitch.y,
        });
    }

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
            stitchesByThread: positions,
        });

        for (const stitch of stitches) {
            const thread = props.project.threads.find(x => x.thread.index === stitch.threadIndex);
            if (thread === undefined)
                continue;

            thread.stitches.push([stitch.x, stitch.y]);

            const completedStitchIndex = thread.completedStitches.findIndex(x => x[0] === stitch.x && x[1] === stitch.y);
            thread.completedStitches.splice(completedStitchIndex, 1);
        }
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
            stitchesByThread: positions,
        });

        for (const stitch of stitches) {
            const thread = props.project.threads.find(x => x.thread.index === stitch.threadIndex);
            if (thread === undefined)
                return;

            thread.completedStitches.push([stitch.x, stitch.y, dayjs()]);

            const stitchIndex = thread.stitches.findIndex(x => x[0] === stitch.x && x[1] === stitch.y);
            thread.stitches.splice(stitchIndex, 1);
        }
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

    const thread = currentProject.palette.value.get(stitch.threadIndex);
    if (!thread || thread.index === 0) {
        hoveredStitch.value = null;
        return;
    }

    hoveredStitch.value = stitch;
};

const onMouseMove = function (event: MouseEvent): void {
    mousePosition.value = Position.at(event.x, event.y).translate(-(component.value.offsetLeft ?? 0), -(component.value.offsetTop ?? 0));

    handleHoveredStitch();
};

const onMouseLeave = function (): void {
    isDragMoving.value = false;
    hoveredStitch.value = null;
};

const onMouseWheel = function (event: WheelEvent): void {
    if (!isMouseOverPattern.value)
        return;

    zoom(event.deltaY, mousePosition.value.x, mousePosition.value.y);
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