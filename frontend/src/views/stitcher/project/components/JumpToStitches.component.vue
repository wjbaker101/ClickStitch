<template>
    <div class="jump-to-stitches-component" :class="{ 'is-enabled': isEnabled }">
        <div class="header flex gap align-items-center">
            <div>Jump to Stitch</div>
            <div class="close flex-auto" @click="onClose">
                <IconComponent icon="cross" />
            </div>
        </div>
        <div class="flex gap align-items-center">
            <ButtonComponent class="secondary flex-auto" @click="onNavigate(-1)">
                <IconComponent icon="arrow-left" />
            </ButtonComponent>
            <div>
                {{ thread?.thread.name }} (#{{ currentIndex + 1 }})
            </div>
            <ButtonComponent class="secondary flex-auto" @click="onNavigate(1)">
                <IconComponent icon="arrow-right" />
            </ButtonComponent>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref } from 'vue';

import { useEvent, useEvents } from '@/use/events/Events.use';

import type { IStartJumpToStitchesEvent } from '@/use/events/types/EventsMap.type';
import type { IThreadDetails } from '@/models/GetProject.model';

const events = useEvents();

const isEnabled  = ref<boolean>(false);
const thread = ref<IThreadDetails | null>(null);

const stitches = computed<Array<[number, number]>>(() => thread.value?.stitches ?? []);

const currentIndex = ref<number>(0);

const onNavigate = function (value: number): void {
    if (stitches.value.length === 0)
        return;

    currentIndex.value += value;

    if (currentIndex.value < 0)
        currentIndex.value = stitches.value.length - 1;

    if (currentIndex.value > stitches.value.length - 1)
        currentIndex.value = 0;

    events.publish('JumpToStitch', {
        x: stitches.value[currentIndex.value][0],
        y: stitches.value[currentIndex.value][1],
    });
};

const onStart = function (event: IStartJumpToStitchesEvent): void {
    thread.value = event.thread;
    isEnabled.value = true;
    currentIndex.value = 0;

    events.publish('JumpToStitch', {
        x: stitches.value[currentIndex.value][0],
        y: stitches.value[currentIndex.value][1],
    });
};

const onClose = function (): void {
    isEnabled.value = false;
    events.publish('EndJumpToStitches', {});
};

useEvent('StartJumpToStitches', onStart);
</script>

<style lang="scss">
.jump-to-stitches-component {
    position: absolute;
    left: 50%;
    bottom: 4.5rem;
    translate: -50% 0;
    padding: 0.5rem;
    line-height: 1em;
    background-color: var(--wjb-primary);
    background: linear-gradient(-5deg, rgba(24, 105, 92, 0.9), rgba(34, 146, 127, 0.9));
    backdrop-filter: blur(2px);
    color: var(--wjb-light);
    text-shadow: 1px 1px rgba(0, 0, 0, 0.6);
    box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 12px 24px -12px rgba(0, 0, 0, 0.5);
    border: 1px solid var(--wjb-primary-dark);
    border-radius: var(--wjb-border-radius);
    opacity: 0;
    pointer-events: none;
    z-index: 1;

    &.is-enabled {
        opacity: 1;
        pointer-events: all;
    }

    .header {
        margin-bottom: 0.5rem;
    }

    .close {
        padding: 0.5rem;
        border-radius: var(--wjb-border-radius);
        cursor: pointer;

        &:hover {
            background-color: var(--wjb-primary-dark);
        }
    }
}
</style>