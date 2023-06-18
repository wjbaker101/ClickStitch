<template>
    <ViewComponent class="creator-patterns-view">
        <template #nav>
            <strong>Creator Patterns</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded class="flex align-items-center gap">
                    <div>
                        <h2>Your Patterns</h2>
                        <p>Manage your patterns here.</p>
                    </div>
                    <div class="flex-auto">
                        Total: <strong class="total-count">{{ getPatterns?.pagination.totalCount ?? '-' }}</strong>
                    </div>
                </CardComponent>
            </section>
            <section>
                <PaginatedContentComponent loadingItemName="patterns" :logic="getPatternsLogic">
                    <CreatorPatternComponent :key="pattern.reference" v-for="pattern in getPatterns?.patterns" :pattern="pattern" />
                </PaginatedContentComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import PaginatedContentComponent from '@/components/paginated-content/PaginatedContent.component.vue';
import CreatorPatternComponent from '@/views/creator/patterns/components/CreatorPattern.component.vue';

import { api } from '@/api/api';

import { IGetCreatorPatterns } from '@/models/GetCreatorPatterns.model';
import { ICreator } from '@/models/Creator.model';
import { IPagination } from '@/models/Pagination.model';

const getPatterns = ref<IGetCreatorPatterns | null>(null);

const self = ref<ICreator | null>(null);

const getPatternsLogic = async function (pageNumber: number, pageSize: number): Promise<IPagination | Error> {
    if (self.value === null) {
        const selfResult = await api.creators.getSelf();
        if (selfResult instanceof Error)
            return new Error('Unable to retrieve creator.');

        self.value = selfResult;
    }

    if (self.value == null)
        return new Error('Unable to retrieve creator.');

    const getPatternsResult = await api.creators.getPatterns(self.value.reference, pageSize, pageNumber);
    if (getPatternsResult instanceof Error)
        return new Error('Unable to retrieve patterns.');

    getPatterns.value = getPatternsResult;

    return getPatterns.value.pagination;
};
</script>

<style lang="scss">
.creator-patterns-view {

    .total-count {
        $size: 2rem;

        width: $size;
        line-height: $size;
        display: inline-block;
        aspect-ratio: 1;
        border-radius: 50%;
        background-color: var(--wjb-secondary);
        color: var(--wjb-light);
        text-align: center;
    }
}
</style>