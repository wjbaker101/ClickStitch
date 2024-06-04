<template>
    <ViewComponent hideFooter>
        <template #nav>
            <strong class="align-middle">{{ project?.project.pattern.title ?? '-' }}</strong>
            <sub class="ml-2 align-middle percentage-completed">({{ percentageCompleted.toFixed(2) }}%)</sub>
        </template>
        <div v-if="isLoading">
            <LoadingComponent itemName="pattern" />
        </div>
        <div v-else-if="project === null">
            <UserMessageComponent ref="userMessageComponent" />
        </div>
        <template v-else>
            <div>
                <CanvasComponent :project="project" />
            </div>
            <JumpToStitchesComponent />
            <ActionBarComponent :project="project" />
        </template>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';

import LoadingComponent from '@/components/loading/LoadingComponent.vue';
import UserMessageComponent from '@/components/UserMessageComponent.vue';
import CanvasComponent from '@/views/stitcher/project/CanvasComponent.vue';
import JumpToStitchesComponent from '@/views/stitcher/project/components/JumpToStitchesComponent.vue';
import ActionBarComponent from '@/views/stitcher/project/components/ActionBarComponent.vue';

import { api } from '@/api/api';
import { setTitle } from '@/helper/helper';
import { useCurrentProject } from '@/views/stitcher/project/use/CurrentProject.use';

import { type IGetProject } from '@/models/GetProject.model';

const currentProject = useCurrentProject();
const route = useRoute();

const userMessageComponent = ref<InstanceType<typeof UserMessageComponent>>({} as InstanceType<typeof UserMessageComponent>);

const patternReference = route.params.patternReference as string;

const project = ref<IGetProject | null>(null);
const isLoading = ref<boolean>(false);

const percentageCompleted = currentProject.percentageCompleted;

onMounted(async () => {
    isLoading.value = true;

    const result = await api.projects.get(patternReference);

    isLoading.value = false;

    if (result instanceof Error) {
        userMessageComponent.value.set(result.message, true);
        return;
    }

    currentProject.setProject(result);
    project.value = result;

    setTitle(`${project.value.project.pattern.title}`);
});
</script>