<template>
    <div class="project-threads-modal-component">
        <h2>Actions:</h2>
        <p>
            <ButtonComponent @click="onGoToPausePosition">
                <IconComponent icon="compass" gap="right" />
                <span>Go to Pause Position</span>
            </ButtonComponent>
        </p>
        <h2>Threads:</h2>
        <div>
            <ThreadDetailsComponent v-for="thread in threads" :thread="thread" />
        </div>
    </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';

import ThreadDetailsComponent from '@/views/stitcher/project/components/ThreadDetails.component.vue';

import { api } from '@/api/api';
import { useEvents } from '@/use/events/Events.use';
import { useModal } from '@wjb/vue/use/modal.use';

import type { IGetProject } from '@/models/GetProject.model';

const props = defineProps<{
    project: IGetProject;
}>();

const events = useEvents();
const modal = useModal();

const threads = props.project.threads;

const onGoToPausePosition = function () {
    events.publish('GoToPausePosition', {});
    modal.hide();
};

onMounted(async () => {
    const inventory = await api.patterns.getInventory(props.project.project.pattern.reference);
    if (inventory instanceof Error)
        return;

    console.log(inventory);
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