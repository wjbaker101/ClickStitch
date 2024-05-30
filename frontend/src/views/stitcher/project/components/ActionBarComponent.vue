<template>
    <div class="absolute inset-0 top-auto flex items-center gap-4 p-1 text-center backdrop-blur-sm action-bar-component text-light [text-shadow:1px_1px_rgba(0,0,0,.6)] shadow-lg border-[1px] border-solid border-primary-dark rounded-t-md">
        <div>
            <div>{{ percentageCompleted.toFixed(2) }}%</div>
        </div>
        <div class="centre-container">
            <div class="flex items-center centre-button" @click="onShowModal">
                <IconComponent icon="info" />
            </div>
        </div>
        <div>
            <ActiveStitchComponent />
        </div>
    </div>
</template>

<script setup lang="ts">
import ProjectThreadsModalComponent from '@/views/stitcher/project/modals/ProjectThreadsModalComponent.vue';
import ActiveStitchComponent from '@/views/stitcher/project/components/ActiveStitchComponent.vue';

import { useCurrentProject } from '@/views/stitcher/project/use/CurrentProject.use';
import { useModal } from '@wjb/vue/use/modal.use';

import type { IGetProject } from '@/models/GetProject.model';

const props = defineProps<{
    project: IGetProject;
}>();

const { percentageCompleted } = useCurrentProject();
const modal = useModal();

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
.action-bar-component {
    background: linear-gradient(
        -5deg,
        color-mix(in srgb, var(--wjb-primary-dark) 90%, transparent),
        color-mix(in srgb, var(--wjb-primary) 90%, transparent),
    );
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