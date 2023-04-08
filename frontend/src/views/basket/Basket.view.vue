<template>
    <ViewComponent class="basket-view">
        <div class="content-width">
            <h1>Basket</h1>
            <div v-if="basket === null">
                <LoadingComponent itemName="basket" />
            </div>
            <ZeroStateComponent v-else-if="basket.items.length === 0" icon="cart">
                <p>No patterns in your basket yet!</p>
                <RouterLink to="/marketplace">
                    <ButtonComponent>
                        <IconComponent icon="arrow-left" gap="right" />
                        <span>Find Patterns</span>
                    </ButtonComponent>
                </RouterLink>
            </ZeroStateComponent>
            <div v-else>
                <TransitionGroup name="items-transition-group" tag="div" class="basket-items">
                    <CardComponent :key="basketItem.pattern.reference" v-for="basketItem in basket?.items" class="basket-item flex gap align-items-center" border="left">
                        <div class="flex-auto">
                            <PatternImageComponent :pattern="basketItem.pattern" />
                        </div>
                        <div>
                            <strong>{{ basketItem.pattern.title }}</strong>
                        </div>
                        <div class="flex-auto">
                            {{ currency(basketItem.pattern.price) }}
                        </div>
                        <div class="flex-auto">
                            <DeleteButtonComponent onlyIcon @delete="onDelete(basketItem)" />
                        </div>
                    </CardComponent>
                </TransitionGroup>
                <div class="pay-container text-centered">
                    <ButtonComponent @click="onPay">Pay {{ currency(basket?.totalPrice ?? 0) }}</ButtonComponent>
                </div>
            </div>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import PatternImageComponent from '@/components/shared/PatternImage.component.vue';

import { currency } from '@/helper/helper';
import { useMarketplace } from '@/use/marketplace/Marketplace.use';

import { IBasketItem } from '@/models/Basket.model';
import ZeroStateComponent from '@/components/ZeroState.component.vue';

const marketplace = useMarketplace();

const basket = marketplace.basket;

const onDelete = async function (basketItem: IBasketItem): Promise<void> {
    await marketplace.removeItem(basketItem.pattern);
};

const onPay = async function (): Promise<void> {
    await marketplace.complete();
};
</script>

<style lang="scss">
@use '@/style/variables' as *;

.basket-view {

    .basket-item {
        padding: 1rem;

        & + .basket-item {
            margin-top: 1rem;
        }
    }

    .pay-container {
        margin-top: 2rem;
    }

    .items-transition-group-enter,
    .items-transition-group-leave-to {
        opacity: 0;
    }

    .items-transition-group-leave-to {
        transform: scale(0.5);
    }
}
</style>