<template>
    <CardComponent class="pattern-component" border="top">
        <div class="text-centered">
            <img src="@/assets/templar-knight.png">
        </div>
        <div class="description flex align-items-center gap-small">
            <div>
                <strong>{{ pattern.title }}</strong>
                <br>
                {{ currency(pattern.price) }}
            </div>
            <div class="flex-auto">
                <ButtonComponent class="add-to-basket-button mini" :title="hoverText" @click="onAddToBasket(pattern)" :disabled="isInBasket">
                    <IconComponent v-if="isInBasket" icon="tick" />
                    <IconComponent v-else icon="cart" />
                </ButtonComponent>
            </div>
        </div>
    </CardComponent>
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
const hoverText = computed<string>(() => isInBasket.value ? 'Already in basket!' : 'Add to basket');

const onAddToBasket = async function (pattern: IPattern): Promise<void> {
    await marketplace.addItem(pattern);
};
</script>

<style lang="scss">
@use '@/style/variables' as *;

.pattern-component {
    padding: 1rem;
    background-color: var(--wjb-background-colour-dark);

    img {
        border-radius: var(--wjb-border-radius);
        vertical-align: middle;

        @include shadow-small();
    }

    .description {
        margin-top: 1rem;
    }

    .add-to-basket-button {
        &[disabled] {
            opacity: 0.5;
        }
    }

    @include shadow-small();
}
</style>