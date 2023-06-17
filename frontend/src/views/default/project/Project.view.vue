<template>
    <ViewComponent class="pattern-view">
        <template #nav>
            <strong>{{ project?.project.pattern.title ?? '-' }}</strong>
            <sub class="percentage-completed">({{ percentage?.toFixed(2) ?? '-' }}%)</sub>
        </template>
        <div class="loading-container flex-auto">
            <UserMessageComponent ref="userMessageComponent" />
        </div>
        <div class="loading-container" v-if="isLoading">
            <LoadingComponent itemName="pattern" />
        </div>
        <template v-else-if="project !== null">
            <CanvasComponent :project="project" @openInformation="isInformationOpen = !isInformationOpen" />
            <StitchCountsComponent :project="project" :isOpen="isInformationOpen" />
        </template>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, onUnmounted, ref } from 'vue';
import { useRoute } from 'vue-router';

import UserMessageComponent from '@/components/UserMessage.component.vue';
import CanvasComponent from '@/views/default/project/components/Canvas.component.vue';
import StitchCountsComponent from '@/views/default/project/components/StitchCounts.component.vue';

import { api } from '@/api/api';
import { setTitle } from '@/helper/helper';
import { useSharedStitch } from '@/views/default/project/use/SharedStitch';

import { IGetProject } from '@/models/GetProject.model';

const route = useRoute();
const sharedStitch = useSharedStitch();

const userMessageComponent = ref<InstanceType<typeof UserMessageComponent>>({} as InstanceType<typeof UserMessageComponent>);

const patternReference = route.params.patternReference as string;

const project = ref<IGetProject | null>(null);
const isLoading = ref<boolean>(false);
const isInformationOpen = ref<boolean>(false);

const percentage = sharedStitch.percentageCompleted;

onMounted(async () => {
    isLoading.value = true;

    const result = await api.projects.get(patternReference);

    isLoading.value = false;

    if (result instanceof Error) {
        userMessageComponent.value.set(result.message, true);
        return;
    }

    project.value = result;

    setTitle(`${project.value.project.pattern.title}`);

    document.body.classList.add('no-scroll');
});

onUnmounted(() => {
    document.body.classList.remove  ('no-scroll');
});
</script>

<style lang="scss">
.pattern-view {
    height: 100%;

    .loading-container {
        margin: auto;
    }

    .percentage-completed {
        padding-left: 0.25rem;
        color: #ddd;
    }
}
</style>