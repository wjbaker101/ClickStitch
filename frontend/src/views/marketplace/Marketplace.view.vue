<template>
    <div class="marketplace-view">
        <div class="content-width">
            <h1>Marketplace</h1>
            <p>Add patterns to your account and stitch away!</p>
            <UserMessageComponent ref="userMessageComponent" />
            <div v-if="isLoading">
                <LoadingComponent itemName="patterns" />
            </div>
            <div v-else-if="patterns?.length === 0">
                <p>No patterns were recieved, you must own them all!</p>
            </div>
            <div v-else class="patterns">
                <PatternComponent :key="pattern.reference" v-for="pattern in patterns" :pattern="pattern" />
            </div>
        </div>
        <div class="basket-container">
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
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import UserMessageComponent from '@/components/UserMessage.component.vue';
import PatternComponent from '@/views/marketplace/components/Pattern.component.vue';

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
        userMessageComponent.value.set(result.message);
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
        grid-template-columns: repeat(auto-fill, minmax(260px, 1fr));
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