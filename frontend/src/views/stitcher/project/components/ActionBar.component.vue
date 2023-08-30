<template>
    <div class="action-bar-component flex gap align-items-center text-centered">
        <div></div>
        <div class="main-items">
            <span class="action-button"><IconComponent icon="zoom-in" gap="right" /><span>Zoom In</span></span>
            <span class="action-button"><IconComponent icon="zoom-out" gap="right" /><span>Zoom Out</span></span>
        </div>
        <div>
            <code v-if="hoveredStitch !== null">[{{ hoveredStitch.x }},{{ hoveredStitch.y }}]</code> - <small>{{ thread?.description }}</small>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

import { useCurrentProject } from '../use/CurrentProject.use';
import { useSharedStitch } from '../use/SharedStitch';

import type { IPatternThread } from '@/models/Pattern.model';

const sharedStitch = useSharedStitch();
const currentProject = useCurrentProject();

const hoveredStitch = sharedStitch.hoveredStitch;

const thread = computed<IPatternThread | null>(() => {
    if (hoveredStitch.value === null)
        return null;

    return currentProject.palette.value.get(hoveredStitch.value.threadIndex) as IPatternThread;
})
</script>

<style lang="scss">
@use '@/style/variables' as *;

.action-bar-component {
    inset: 0;
    top: auto;
    position: absolute;
    padding: 0.5rem;
    line-height: 1em;
    background-color: var(--wjb-primary);
    background: linear-gradient(
        -5deg,
        transparentize($primary-dark, 0.1),
        transparentize($primary, 0.1),
    );
    backdrop-filter: blur(2px);
    color: var(--wjb-light);
    text-shadow: 1px 1px rgba(0, 0, 0, 0.6);
    box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 12px 24px -12px rgba(0, 0, 0, 0.5);
    border: 1px solid var(--wjb-primary-dark);
    border-top-right-radius: var(--wjb-border-radius);
    border-top-left-radius: var(--wjb-border-radius);
    z-index: 1;

    .action-button {
        padding: 0.25rem;
        border-radius: var(--wjb-border-radius);
        cursor: pointer;
        user-select: none;

        span {
            vertical-align: middle;
        }

        & + .action-button {
            margin-left: 0.5rem;
        }

        &:hover {
            background-color: var(--wjb-tertiary);
        }

        &:active {
            background-color: var(--wjb-tertiary-dark);
        }
    }
}
</style>