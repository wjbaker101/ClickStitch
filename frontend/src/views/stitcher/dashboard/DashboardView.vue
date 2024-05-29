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
                        <CardComponent class="flex number align-items-center"></CardComponent>
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
                        <CardComponent class="flex number align-items-center"></CardComponent>
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
            <div v-else class="grid gap-12 grid-cols-for-patterns">
                <DisplayPatternComponent :key="project.pattern.reference" v-for="project in projects" :pattern="project.pattern" :project="project" :userHasPattern="false" />
            </div>
        </div>
        <div v-if="!isLoading && projects !== null && projects.length > 0" class="fixed right-1/2 bottom-4 translate-x-1/2 md:right-4 md:translate-x-0">
            <RouterLink to="/patterns/new">
                <ButtonComponent class="!rounded-full">
                    <IconComponent icon="plus" gap="right" />
                    <span>New Pattern</span>
                </ButtonComponent>
            </RouterLink>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import UserMessageComponent from '@/components/UserMessageComponent.vue';
import ZeroStateComponent from '@/components/ZeroStateComponent.vue';
import DisplayPatternComponent from '@/components/display-pattern/DisplayPatternComponent.vue';

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
}
</style>