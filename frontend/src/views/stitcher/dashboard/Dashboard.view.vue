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
                <h2>No projects yet!</h2>
                <div class="new-project-options">
                    <CardComponent padded border="top">
                        <CardComponent class="number flex align-items-center"></CardComponent>
                        <div class="flex-auto">
                            <p>Upload your first pattern now!</p>
                            <RouterLink to="/patterns/new">
                                <ButtonComponent>
                                    <IconComponent icon="plus" gap="right" />
                                    <span>New Pattern</span>
                                </ButtonComponent>
                            </RouterLink>
                        </div>
                    </CardComponent>
                    <CardComponent padded border="top">
                        <CardComponent class="number flex align-items-center"></CardComponent>
                        <div class="flex-auto">
                            <p>Looking for inspiration?</p>
                            <RouterLink to="/patterns">
                                <ButtonComponent>
                                    <IconComponent icon="download" gap="right" />
                                    View Creator Patterns
                                </ButtonComponent>
                            </RouterLink>
                        </div>
                    </CardComponent>
                </div>
            </ZeroStateComponent>
            <div v-else class="projects">
                <ProjectComponent :key="project.pattern.reference" v-for="project in projects" :project="project" />
            </div>
        </div>
        <div v-if="!isLoading && projects !== null && projects.length > 0" class="floating-action-button">
            <RouterLink to="/patterns/new">
                <ButtonComponent>
                    <IconComponent icon="plus" gap="right" />
                    <span>New Pattern</span>
                </ButtonComponent>
            </RouterLink>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import UserMessageComponent from '@/components/UserMessage.component.vue';
import ZeroStateComponent from '@/components/ZeroState.component.vue';
import ProjectComponent from '@/views/stitcher/dashboard/components/Project.component.vue';

import { api } from '@/api/api';

import { type IProject } from '@/models/Project.model';

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

    .new-project-options {
        max-width: 720px;
        margin: 4rem auto 0 auto;
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
        justify-items: stretch;
        gap: 3rem;
        counter-reset: index;

        & > * {
            position: relative;
        }
    }

    .number {
        width: 4rem;
        position: absolute;
        padding: 1rem;
        top: 0;
        left: 50%;
        translate: -50% -50%;
        aspect-ratio: 1;
        border-radius: 50%;
        justify-content: center;
        font-weight: bold;
        font-size: 2rem;
        color: var(--wjb-secondary);
        border: 2px solid var(--wjb-primary);

        &::before {
            counter-increment: index;
            content: counter(index);
        }
    }

    .projects {
        display: grid;
        gap: 2rem;
        grid-template-columns: repeat(auto-fill, minmax(min(450px, 100%), 1fr));
    }

    .floating-action-button {
        position: fixed;
        bottom: 1rem;
        right: 1rem;

        button {
            border-radius: 9999px;
        }

        @media screen and (max-width: 600px) {
            right: 50%;
            translate: 50% 0;
        }
    }
}
</style>