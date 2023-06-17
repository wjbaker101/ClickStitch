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
            <section v-if="getPatterns">
                <ListItemComponent :key="pattern.reference" v-for="pattern in getPatterns.patterns">
                    <div class="flex align-items-center gap">
                        <PatternImageComponent width="150" class="flex-auto" :image="pattern.bannerImageUrl" />
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
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import ListItemComponent from '@/components/ListItem.component.vue';

import { api } from '@/api/api';
import { IGetCreatorPatterns } from '@/models/GetCreatorPatterns.model';
import PatternImageComponent from '@/components/shared/PatternImage.component.vue';

const getPatterns = ref<IGetCreatorPatterns | null>(null);

onMounted(async () => {
    const selfResult = await api.creators.getSelf();
    if (selfResult instanceof Error)
        return;

    if (selfResult === null) {
        return;
    }

    const getPatternsResult = await api.creators.getPatterns(selfResult.reference, 5, 1);
    if (getPatternsResult instanceof Error)
        return;

    getPatterns.value = getPatternsResult;
});
</script>

<style lang="scss">
.creator-patterns-view {}
</style>