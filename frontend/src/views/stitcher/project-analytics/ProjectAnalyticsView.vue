<template>
    <ViewComponent class="project-analytics-view">
        <template #nav>
            <strong>Analytics</strong>
        </template>
        <div class="flex-auto loading-container">
            <UserMessageComponent ref="userMessageComponent" />
        </div>
        <div class="loading-container" v-if="isLoading">
            <LoadingComponent itemName="pattern" />
        </div>
        <div v-else-if="analytics !== null">
            <div class="mb-4 grid gap-4 top-grid grid-cols-[1fr_2fr]">
                <CardComponent class="flex items-center text-center">
                    <img width="250" height="166" :src="analytics.bannerImageUrl" class="h-auto w-full rounded-md align-middle image">
                </CardComponent>
                <CardComponent border="top" padded>
                    <h2>{{ analytics.title }}</h2>
                    <p><strong>Added At: </strong> {{ analytics.purchasedAt }}</p>
                    <p><strong>Total Stitches: </strong> {{ formatNumber(analytics.totalStitches) }}</p>
                    <p>
                        <RouterLink :to="`/projects/${patternReference}`">
                            <BtnComponent>
                                <IconComponent icon="external-link" gap="right" />
                                <span class="align-middle">Continue Stitching!</span>
                            </BtnComponent>
                        </RouterLink>
                    </p>
                </CardComponent>
            </div>
            <CardComponent border="top" padded class="mb-4">
                <h3>At a Glance:</h3>
                <p><strong>Remaining Stitches: </strong> {{ formatNumber(analytics.remainingStitches) }}</p>
                <p><strong>Completed Stitches: </strong> {{ formatNumber(analytics.completedStitches) }} ({{ completedPercentage.toFixed(2) }}%)</p>
            </CardComponent>
            <CardComponent border="top" padded>
                <Bar
                    :data="{
                        labels: analytics.data.headings,
                        datasets: [{
                            data: analytics.data.values,
                            label: 'Stitches per Day',
                            backgroundColor: '#30a390',
                        }],
                    }"
                    :options="{}"
                />
            </CardComponent>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale
} from 'chart.js'
import { Bar } from 'vue-chartjs'

import LoadingComponent from '@/components/loading/LoadingComponent.vue';
import UserMessageComponent from '@/components/UserMessageComponent.vue';

import { api } from '@/api/api';
import { formatNumber, setTitle } from '@/helper/helper';

import { type IGetAnalytics } from '@/models/GetAnalytics.model';
import BtnComponent from '@/components/BtnComponent.vue';

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

const route = useRoute();

const userMessageComponent = ref<InstanceType<typeof UserMessageComponent>>({} as InstanceType<typeof UserMessageComponent>);

const patternReference = route.params.patternReference as string;

const analytics = ref<IGetAnalytics | null>(null);
const isLoading = ref<boolean>(false);

const completedPercentage = computed<number>(() => (analytics.value?.completedStitches ?? 1) / (analytics.value?.totalStitches ?? 1) * 100);

onMounted(async () => {
    isLoading.value = true;

    const result = await api.projects.getAnalytics(patternReference);

    isLoading.value = false;

    if (result instanceof Error) {
        userMessageComponent.value.set(result.message, true);
        return;
    }

    analytics.value = result;

    setTitle(`Analytics - ${analytics.value.title}`);
});
</script>