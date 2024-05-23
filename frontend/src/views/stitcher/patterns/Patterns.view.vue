<template>
    <ViewComponent class="patterns-view">
        <template #nav>
            <strong>Creator Patterns</strong>
        </template>
        <div class="content-width">
            <UserMessageComponent ref="userMessageComponent" />
            <div v-if="isLoading">
                <LoadingComponent itemName="patterns" />
            </div>
            <ZeroStateComponent v-else-if="patterns?.length === 0" icon="info">
                <p>Could not find any patterns, you must own them all!</p>
                <p>Check back later for more!</p>
            </ZeroStateComponent>
            <template v-else>
                <section>
                    <CardComponent border="top" padded>
                        <h2>What are Creator Patterns?</h2>
                        <p>All patterns here have been made by Creators! You are free to add them to your dashboard where you'll be able to track your progress like normal.</p>
                        <p>A link to their product page will also be available, where you'll be able to purchase the necessary resources for the pattern.</p>
                        <p>These are great for when you are just looking for a new project to start!</p>
                    </CardComponent>
                </section>
                <section>
                    <div class="patterns">
                        <PatternComponent :key="pattern.reference" v-for="pattern in patterns" :pattern="pattern" />
                    </div>
                </section>
            </template>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import UserMessageComponent from '@/components/UserMessageComponent.vue';
import ZeroStateComponent from '@/components/ZeroStateComponent.vue';
import PatternComponent from '@/views/stitcher/patterns/components/Pattern.component.vue';

import { api } from '@/api/api';

import { type IPattern } from '@/models/Pattern.model';

const userMessageComponent = ref<InstanceType<typeof UserMessageComponent>>({} as InstanceType<typeof UserMessageComponent>);

const patterns = ref<Array<IPattern> | null>(null);
const isLoading = ref<boolean>(false);

onMounted(async () => {
    isLoading.value = true;

    const result = await api.patterns.search();

    isLoading.value = false;

    if (result instanceof Error){
        userMessageComponent.value.set(result.message, true);
        return;
    }

    patterns.value = result;
});
</script>

<style lang="scss">
@use '@/style/variables' as *;

.patterns-view {

    .patterns {
        display: grid;
        gap: 1rem;
        grid-template-columns: repeat(auto-fill, minmax(min(370px, 100%), 1fr));
    }
}
</style>