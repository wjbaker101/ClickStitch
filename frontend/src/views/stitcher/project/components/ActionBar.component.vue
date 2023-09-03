<template>
    <div class="action-bar-component flex gap align-items-center text-centered">
        <div>
            <code v-if="hoveredStitch !== null">[{{ hoveredStitch.x }},{{ hoveredStitch.y }}]</code> - <small>{{ thread?.description }}</small>
        </div>
        <div class="main-items">
            <span class="action-button" @click="onZoomClick(-1)"><IconComponent icon="zoom-in" gap="right" /><span>Zoom In</span></span>
            <span class="action-button" @click="onZoomClick(1)"><IconComponent icon="zoom-out" gap="right" /><span>Zoom Out</span></span>
        </div>
        <div>
            <span class="action-button" @click="onShowModal"><IconComponent icon="arrow-up" gap="right" /><span>More Details</span></span>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

import ProjectThreadsModalComponent from '@/views/stitcher/project/modals/ProjectThreadsModal.component.vue';

import { useCurrentProject } from '@/views/stitcher/project/use/CurrentProject.use';
import { useSharedStitch } from '@/views/stitcher/project/use/SharedStitch';
import { useTransformation } from '@/views/stitcher/project/use/Transformation.use';
import { useModal } from '@wjb/vue/use/modal.use';

import type { IPatternThread } from '@/models/Pattern.model';
import type { IGetProject } from '@/models/GetProject.model';

const props = defineProps<{
    project: IGetProject;
}>();

const sharedStitch = useSharedStitch();
const currentProject = useCurrentProject();
const { width, height, zoom } = useTransformation();
const modal = useModal();

const hoveredStitch = sharedStitch.hoveredStitch;

const thread = computed<IPatternThread | null>(() => {
    if (hoveredStitch.value === null)
        return null;

    return currentProject.palette.value.get(hoveredStitch.value.threadIndex) as IPatternThread;
});

const onZoomClick = function (amount: number): void {
    zoom(amount, width.value / 2, height.value / 2);
};

const onShowModal = function (): void {
    modal.show({
        component: ProjectThreadsModalComponent,
        componentProps: {
            project: props.project,
        },
        style: 'side-right',
    });
};
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