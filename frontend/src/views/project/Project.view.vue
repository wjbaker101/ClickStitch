<template>
    <div class="pattern-view flex">
        <CanvasComponent :canvas="project.canvas" />
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
import { useCurrentProject } from '@/use/current-project/CurrentProject.use';

import { IProject } from '@/models/Project.model';

const route = useRoute();
const input = useInput();
const currentProject = useCurrentProject();

const patternReference = route.params.patternReference as string;

// const project = ref<IProject | null>(null)

const project = currentProject.project;

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

    // project.value = result;

    // setTitle(`${project.value.pattern.title}`);
});
</script>

<style lang="scss">
.pattern-view {
    height: 100%;
}
</style>