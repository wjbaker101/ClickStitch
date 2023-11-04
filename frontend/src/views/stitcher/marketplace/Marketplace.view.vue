<template>
    <ViewComponent class="marketplace-view">
        <template #nav>
            <strong>Patterns</strong>
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
                        <p>Once patterns are added to your account, you can start tracking your progress!</p>
                        <p><strong>Make sure to purchase the digital patterns from the <LinkComponent :href="etsyStoreUrl" external>Etsy Store</LinkComponent> first.</strong></p>
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

import UserMessageComponent from '@/components/UserMessage.component.vue';
import ZeroStateComponent from '@/components/ZeroState.component.vue';
import PatternComponent from '@/views/stitcher/marketplace/components/Pattern.component.vue';

import { etsyStoreUrl } from '@/data/data';
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

.marketplace-view {

    .patterns {
        display: grid;
        gap: 1rem;
        grid-template-columns: repeat(auto-fill, minmax(min(370px, 100%), 1fr));
    }
}
</style>