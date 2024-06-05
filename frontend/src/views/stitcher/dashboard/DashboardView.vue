<template>
    <ViewComponent>
        <template #nav>
            <strong>Dashboard</strong>
        </template>
        <h2 class="mb-4 text-2xl font-bold">Your Projects:</h2>
        <UserMessageComponent ref="userMessageComponent" />
        <div v-if="isLoading">
            <LoadingComponent itemName="projects" />
        </div>
        <div v-else-if="projects?.length === 0" class="text-center">
            <InfoIcon class="!size-24 mr-2" />
            <h2 class="mb-8 mt-2 text-2xl font-bold">No projects yet!</h2>
            <NumberedSectionComponent class="max-w-3xl">
                <NumberedCardComponent>
                    <p>Upload your first pattern now!</p>
                    <RouterLink to="/patterns/new">
                        <BtnComponent>
                            <PlusIcon class="mr-2" />
                            <span class="align-middle">New Pattern</span>
                        </BtnComponent>
                    </RouterLink>
                </NumberedCardComponent>
                <NumberedCardComponent>
                    <p>Looking for inspiration?</p>
                    <RouterLink to="/patterns">
                        <BtnComponent>
                            <DownloadIcon class="mr-2" />
                            <span class="align-middle">View Creator Patterns</span>
                        </BtnComponent>
                    </RouterLink>
                </NumberedCardComponent>
            </NumberedSectionComponent>
        </div>
        <div v-else class="grid gap-12 grid-cols-for-patterns">
            <DisplayPatternComponent :key="project.pattern.reference" v-for="project in projects" :pattern="project.pattern" :project="project" :userHasPattern="false" />
        </div>
        <div v-if="!isLoading && projects !== null && projects.length > 0" class="fixed right-1/2 bottom-4 translate-x-1/2 md:right-4 md:translate-x-0">
            <RouterLink to="/patterns/new">
                <BtnComponent class="!rounded-full shadow-xl">
                    <PlusIcon class="mr-2" />
                    <span class="align-middle">New Pattern</span>
                </BtnComponent>
            </RouterLink>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import { InfoIcon, PlusIcon, DownloadIcon } from 'lucide-vue-next';
import BtnComponent from '@/components/BtnComponent.vue';
import LoadingComponent from '@/components/loading/LoadingComponent.vue';
import UserMessageComponent from '@/components/UserMessageComponent.vue';
import DisplayPatternComponent from '@/components/display-pattern/DisplayPatternComponent.vue';
import NumberedSectionComponent from '@/components/numbered-card/NumberedSectionComponent.vue';
import NumberedCardComponent from '@/components/numbered-card/NumberedCardComponent.vue';

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