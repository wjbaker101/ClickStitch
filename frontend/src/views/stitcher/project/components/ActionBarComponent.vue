<template>
    <div
        class="absolute inset-0 top-auto flex items-center gap-4 rounded-t-md border-solid bg-gradient-to-tl p-1
        text-center shadow-lg backdrop-blur-sm z-[1] action-bar-component text-light text-shadow border-1
        border-primary-dark from-primary-dark/90 to-primary/90"
    >
        <div class="grow basis-1">
            <div>{{ percentageCompleted.toFixed(2) }}%</div>
        </div>
        <div class="w-16 shrink">
            <div
                @click="onShowModal"
                class="absolute -top-1 left-1/2 m-auto grid place-items-center -translate-x-1/2 -translate-y-1/2 items-center rounded-full
                    border-solid bg-gradient-to-tl backdrop-blur-sm size-16 from-primary-dark/90 to-primary/90 border-1
                    border-primary-dark text-light text-shadow shadow-md cursor-pointer outline-2 outline-dashed
                    outline-transparent hover:outline-secondary"
            >
                <InfoIcon class="drop-shadow-icon" />
            </div>
        </div>
        <div class="grow basis-1">
            <ActiveStitchComponent class="hidden md:block" />
        </div>
    </div>
</template>

<script setup lang="ts">
import { InfoIcon } from 'lucide-vue-next';
import ProjectThreadsModalComponent from '@/views/stitcher/project/modals/ProjectThreadsModalComponent.vue';
import ActiveStitchComponent from '@/views/stitcher/project/components/ActiveStitchComponent.vue';

import { useCurrentProject } from '@/views/stitcher/project/use/CurrentProject.use';
import { useModal } from '@/components/modals/Modal.use';

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
    });
};
</script>