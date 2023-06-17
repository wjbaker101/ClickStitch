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
        <div v-if="false" class="basket-container">
            <RouterLink to="/basket">
                <ButtonComponent>
                    <IconComponent icon="cart" />
                </ButtonComponent>
            </RouterLink>
            <div class="item-count" :class="{ 'is-visible': basket?.items.length ?? 0 > 0 }">
                <small>
                    <strong>{{ basket?.items.length }}</strong>
                </small>
            </div>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import UserMessageComponent from '@/components/UserMessage.component.vue';
import ZeroStateComponent from '@/components/ZeroState.component.vue';
import PatternComponent from '@/views/default/marketplace/components/Pattern.component.vue';

import { etsyStoreUrl } from '@/data/data';
import { api } from '@/api/api';
import { useMarketplace } from '@/use/marketplace/Marketplace.use';

import { IPattern } from '@/models/Pattern.model';

const marketplace = useMarketplace();

const userMessageComponent = ref<InstanceType<typeof UserMessageComponent>>({} as InstanceType<typeof UserMessageComponent>);

const basket = marketplace.basket;

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

    .basket-container {
        position: fixed;
        inset: auto 3rem 3rem auto;

        button {
            width: 4rem;
            border-radius: 50%;
            aspect-ratio: 1;

            @include shadow-large();
        }

        .item-count {
            width: 2rem;
            text-align: center;
            line-height: 2rem;
            position: absolute;
            inset: -0.25rem auto auto -0.25rem;
            aspect-ratio: 1;
            background-color: var(--wjb-tertiary);
            border-radius: 50%;
            opacity: 0;
            pointer-events: none;

            &.is-visible {
                opacity: 1;
                pointer-events: all;
            }

            @include shadow-large();
        }
    }
}
</style>