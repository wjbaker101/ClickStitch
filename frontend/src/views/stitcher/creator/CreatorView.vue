<template>
    <ViewComponent class="creator-view">
        <template #nav>
            <strong>Creator</strong>
        </template>
        <LoadingComponent v-if="isLoading" itemName="creator" />
        <div v-else-if="creator !== null" class="content-width">
            <CardComponent border="top" padded class="flex gap align-items-center">
                <div>
                    <h2 class="creator-title">
                        <IconComponent icon="user" size="large" gap="right" />
                        <span class="name">{{ creator.name }}</span>
                    </h2>
                    <p>
                        Creator since: {{ creator.createdAt.format('MMMM YYYY') }}
                    </p>
                    <LinkComponent :href="creator.storeUrl">
                        <ButtonComponent>
                            Visit their Shop!
                        </ButtonComponent>
                    </LinkComponent>
                </div>
                <div class="flex-auto">
                    <div class="total-count">{{ totalPatternCount }}</div> patterns
                </div>
            </CardComponent>
            <PaginatedContentComponent loadingItemName="pattens" :pageSize="10" :logic="loadPatterns">
                <div class="grid gap-12 grid-cols-for-patterns">
                    <DisplayPatternComponent v-for="pattern in patterns" :pattern="pattern" :userHasPattern="doesUserHavePattern(pattern.reference)" />
                </div>
            </PaginatedContentComponent>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onBeforeMount, ref } from 'vue';
import { useRoute } from 'vue-router';

import PaginatedContentComponent from '@/components/paginated-content/PaginatedContentComponent.vue';
import DisplayPatternComponent from '@/components/display-pattern/DisplayPatternComponent.vue';

import { api } from '@/api/api';

import type { ICreator } from '@/models/Creator.model';
import type { IPattern } from '@/models/Pattern.model';
import type { IPagination } from '@/models/Pagination.model';

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

<style lang="scss">
.creator-view {

    .name {
        vertical-align: middle;
    }

    .creator-title {
        margin: 0;
    }

    .total-count {
        width: 2.5rem;
        aspect-ratio: 1;
        display: inline-block;
        padding: 0.5rem;
        margin-right: 0.25rem;
        background-color: var(--wjb-secondary);
        border-radius: 50%;
        color: var(--wjb-light);
        text-align: center;
        box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);
    }
}
</style>