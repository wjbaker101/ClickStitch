<template>
    <div class="project-threads-modal-component flex flex-col gap-4 h-full">
        <div>
            <h2>Actions:</h2>
            <div class="flex gap-small">
                <div class="flex-auto">
                    <BtnComponent @click="onGoToPausePosition">
                        <IconComponent icon="compass" gap="right" />
                        <span class="align-middle">Go to Pause Position</span>
                    </BtnComponent>
                </div>
                <RouterLink v-if="project.project.pattern.user.reference === authDetails?.reference" class="flex-auto" :to="`/projects/${project.project.pattern.reference}/edit`" @click="onEditDetails">
                    <BtnComponent>
                        <IconComponent icon="pencil" gap="right" />
                        <span class="align-middle">Edit Details</span>
                    </BtnComponent>
                </RouterLink>
            </div>
            <h2>Layers:</h2>
            <div>
                <label>
                    <input type="checkbox" v-model="isStitchesVisible"> Stitches
                </label>
                <label>
                    <input type="checkbox" v-model="isBackStitchesVisible"> Back Stitches
                </label>
                <label>
                    <input type="checkbox" v-model="isGridVisible"> Grid
                </label>
            </div>
            <h2>Threads:</h2>
            <div>
                <ThreadDetailsComponent v-for="thread in threads" :thread="thread" :inventory="inventory" :pattern="project.project.pattern" />
            </div>
        </div>
        <div class="flex-auto"><em>* Recommendations based on the pattern's aida count of {{ project.project.pattern.aidaCount }}</em></div>
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import BtnComponent from '@/components/BtnComponent.vue';
import ThreadDetailsComponent from '@/views/stitcher/project/components/ThreadDetailsComponent.vue';

import { api } from '@/api/api';
import { useAuth } from '@/use/auth/Auth.use';
import { useEvents } from '@/use/events/Events.use';
import { useModal } from '@wjb/vue/use/modal.use';
import { useLayers } from '@/views/stitcher/project/use/Layers.use';

import type { IGetProject } from '@/models/GetProject.model';
import type { IGetPatternInventory } from '@/models/GetPatternInventory.model';

const props = defineProps<{
    project: IGetProject;
}>();

const auth = useAuth();
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