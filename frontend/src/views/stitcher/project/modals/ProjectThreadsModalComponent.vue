<template>
    <div class="grid h-full gap-4 project-threads-modal-component">
        <div>
            <h2 class="mb-4 text-2xl font-bold">Actions:</h2>
            <BtnComponent
                @click="onGoToPausePosition"
                class="mr-2"
                :disabled="pausePosition === null"
                :title="pausePosition !== null ? '' : `You'll need pause on a stitch before being able to use this button`"
            >
                <CompassIcon class="mr-2" />
                <span class="align-middle">Go to Pause Position</span>
            </BtnComponent>
            <RouterLink v-if="project.project.pattern.user.reference === authDetails?.reference" :to="`/projects/${project.project.pattern.reference}/edit`" @click="onEditDetails">
                <BtnComponent>
                    <PencilIcon class="mr-2" />
                    <span class="align-middle">Edit Details</span>
                </BtnComponent>
            </RouterLink>
            <h2 class="my-4 text-2xl font-bold">Layers:</h2>
            <div>
                <CheckBoxComponent v-model="isStitchesVisible" />
                <span class="mr-2 pl-1 align-middle">Stitches</span>
                <CheckBoxComponent v-model="isBackStitchesVisible" />
                <span class="mr-2 pl-1 align-middle">Back Stitches</span>
                <CheckBoxComponent v-model="isGridVisible" />
                <span class="mr-2 pl-1 align-middle">Grid</span>
            </div>
            <h2 class="my-4 text-2xl font-bold">Threads:</h2>
            <div>
                <ThreadDetailsComponent v-for="thread in threads" :thread="thread" :inventory="inventory" :pattern="project.project.pattern" />
            </div>
        </div>
        <div class="self-end"><em>* Recommendations based on the pattern's aida count of {{ project.project.pattern.aidaCount }}</em></div>
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import { CompassIcon, PencilIcon } from 'lucide-vue-next';
import BtnComponent from '@/components/BtnComponent.vue';
import CheckBoxComponent from '@/components/inputs/CheckBoxComponent.vue';
import ThreadDetailsComponent from '@/views/stitcher/project/components/ThreadDetailsComponent.vue';

import { api } from '@/api/api';
import { useAuth } from '@/use/auth/Auth.use';
import { useCurrentProject } from '@/views/stitcher/project/use/CurrentProject.use';
import { useEvents } from '@/use/events/Events.use';
import { useModal } from '@/components/modals/Modal.use';
import { useLayers } from '@/views/stitcher/project/use/Layers.use';

import type { IGetProject } from '@/models/GetProject.model';
import type { IGetPatternInventory } from '@/models/GetPatternInventory.model';

const props = defineProps<{
    project: IGetProject;
}>();

const auth = useAuth();
const { pausePosition } = useCurrentProject();
const events = useEvents();
const modal = useModal();
const layers = useLayers();

const isStitchesVisible = layers.stitches;
const isBackStitchesVisible = layers.backStitches;
const isGridVisible = layers.grid;

const authDetails = auth.details;

const threads = props.project.threads;

const inventory = ref<IGetPatternInventory | null>(null);

const onGoToPausePosition = function () {
    events.publish('GoToPausePosition', {});
    modal.hide();
};

const onEditDetails = function () {
    modal.hide();
};

onMounted(async () => {
    const inventoryResult = await api.patterns.getInventory(props.project.project.pattern.reference);
    if (inventoryResult instanceof Error)
        return;

    inventory.value = inventoryResult;
});
</script>

<style lang="scss">
.project-threads-modal-component {

    .list-item-component {
        padding: 0;
        padding-right: 0.5rem;

        & + .list-item-component {
            margin-top: 0.125rem;
        }

        .more-content {
            margin-right: -0.25rem;
        }

        &.is-expanded {
            .more-content {
                margin-top: 0;
            }
        }
    }
}
</style>