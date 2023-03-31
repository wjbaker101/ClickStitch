<template>
    <div class="pattern-view flex">
        <div class="loading-container" v-if="project === null">
            <LoadingComponent itemName="pattern" />
        </div>
        <CanvasComponent v-else :project="project" />
        <StitchCountsComponent />
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';

import CanvasComponent from '@/views/project/components/Canvas.component.vue';
import StitchCountsComponent from '@/views/project/components/StitchCounts.component.vue';

import { api } from '@/api/api';
import { setTitle } from '@/helper/helper';
import { useInput } from '@/use/input/input.use';

import { IGetProject } from '@/models/GetProject.model';

const route = useRoute();
const input = useInput();

const patternReference = route.params.patternReference as string;

const project = ref<IGetProject | null>(null)

document.addEventListener('keydown', (event: KeyboardEvent) => {
    input.keysDown.add(event.key);
});

document.addEventListener('keyup', (event: KeyboardEvent) => {
    input.keysDown.delete(event.key);
});

onMounted(async () => {
    const result = await api.projects.get(patternReference);
    if (result instanceof Error)
        return;

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