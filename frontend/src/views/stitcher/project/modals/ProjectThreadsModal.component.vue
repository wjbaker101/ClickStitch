<template>
    <div class="project-threads-modal-component">
        <h2>Threads</h2>
        <div>
            <ThreadDetailsComponent v-for="thread in threads" :thread="thread" />
        </div>
    </div>
</template>

<script setup lang="ts">
import ThreadDetailsComponent from '../components/ThreadDetails.component.vue';

import type { IGetProject } from '@/models/GetProject.model';

const props = defineProps<{
    project: IGetProject;
}>();

const threads = props.project.threads;
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