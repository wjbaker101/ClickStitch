<template>
    <ViewComponent class="pattern-view" hideFooter>
        <template #nav>
            <strong>{{ project?.project.pattern.title ?? '-' }}</strong>
            <sub class="percentage-completed">({{ percentage?.toFixed(2) ?? '-' }}%)</sub>
        </template>
        <div class="loading-container" v-if="isLoading">
            <LoadingComponent itemName="pattern" />
        </div>
        <div v-else-if="project === null" class="loading-container flex-auto">
            <UserMessageComponent ref="userMessageComponent" />
        </div>
        <template v-else>
            <div class="canvas-wrapper">
                <CanvasComponent :project="project" />
            </div>
            <JumpToStitchesComponent />
            <ActionBarComponent :project="project" />
        </template>
    </ViewComponent>
</template>

<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref } from 'vue';
import { useRoute } from 'vue-router';

import UserMessageComponent from '@/components/UserMessage.component.vue';
import CanvasComponent from '@/views/stitcher/project/Canvas.component.vue';
import JumpToStitchesComponent from '@/views/stitcher/project/components/JumpToStitches.component.vue';
import ActionBarComponent from '@/views/stitcher/project/components/ActionBar.component.vue';

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

const percentage = computed<number>(() => {
    let incomplete = 0;
    let complete = 0;

    for (const stitch of currentProject.stitches.value) {
        if (stitch.stitchedAt === null)
            incomplete++;
        else
            complete++;
    }

    return complete / incomplete * 100;
});

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

    document.body.classList.add('no-scroll');
});

onUnmounted(() => {
    document.body.classList.remove  ('no-scroll');
});
</script>

<style lang="scss">
.pattern-view {
    height: 100%;

    .page-content {
        padding-top: 0 !important;
        min-height: calc(100vh) !important;
        display: flex;
    }

    .loading-container {
        margin: auto;
    }

    .percentage-completed {
        padding-left: 0.25rem;
        color: #ddd;
    }

    .canvas-wrapper {
        position: relative;
        flex: 1;
    }
}
</style>