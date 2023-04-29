<template>
    <ViewComponent class="project-analytics-view">
        <div class="content-width">
            <h1>Analytics</h1>
            <div class="loading-container flex-auto">
                <UserMessageComponent ref="userMessageComponent" />
            </div>
            <div class="loading-container" v-if="isLoading">
                <LoadingComponent itemName="pattern" />
            </div>
            <div v-else-if="project !== null">
                <CardComponent border="top" padded>
                    <h3>{{ project.project.pattern.title }}</h3>
                    <p><strong>Added to your account: </strong> {{ project.project.purchasedAt }}</p>
                </CardComponent>
                <CardComponent border="top" padded>
                    <p><strong>Completed Stitches: </strong> {{ completedStitches }}</p>
                    <p><strong>Total Stitches: </strong> {{ project.stitches.length }}</p>
                    <p><strong>Remaining Stitches: </strong> {{ project.stitches.length - completedStitches }}</p>
                </CardComponent>
            </div>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';

import UserMessageComponent from '@/components/UserMessage.component.vue';

import { api } from '@/api/api';
import { setTitle } from '@/helper/helper';

import { IGetProject } from '@/models/GetProject.model';
import { computed } from '@vue/reactivity';

const route = useRoute();

const userMessageComponent = ref<InstanceType<typeof UserMessageComponent>>({} as InstanceType<typeof UserMessageComponent>);

const patternReference = route.params.patternReference as string;

const project = ref<IGetProject | null>(null);
const isLoading = ref<boolean>(false);

const completedStitches = computed<number>(() => project.value?.stitches.filter(x => x.stitchedAt !== null).length ?? 0);

onMounted(async () => {
    isLoading.value = true;

    const result = await api.projects.get(patternReference);

    isLoading.value = false;

    if (result instanceof Error) {
        userMessageComponent.value.set(result.message, true);
        return;
    }

    project.value = result;

    setTitle(`Analytics - ${project.value.project.pattern.title}`);
});
</script>

<style lang="scss">
</style>