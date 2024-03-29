<template>
    <div class="action-bar-component flex gap align-items-center text-centered">
        <div>
            <div>{{ percentage.toFixed(2) }}%</div>
        </div>
        <div class="centre-container">
            <div class="centre-button flex align-items-center" @click="onShowModal">
                <IconComponent icon="info" />
            </div>
        </div>
        <div>
            <div class="hovered-stitch">
                <code v-if="hoveredStitch !== null">[{{ hoveredStitch.x }},{{ hoveredStitch.y }}]</code> - <small>{{ thread?.description }}</small>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

import ProjectThreadsModalComponent from '@/views/stitcher/project/modals/ProjectThreadsModal.component.vue';

import { useCurrentProject } from '@/views/stitcher/project/use/CurrentProject.use';
import { useSharedStitch } from '@/views/stitcher/project/use/SharedStitch';
import { useModal } from '@wjb/vue/use/modal.use';

import type { IPatternThread } from '@/models/Pattern.model';
import type { IGetProject } from '@/models/GetProject.model';

const props = defineProps<{
    project: IGetProject;
}>();

const sharedStitch = useSharedStitch();
const currentProject = useCurrentProject();
const modal = useModal();

const hoveredStitch = sharedStitch.hoveredStitch;

const percentage = computed<number>(() => {
    const complete = props.project.threads.reduce((total, x) => total + x.completedStitches.length, 0);
    const incomplete = props.project.threads.reduce((total, x) => total + x.stitches.length, 0);

    return complete / (complete + incomplete) * 100;
});

const thread = computed<IPatternThread | null>(() => {
    if (hoveredStitch.value === null)
        return null;

    return currentProject.palette.value.get(hoveredStitch.value.threadIndex) as IPatternThread;
});

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
    padding: 0.25rem;
    background-color: var(--wjb-primary);
    background: linear-gradient(
        -5deg,
        color-mix(in srgb, var(--wjb-primary-dark) 90%, transparent),
        color-mix(in srgb, var(--wjb-primary) 90%, transparent),
    );
    backdrop-filter: blur(2px);
    color: var(--wjb-light);
    text-shadow: 1px 1px rgba(0, 0, 0, 0.6);
    box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 12px 24px -12px rgba(0, 0, 0, 0.5);
    border: 1px solid var(--wjb-primary-dark);
    border-top-right-radius: var(--wjb-border-radius);
    border-top-left-radius: var(--wjb-border-radius);
    z-index: 1;

    @media screen and (max-width: 720px) {
        .hovered-stitch,
        .main-items {
            display: none;
        }
    }

    .centre-container {
        flex: 0 0 4rem;
    }

    .centre-button {
        width: 4rem;
        position: absolute;
        top: 0;
        left: 50%;
        translate: -50% -50%;
        aspect-ratio: 1;
        border-radius: 50%;
        margin: auto;
        background: linear-gradient(
            -5deg,
            color-mix(in srgb, var(--wjb-primary-dark) 90%, transparent),
            color-mix(in srgb, var(--wjb-primary) 90%, transparent),
        );
        border: 1px solid var(--wjb-primary-dark);
        backdrop-filter: blur(2px);
        color: var(--wjb-light);
        text-shadow: 1px 1px rgba(0, 0, 0, 0.6);
        box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 12px 24px -12px rgba(0, 0, 0, 0.5);
        cursor: pointer;
    }

    .action-button {
        display: inline-block;
        padding: 0.25rem 0.5rem;
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