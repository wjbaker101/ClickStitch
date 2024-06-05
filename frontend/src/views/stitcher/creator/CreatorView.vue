<template>
    <ViewComponent>
        <template #nav>
            <strong>Creator</strong>
        </template>
        <LoadingComponent v-if="isLoading" itemName="creator" />
        <CardComponent v-else-if="creator !== null" border="top" padded class="grid grid-flow-col place-content-between items-center gap-4">
            <div>
                <h2 class="mb-4 text-2xl font-bold">
                    <UserIcon class="!size-12 mr-2" />
                    <span class="align-middle">{{ creator.name }}</span>
                </h2>
                <p>Creator since: {{ creator.createdAt.format('MMMM YYYY') }}</p>
                <a :href="creator.storeUrl" target="_blank">
                    <BtnComponent>
                        <ExternalLinkIcon class="mr-2" />
                        <span class="align-middle text-light">Visit their Shop!</span>
                    </BtnComponent>
                </a>
            </div>
            <div>
                <CountDisplayComponent :count="totalPatternCount" description="patterns" />
            </div>
        </CardComponent>
        <PaginatedContentComponent loadingItemName="pattens" :pageSize="10" :logic="loadPatterns">
            <div class="grid gap-12 grid-cols-for-patterns">
                <DisplayPatternComponent v-for="pattern in patterns" :pattern="pattern" :userHasPattern="doesUserHavePattern(pattern.reference)" />
            </div>
        </PaginatedContentComponent>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onBeforeMount, ref } from 'vue';
import { useRoute } from 'vue-router';

import { UserIcon, ExternalLinkIcon } from 'lucide-vue-next';
import BtnComponent from '@/components/BtnComponent.vue';
import LoadingComponent from '@/components/loading/LoadingComponent.vue';
import PaginatedContentComponent from '@/components/paginated-content/PaginatedContentComponent.vue';
import DisplayPatternComponent from '@/components/display-pattern/DisplayPatternComponent.vue';

import { api } from '@/api/api';

import type { ICreator } from '@/models/Creator.model';
import type { IPattern } from '@/models/Pattern.model';
import type { IPagination } from '@/models/Pagination.model';
import CountDisplayComponent from '@/components/count-display/CountDisplayComponent.vue';

const route = useRoute();

const creatorReference = route.params.creatorReference as string;

const creator = ref<ICreator | null>(null);
const isLoading = ref<boolean>(false);

const isPatternsLoading = ref<boolean>(false);
const patterns = ref<Array<IPattern>>([]);
const totalPatternCount = ref<number | null>(null);
const projectPatternReferencesForUser = ref<Array<string>>([]);

const loadPatterns = async function (pageNumber: number, pageSize: number): Promise<IPagination | Error> {
    isPatternsLoading.value = true;
    const response = await api.creators.searchPatterns(creatorReference, pageSize, pageNumber);
    isPatternsLoading.value = false;

    if (response instanceof Error)
        return response;

    patterns.value = response.patterns;
    projectPatternReferencesForUser.value = response.projectPatternReferencesForUser;
    totalPatternCount.value = response.pagination.totalCount;

    return response.pagination;
};

const doesUserHavePattern = function (patternReference: string): boolean {
    return projectPatternReferencesForUser.value.some(x => x === patternReference);
};

onBeforeMount(async () => {
    isLoading.value = true;
    const response = await api.creators.getByReference(creatorReference);
    isLoading.value = false;

    if (response instanceof Error)
        return;

    creator.value = response;
});
</script>