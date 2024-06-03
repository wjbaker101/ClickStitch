<template>
    <ViewComponent class="creator-patterns-view">
        <template #nav>
            <strong>Creator Patterns</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded class="flex items-center gap-4">
                    <div>
                        <h2>Your Patterns</h2>
                        <p>Manage your patterns here.</p>
                        <RouterLink to="/patterns/new">
                            <BtnComponent>
                                <IconComponent icon="plus" gap="right" />
                                <span class="align-middle">New Pattern</span>
                            </BtnComponent>
                        </RouterLink>
                    </div>
                    <div class="flex-auto">
                        <CountDisplayComponent :count="getPatterns?.pagination.totalCount" description="patterns" />
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

import BtnComponent from '@/components/BtnComponent.vue';
import PaginatedContentComponent from '@/components/paginated-content/PaginatedContentComponent.vue';
import CreatorPatternComponent from '@/views/creator/patterns/components/CreatorPatternComponent.vue';

import { api } from '@/api/api';

import { type ISearchCreatorPatterns } from '@/models/GetCreatorPatterns.model';
import { type ICreator } from '@/models/Creator.model';
import { type IPagination } from '@/models/Pagination.model';
import CountDisplayComponent from '@/components/count-display/CountDisplayComponent.vue';

const getPatterns = ref<ISearchCreatorPatterns | null>(null);

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

    const getPatternsResult = await api.creators.searchPatterns(self.value.reference, pageSize, pageNumber);
    if (getPatternsResult instanceof Error)
        return new Error('Unable to retrieve patterns.');

    getPatterns.value = getPatternsResult;

    return getPatterns.value.pagination;
};
</script>