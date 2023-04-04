<template>
    <div class="pattern-view flex">
        <div class="loading-container flex-auto">
            <UserMessageComponent ref="userMessageComponent" />
        </div>
        <div class="loading-container" v-if="isLoading">
            <LoadingComponent itemName="pattern" />
        </div>
        <template v-else-if="project !== null">
            <CanvasComponent :project="project" />
            <StitchCountsComponent :project="project" />
        </template>
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';

import UserMessageComponent from '@/components/UserMessage.component.vue';
import CanvasComponent from '@/views/project/components/Canvas.component.vue';
import StitchCountsComponent from '@/views/project/components/StitchCounts.component.vue';

import { api } from '@/api/api';
import { setTitle } from '@/helper/helper';
import { useInput } from '@/use/input/input.use';

import { IGetProject } from '@/models/GetProject.model';

const route = useRoute();
const input = useInput();

const userMessageComponent = ref<InstanceType<typeof UserMessageComponent>>({} as InstanceType<typeof UserMessageComponent>);

const patternReference = route.params.patternReference as string;

const project = ref<IGetProject | null>(null);
const isLoading = ref<boolean>(false);

document.addEventListener('keydown', (event: KeyboardEvent) => {
    input.keysDown.add(event.key);
});

document.addEventListener('keyup', (event: KeyboardEvent) => {
    input.keysDown.delete(event.key);
});

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
});
</script>

<style lang="scss">
.pattern-view {
    height: 100%;

    .loading-container {
        margin: auto;
    }
}
</style>