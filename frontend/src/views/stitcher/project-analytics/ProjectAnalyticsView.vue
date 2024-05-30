<template>
    <ViewComponent class="project-analytics-view">
        <template #nav>
            <strong>Analytics</strong>
        </template>
        <div class="content-width">
            <div class="loading-container flex-auto">
                <UserMessageComponent ref="userMessageComponent" />
            </div>
            <div class="loading-container" v-if="isLoading">
                <LoadingComponent itemName="pattern" />
            </div>
            <div v-else-if="analytics !== null">
                <section class="top-grid">
                    <CardComponent class="flex align-items-center text-centered">
                        <img class="image" width="250" height="166" :src="analytics.bannerImageUrl">
                    </CardComponent>
                    <CardComponent border="top" padded>
                        <h2>{{ analytics.title }}</h2>
                        <p><strong>Added At: </strong> {{ analytics.purchasedAt }}</p>
                        <p><strong>Total Stitches: </strong> {{ formatNumber(analytics.totalStitches) }}</p>
                        <p>
                            <RouterLink :to="`/projects/${patternReference}`" @click.native="closeModal">
                                <BtnComponent>
                                    <IconComponent icon="external-link" gap="right" />
                                    <span class="align-middle">Continue Stitching!</span>
                                </BtnComponent>
                            </RouterLink>
                        </p>
                    </CardComponent>
                </section>
                <section>
                    <CardComponent border="top" padded>
                        <h3>At a Glance:</h3>
                        <p><strong>Remaining Stitches: </strong> {{ formatNumber(analytics.remainingStitches) }}</p>
                        <p><strong>Completed Stitches: </strong> {{ formatNumber(analytics.completedStitches) }} ({{ completedPercentage.toFixed(2) }}%)</p>
                    </CardComponent>
                </section>
                <section>
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
                </section>
            </div>
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

import UserMessageComponent from '@/components/UserMessageComponent.vue';

import { api } from '@/api/api';
import { formatNumber, setTitle } from '@/helper/helper';
import { useModal } from '@wjb/vue/use/modal.use';

import { type IGetAnalytics } from '@/models/GetAnalytics.model';
import BtnComponent from '@/components/BtnComponent.vue';

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

const route = useRoute();
const modal = useModal();

const userMessageComponent = ref<InstanceType<typeof UserMessageComponent>>({} as InstanceType<typeof UserMessageComponent>);

const patternReference = route.params.patternReference as string;

const analytics = ref<IGetAnalytics | null>(null);
const isLoading = ref<boolean>(false);

const completedPercentage = computed<number>(() => (analytics.value?.completedStitches ?? 1) / (analytics.value?.totalStitches ?? 1) * 100);

const closeModal = function (): void {
    modal.hide();
};

const plByMonth = [
  { name: 'Jan', pl: 1000, avg: 500, inc: 300 },
  { name: 'Feb', pl: 2000, avg: 900, inc: 400 },
  { name: 'Apr', pl: 400, avg: 400, inc: 500 },
  { name: 'Mar', pl: 3100, avg: 1300, inc: 700 },
  { name: 'May', pl: 200, avg: 100, inc: 200 },
  { name: 'Jun', pl: 600, avg: 400, inc: 300 },
  { name: 'Jul', pl: 500, avg: 90, inc: 100 }
]

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

<style lang="scss">
.project-analytics-view {
    .top-grid {
        display: grid;
        gap: 1rem;
        grid-template-columns: 1fr 2fr;
    }

    .image {
        width: 100%;
        height: auto;
        vertical-align: middle;
        border-radius: var(--wjb-border-radius);
    }
}
</style>