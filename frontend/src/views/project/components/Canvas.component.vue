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
            <div
                v-if="stitchSelectStart && stitchSelectEnd"
                class="selected-stitches-wrapper"
                :style="{
                }"
            >
                <div
                    class="selected-stitches"
                    :style="{
                        'width': `${stitchSize * (stitchSelectEnd.x - stitchSelectStart.x + 1)}px`,
                        'height': `${stitchSize * (stitchSelectEnd.y - stitchSelectStart.y + 1)}px`,
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
        <div class="debug">
            <div>w: {{ width.toFixed(0) }} h: {{ height.toFixed(0) }}</div>
            <div>mouse | x: {{ mousePosition.x }} y: {{ mousePosition.y }}</div>
            <div>scale {{ scale.toFixed(1) }}</div>
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

import { isDark } from '@/helper/helper';
import { Position } from '@/class/Position.class';
import { useMouse } from '@/views/project/use/Mouse.use';
import { useSharedStitch } from '@/views/project/use/SharedStitch';
import { useStitch } from '@/views/project/use/Stitch.use';
import { useTransformation } from '@/views/project/use/Transformation.use';

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
const graphics = computed<CanvasRenderingContext2D>(() => canvasElement.value.getContext('2d') as CanvasRenderingContext2D);

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
    }

    graphics.value.strokeStyle = '#666';
    graphics.value.lineWidth = 2;

    for (let x = 0; x < props.project.project.pattern.width; ++x) {
        graphics.value.beginPath();
        graphics.value.moveTo(x * baseStitchSize, 0);
        graphics.value.lineTo(x * baseStitchSize, props.project.project.pattern.height * baseStitchSize);
        graphics.value.closePath();
        graphics.value.stroke();
    }

    for (let y = 0; y < props.project.project.pattern.height; ++y) {
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

const onMouseDown = function (event: MouseEvent): void {
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
    scale.value = Math.max(0.1, Math.min(1, scale.value * factor));

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
        position: relative;
        transition: transform 0.1s;
        transform-origin: top left;
    }

    canvas {
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