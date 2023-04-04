<template>
    <div class="dashboard-view">
        <div class="content-width">
            <h1>Dashboard</h1>
            <h2>Your Projects:</h2>
            <UserMessageComponent ref="userMessageComponent" />
            <div v-if="isLoading">
                <LoadingComponent itemName="projects" />
            </div>
            <div v-else-if="projects?.length === 0">
                <p>No projects yet!</p>
                <p>Visit the marketplace to get your first pattern!</p>
                <RouterLink to="/marketplace">
                    <ButtonComponent>Go to Marketplace</ButtonComponent>
                </RouterLink>
            </div>
            <div v-else class="projects">
                <ProjectComponent :key="project.pattern.reference" v-for="project in projects" :project="project" />
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import UserMessageComponent from '@/components/UserMessage.component.vue';
import ProjectComponent from '@/views/dashboard/components/Project.component.vue';

import { api } from '@/api/api';

import { IProject } from '@/models/Project.model';

const userMessageComponent = ref<InstanceType<typeof UserMessageComponent>>({} as InstanceType<typeof UserMessageComponent>);

const projects = ref<Array<IProject> | null>(null);
const isLoading = ref<boolean>(false);

onMounted(async () => {
    isLoading.value = true;

    const result = await api.projects.getAll();

    isLoading.value = false;

    if (result instanceof Error) {
        userMessageComponent.value.set(result.message, true);
        return;
    }

    projects.value = result;
});
</script>

<style lang="scss">
.dashboard-view {

    .projects {
        display: grid;
        gap: 1rem;
        grid-template-columns: repeat(auto-fill, minmax(450px, 1fr));
    }
}
</style>