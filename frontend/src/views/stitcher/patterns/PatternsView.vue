<template>
    <ViewComponent>
        <template #nav>
            <strong>Creator Patterns</strong>
        </template>
        <UserMessageComponent ref="userMessageComponent" />
        <div v-if="isLoading">
            <LoadingComponent itemName="patterns" />
        </div>
        <div v-else-if="patterns?.length === 0" class="text-center">
            <InfoIcon class="!size-24" />
            <p>Could not find any patterns, you must own them all!</p>
            <p>Check back later for more!</p>
        </div>
        <template v-else>
            <CardComponent border="top" padded class="mb-4">
                <h2 class="mb-4 text-2xl font-bold">What are Creator Patterns?</h2>
                <p>All patterns here have been made by Creators! You are free to add them to your dashboard where you'll be able to track your progress like normal.</p>
                <p>A link to their product page will also be available, where you'll be able to purchase the necessary resources for the pattern.</p>
                <p>These are great for when you are just looking for a new project to start!</p>
            </CardComponent>
            <div class="grid gap-12 grid-cols-for-patterns">
                <DisplayPatternComponent :key="pattern.reference" v-for="pattern in patterns" :pattern="pattern" :userHasPattern="false" />
            </div>
        </template>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import { InfoIcon } from 'lucide-vue-next';
import LoadingComponent from '@/components/loading/LoadingComponent.vue';
import UserMessageComponent from '@/components/UserMessageComponent.vue';
import DisplayPatternComponent from '@/components/display-pattern/DisplayPatternComponent.vue';

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