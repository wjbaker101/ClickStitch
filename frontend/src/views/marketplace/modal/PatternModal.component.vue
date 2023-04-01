<template>
    <div class="pattern-modal-component">
        <h2>{{ pattern.title }}</h2>
        <div class="flex gap align-items-center">
            <img class="flex-auto" :src="pattern.thumbnailUrl">
            <div>
                <p><strong>{{ currency(pattern.price) }}</strong></p>
                <p>{{ pattern.width }}&times;{{ pattern.height }}</p>
            </div>
        </div>
        <p class="text-centered">
            <ButtonComponent :class="{ 'danger': isInBasket }" @click="onAddToBasket">
                <IconComponent icon="cart" gap="right" />
                <span v-if="isInBasket">Remove from basket</span>
                <span v-else>Add to basket</span>
            </ButtonComponent>
        </p>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

import { currency } from '@/helper/helper';
import { useMarketplace } from '@/use/marketplace/Marketplace.use';

import { IPattern } from '@/models/Pattern.model';

const props = defineProps<{
    pattern: IPattern;
}>();

const marketplace = useMarketplace();

const patternsInBasket = marketplace.patternReferences;
const isInBasket = computed<boolean>(() => patternsInBasket.value.has(props.pattern.reference));

const onAddToBasket = async function (): Promise<void> {
    if (isInBasket.value) {
        await marketplace.removeItem(props.pattern);
        return;
    }

    await marketplace.addItem(props.pattern);
};
</script>

<style lang="scss">
@use '@/style/variables' as *;

.pattern-modal-component {
    width: 720px;
    max-width: 100%;

    img {
        border-radius: var(--wjb-border-radius);
        vertical-align: middle;

        @include shadow-small();
    }
}
</style>