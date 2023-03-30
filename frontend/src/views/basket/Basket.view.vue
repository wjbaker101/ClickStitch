<template>
    <div class="basket-view">
        <div class="content-width">
            <h1>Basket</h1>
            <div v-if="basket === null"></div>
            <div v-else-if="basket.items.length === 0" class="text-centered">
                <IconComponent icon="cart" size="huge" />
                <p>No patterns in your basket yet!</p>
                <RouterLink to="/marketplace">
                    <ButtonComponent>Find Patterns</ButtonComponent>
                </RouterLink>
            </div>
            <div v-else>
                <TransitionGroup name="items-transition-group" tag="div" class="basket-items">
                    <CardComponent :key="basketItem.pattern.reference" v-for="basketItem in basket?.items" class="basket-item flex gap align-items-center" border="left">
                        <div class="flex-auto">
                            <img src="@/assets/example-pattern-1.png">
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
    </div>
</template>

<script setup lang="ts">
import { currency } from '@/helper/helper';
import { useMarketplace } from '@/use/marketplace/Marketplace.use';

import { IBasketItem } from '@/models/Basket.model';

const marketplace = useMarketplace();

const basket = marketplace.get();

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

        img {
            background-color: #efefef;
            border-radius: var(--wjb-border-radius);
            vertical-align: middle;

            @include shadow-small();
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