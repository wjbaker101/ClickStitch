<template>
    <ViewComponent class="creator-patterns-view">
        <template #nav>
            <strong>Creator Patterns</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <h2>Your Patterns</h2>
                    <p>Manage your patterns here.</p>
                </CardComponent>
            </section>
            <section>
                <PaginatedContentComponent loadingItemName="patterns" :logic="getPatternsLogic">
                    <template v-if="getPatterns" #content>
                        <ListItemComponent :key="pattern.reference" v-for="pattern in getPatterns.patterns">
                            <div class="flex align-items-center gap">
                                <PatternImageComponent width="150" height="100" class="flex-auto" :image="pattern.bannerImageUrl" />
                                <h3>{{ pattern.title }}</h3>
                            </div>
                            <template #expanded>
                                <FormComponent>
                                    <FormSectionComponent>
                                        <h4>Edit Pattern Details</h4>
                                        <FormInputComponent label="Shop URL">
                                            <input type="text" placeholder="https://etsy.com/shop/beautifulpatternsco/amazing_pattern" :value="pattern.externalShopUrl">
                                        </FormInputComponent>
                                    </FormSectionComponent>
                                </FormComponent>
                            </template>
                        </ListItemComponent>
                    </template>
                </PaginatedContentComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import ListItemComponent from '@/components/ListItem.component.vue';
import PatternImageComponent from '@/components/shared/PatternImage.component.vue';
import PaginatedContentComponent from '@/components/paginated-content/PaginatedContent.component.vue';

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
.creator-patterns-view {}
</style>