<template>
    <CardComponent class="pattern-component" border="top" hoverable @click="onClick">
        <div class="text-centered">
            <PatternImageComponent :pattern="pattern" />
        </div>
        <div class="description flex align-items-center gap-small">
            <div>
                <strong>{{ pattern.title }}</strong>
                <br>
                {{ currency(pattern.price) }}
            </div>
            <div class="flex-auto">
                <ButtonComponent class="add-to-basket-button mini" :title="hoverText" @click.stop="onAddToBasket(pattern)" :disabled="isInBasket">
                    <IconComponent v-if="isInBasket" icon="tick" />
                    <IconComponent v-else icon="cart" />
                </ButtonComponent>
            </div>
        </div>
    </CardComponent>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRouter } from 'vue-router';

import PatternImageComponent from '@/components/shared/PatternImage.component.vue';
import PatternModal from '@/views/marketplace/modal/PatternModal.component.vue';
import PatternImagesModal from '@/views/marketplace/modal/PatternImagesModal.component.vue';

import { currency } from '@/helper/helper';
import { useMarketplace } from '@/use/marketplace/Marketplace.use';
import { useModal } from '@wjb/vue/use/modal.use';
import { usePopup } from '@wjb/vue/use/popup.use';

import { IPattern } from '@/models/Pattern.model';

const props = defineProps<{
    pattern: IPattern;
}>();

const router = useRouter();
const marketplace = useMarketplace();
const modal = useModal();
const popup = usePopup();

const patternsInBasket = marketplace.patternReferences;
const isInBasket = computed<boolean>(() => patternsInBasket.value.has(props.pattern.reference));
const hoverText = computed<string>(() => isInBasket.value ? 'Already in basket!' : 'Add to basket');

const onClick = function (): void {
    modal.show({
        component: PatternImagesModal,
        componentProps: {
            pattern: props.pattern,
        },
    });
};

const onAddToBasket = async function (pattern: IPattern): Promise<void> {
    await marketplace.quickAdd(pattern);

    popup.trigger({
        message: `${props.pattern.title} has been added to your dashboard!`,
        style: 'success',
    });

    router.push({ path: '/dashboard' });
};
</script>

<style lang="scss">
@use '@/style/variables' as *;

.pattern-component {
    padding: 1rem;
    background-color: var(--wjb-background-colour-dark);
    cursor: pointer;

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