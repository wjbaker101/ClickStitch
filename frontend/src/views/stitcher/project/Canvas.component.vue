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
        @contextmenu="onOpenContextMenu"
    >
        <div class="canvas-wrapper"
            :style="{
                'transform': `translate(${offset.x}px, ${offset.y}px) scale(${scale})`,
            }"
        >
            <canvas
                class="pattern-canvas"
                :class="{
                    'is-highlighting': highlightedThreadIndex !== null,
                    'is-hidden': !isVisible
                }"
                ref="patternCanvas"
                :width="project.project.pattern.width * baseStitchSize"
                :height="project.project.pattern.height * baseStitchSize"
            >
            </canvas>
            <GridLayerComponent />
            <HighlightedThreadLayerComponent
                :baseStitchSize="baseStitchSize"
            />
            <CompletedStitchesLayerComponent
                :baseStitchSize="baseStitchSize"
            />
            <BackStitchesLayerComponent
                :baseStitchSize="baseStitchSize"
            />
            <JumpedStitchLayerComponent
                :baseStitchSize="baseStitchSize"
            />
            <SelectedStitchesLayerComponent
                v-if="stitchSelectStart !== null && stitchSelectEnd !== null"
                :stitchSelectStart="stitchSelectStart"
                :stitchSelectEnd="stitchSelectEnd"
                :stitchSize="baseStitchSize"
            />
            <PausePositionLayerComponent
                :stitchSize="baseStitchSize"
            />
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
import { computed, onMounted, ref } from 'vue';

import CompletedStitchesLayerComponent from '@/views/stitcher/project/layers/CompletedStitchesLayer.component.vue';
import HighlightedThreadLayerComponent from '@/views/stitcher/project/layers/HighlightedThreadLayer.component.vue';
import JumpedStitchLayerComponent from '@/views/stitcher/project/layers/JumpedStitchLayer.component.vue';
import SelectedStitchesLayerComponent from '@/views/stitcher/project/layers/SelectedStitchesLayer.component.vue';
import PausePositionLayerComponent from '@/views/stitcher/project/layers/PausePositionLayer.component.vue';
import BackStitchesLayerComponent from '@/views/stitcher/project/layers/BackStitchesLayer.component.vue';
import GridLayerComponent from '@/views/stitcher/project/layers/GridLayer.component.vue';

import { api } from '@/api/api';
import { isDark } from '@/helper/helper';
import { Position } from '@/class/Position.class';
import { useLayers } from '@/views/stitcher/project/use/Layers.use';
import { useCurrentProject } from '@/views/stitcher/project/use/CurrentProject.use';
import { useHammer } from '@/views/stitcher/project/use/Hammer.use';
import { useMouse } from '@/views/stitcher/project/use/Mouse.use';
import { useSharedStitch } from '@/views/stitcher/project/use/SharedStitch';
import { useStitch } from '@/views/stitcher/project/use/Stitch.use';
import { useTransformation } from '@/views/stitcher/project/use/Transformation.use';
import { useEvents } from '@/use/events/Events.use';
import { useHighlightedThread } from '@/views/stitcher/project/use/HighlightedThread.use';
import { useCanvasElement } from '@/views/stitcher/project/use/CanvasElement.use';
import { factory } from '@/components/context-menu/ContextMenuFactory';

import { type IGetProject } from '@/models/GetProject.model';
import { type IPatternThread } from '@/models/Pattern.model';

const props = defineProps<{
    project: IGetProject;
}>();

const component = ref<HTMLDivElement>({} as HTMLDivElement);

const layers = useLayers();
const currentProject = useCurrentProject();
const events = useEvents();
const sharedStitch = useSharedStitch();
const highlightedThread = useHighlightedThread();
const { mousePosition, prevMousePosition, isDragMoving, isDragSelecting, selectStart, selectEnd } = useMouse();
const { width, height, offset, scale, zoom } = useTransformation();
const pinchStart = ref<number>(1);
const pinchDiff = ref<number>(1);

const { baseStitchSize, scaledStitchSize, mouseStitchPosition, isMouseOverPattern, stitchSelectStart, stitchSelectEnd, viewportToStitchPosition } = useStitch();

const patternCanvas = ref<HTMLCanvasElement>({} as HTMLCanvasElement);
const { graphics } = useCanvasElement(patternCanvas);

const isVisible = layers.stitches;
const hoveredStitch = sharedStitch.hoveredStitch;
const highlightedThreadIndex = highlightedThread.threadIndex;

const canvasWidth = computed<number>(() => props.project.project.pattern.width * scaledStitchSize.value);
const canvasHeight = computed<number>(() => props.project.project.pattern.height * scaledStitchSize.value);

const resizeObserver = new ResizeObserver(entries => {
    const entry = entries[0];

    width.value = entry.contentRect.width;
    height.value = entry.contentRect.height;
});

onMounted(() => {
    highlightedThreadIndex.value = null;
    resizeObserver.observe(component.value);

    width.value = component.value.offsetWidth;
    height.value = component.value.offsetHeight;

    offset.value = Position.at(width.value / 2 - canvasWidth.value / 2, height.value / 2 - canvasHeight.value / 2);
    events.publish('GoToPausePosition', {});

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
    }

    const hammer = useHammer(component);

    hammer.on('double-tap', e => {
        handleHoveredStitch();

        if (e.target.classList.contains('back-stitch-line'))
            return;

        events.publish('PatternDoubleClick', {});
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
});

const onClick = function (): void {
    selectStart.value = null;
    selectEnd.value = null;
};

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

    const stitch = currentProject.stitchPositionLookup.value.get(mouseStitchPosition.value.x, mouseStitchPosition.value.y);
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

const onOpenContextMenu = function (event: MouseEvent): void {
    const x = event.pageX;
    const y = event.pageY;

    const stitchPosition = viewportToStitchPosition(Position.at(x, y));
    const isPausePosition = stitchPosition.equals(currentProject.pausePosition.value);

    events.publish('OpenContextMenu', {
        x,
        y,
        schema: {
            header: 'Actions:',
            items: [
                factory.item(isPausePosition ? 'Clear Pause Position' : 'Pause Here', async () => {
                    if (isPausePosition) {
                        await api.projects.unpause(props.project.project.pattern.reference);
                        currentProject.pausePosition.value = null;

                        return;
                    }

                    await api.projects.pause(props.project.project.pattern.reference, {
                        x: stitchPosition.x,
                        y: stitchPosition.y,
                    });

                    currentProject.pausePosition.value = Position.at(stitchPosition.x, stitchPosition.y);
                }),
            ],
        },
    });

    event.preventDefault();
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

        & > * {
            position: absolute;
            inset: 0;
            transition: transform 0.1s, opacity 0.1s;

            &.is-hidden {
                opacity: 0;
            }
        }
    }

    canvas {
        pointer-events: none;
        border-radius: var(--wjb-border-radius);
        image-rendering: crisp-edges;
        image-rendering: -moz-crisp-edges;
        image-rendering: -webkit-optimize-contrast;
        image-rendering: optimize-contrast;
        -ms-interpolation-mode: nearest-neighbor;
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

    .pattern-canvas {
        transition: transform 0.1s, opacity 0.2s;

        &.is-highlighting {
            opacity: 0.2;
        }
    }
}
</style>