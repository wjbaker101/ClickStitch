<template>
    <div class="jump-to-stitches-component" :class="{ 'is-enabled': isEnabled }">
        Jump to Stitches
        <div class="flex gap align-items-center">
            <ButtonComponent class="secondary flex-auto" @click="onNavigate(-1)">
                <IconComponent icon="arrow-left" />
            </ButtonComponent>
            <div>
                {{ thread?.thread.name }}
            </div>
            <ButtonComponent class="secondary flex-auto" @click="onNavigate(1)">
                <IconComponent icon="arrow-right" />
            </ButtonComponent>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';
import { useJumpToStitches } from '../use/JumpToStitch.use';

const jumpToStitches = useJumpToStitches();

const isEnabled  = jumpToStitches.isEnabled;
const thread = jumpToStitches.thread;

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
};
</script>

<style lang="scss">
.jump-to-stitches-component {
    position: absolute;
    left: 50%;
    bottom: 2.5rem;
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
}
</style>