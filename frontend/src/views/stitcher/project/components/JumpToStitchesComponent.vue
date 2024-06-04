<template>
    <div
        :class="{
            'is-enabled': isEnabled,
        }"
        class="absolute bottom-16 left-1/2 -translate-x-1/2 -translate-y-4 rounded-md border-solid bg-gradient-to-tl
            p-2 shadow-lg backdrop-blur-sm jump-to-stitches-component text-light text-shadow z-[1] border-primary-dark
            from-primary-dark/90 to-primary/90 border-1 opacity-0 pointer-events-none
            [&.is-enabled]:opacity-100 [&.is-enabled]:pointer-events-auto"
    >
        <div class="mb-4 flex items-center gap-4">
            <div class="grow">Jump to Stitch</div>
            <div class="-m-2 cursor-pointer rounded-full p-4 leading-none close hover:bg-primary-dark" @click="onClose">
                <IconComponent icon="cross" />
            </div>
        </div>
        <div class="flex items-center gap-4">
            <BtnComponent class="flex-auto" @click="onNavigate(-1)" type="secondary">
                <IconComponent icon="arrow-left" />
            </BtnComponent>
            <div>
                {{ thread?.thread.name }} (#{{ currentIndex + 1 }})
            </div>
            <BtnComponent class="flex-auto" @click="onNavigate(1)" type="secondary">
                <IconComponent icon="arrow-right" />
            </BtnComponent>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';

import BtnComponent from '@/components/BtnComponent.vue';

import { useEvent, useEvents } from '@/use/events/Events.use';
import { useCurrentProject } from '@/views/stitcher/project/use/CurrentProject.use';

import type { IStartJumpToStitchesEvent } from '@/use/events/types/EventsMap.type';
import type { IThreadDetails } from '@/models/GetProject.model';

const events = useEvents();
const { backStitches, stitches } = useCurrentProject();

const isEnabled  = ref<boolean>(false);
const thread = ref<IThreadDetails | null>(null);

interface IJumpToStitchLocation {
    readonly x: number;
    readonly y: number;
    readonly endX?: number;
    readonly endY?: number;
    readonly type: 'stitch' | 'back-stitch';
    readonly colour: string;
}

const currentStitches = computed<Array<IJumpToStitchLocation>>(() => {
    if (thread.value === null)
        return [];

    const _thread = thread.value.thread;

    const threadIndex = _thread.index;
    const _stitches = stitches.value.filter(x => x.threadIndex === threadIndex);
    const _backStitches = backStitches.value.filter(x => x.threadIndex === threadIndex);

    return _stitches.map<IJumpToStitchLocation>(x => ({
        x: x.x,
        y: x.y,
        type: 'stitch',
        colour: _thread.colour,
    })).concat(_backStitches.map(x => ({
        x: x.startX,
        y: x.startY,
        endX: x.endX,
        endY: x.endY,
        type: 'back-stitch',
        colour: _thread.colour,
    })));
});

const currentIndex = ref<number>(0);

const onNavigate = function (value: number): void {
    if (currentStitches.value.length === 0)
        return;

    currentIndex.value += value;

    if (currentIndex.value < 0)
        currentIndex.value = currentStitches.value.length - 1;

    if (currentIndex.value > currentStitches.value.length - 1)
        currentIndex.value = 0;

    events.publish('JumpToStitch', {
        x: currentStitches.value[currentIndex.value].x,
        y: currentStitches.value[currentIndex.value].y,
        endX: currentStitches.value[currentIndex.value].endX,
        endY: currentStitches.value[currentIndex.value].endY,
        type: currentStitches.value[currentIndex.value].type,
        colour: currentStitches.value[currentIndex.value].colour,
    });
};

const onStart = function (event: IStartJumpToStitchesEvent): void {
    thread.value = event.thread;
    isEnabled.value = true;
    currentIndex.value = 0;

    events.publish('JumpToStitch', {
        x: currentStitches.value[currentIndex.value].x,
        y: currentStitches.value[currentIndex.value].y,
        endX: currentStitches.value[currentIndex.value].endX,
        endY: currentStitches.value[currentIndex.value].endY,
        type: currentStitches.value[currentIndex.value].type,
        colour: currentStitches.value[currentIndex.value].colour,
    });
};

const onClose = function (): void {
    isEnabled.value = false;
    events.publish('EndJumpToStitches', {});
};

useEvent('StartJumpToStitches', onStart);
</script>