<template>
    <ViewComponent class="creator-view">
        <template #nav>
            <strong>Creator</strong>
        </template>
        <LoadingComponent v-if="isLoading" itemName="creator" />
        <div v-else-if="creator !== null" class="content-width">
            <CardComponent border="top" padded>
                <h2>
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
            </CardComponent>
            <PaginatedContentComponent loadingItemName="pattens" :pageSize="10" :logic="loadPatterns">
                <div class="patterns">
                    <DisplayPatternComponent v-for="pattern in patterns" :pattern="pattern" />
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

const loadPatterns = async function (pageNumber: number, pageSize: number): Promise<IPagination | Error> {
    isPatternsLoading.value = true;
    const response = await api.creators.searchPatterns(creatorReference, pageSize, pageNumber);
    isPatternsLoading.value = false;

    if (response instanceof Error)
        return response;

    patterns.value = response.patterns;

    return response.pagination;
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

    .patterns {
        display: grid;
        gap: 3rem;
        grid-template-columns: repeat(auto-fill, minmax(min(350px, 100%), 1fr));
    }
}
</style>