<template>
    <ViewComponent class="dashboard-view">
        <template #nav>
            <strong>Dashboard</strong>
        </template>
        <div class="content-width">
            <h2>Your Projects:</h2>
            <UserMessageComponent ref="userMessageComponent" />
            <div v-if="isLoading">
                <LoadingComponent itemName="projects" />
            </div>
            <ZeroStateComponent v-else-if="projects?.length === 0" icon="info">
                <p>No projects yet!</p>
                <p>Visit the marketplace to get your first pattern!</p>
                <RouterLink to="/marketplace">
                    <ButtonComponent>Go to Marketplace</ButtonComponent>
                </RouterLink>
            </ZeroStateComponent>
            <div v-else class="projects">
                <ProjectComponent :key="project.pattern.reference" v-for="project in projects" :project="project" />
            </div>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import UserMessageComponent from '@/components/UserMessage.component.vue';
import ZeroStateComponent from '@/components/ZeroState.component.vue';
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
        gap: 2rem;
        grid-template-columns: repeat(auto-fill, minmax(min(450px, 100%), 1fr));
    }
}
</style>