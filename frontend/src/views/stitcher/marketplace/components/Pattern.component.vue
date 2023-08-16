<template>
    <CardComponent class="pattern-component text-centered" header hoverable @click="onClick">
        <template #header>
            <h3>{{ pattern.title }}</h3>
        </template>
        <PatternImageComponent :image="pattern.bannerImageUrl" />
        <p>
            <IconComponent icon="user" gap="right" />
            <span class="created-by">{{ pattern.creator?.name }}</span>
        </p>
        <div class="description flex align-items-center gap-small">
            <div class="flex gap-small">
                <div v-if="authDetails !== null" class="flex">
                    <ButtonComponent class="add-to-basket-button" :title="hoverText" @click.stop="onAddToBasket(pattern)" :disabled="isInBasket">
                        <IconComponent v-if="isInBasket" icon="tick" gap="right" />
                        <IconComponent v-else icon="plus" gap="right" />
                        <span>Add</span>
                    </ButtonComponent>
                </div>
                <a v-if="pattern.externalShopUrl !== null" class="flex" :href="pattern.externalShopUrl" target="_blank">
                    <ButtonComponent class="secondary" @click.stop="">
                        <IconComponent icon="external-link" gap="right" />
                        <span>View Page</span>
                    </ButtonComponent>
                </a>
            </div>
        </div>
    </CardComponent>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRouter } from 'vue-router';

import PatternImageComponent from '@/components/shared/PatternImage.component.vue';
import PatternImagesModal from '@/views/stitcher/marketplace/modal/PatternImagesModal.component.vue';

import { useAuth } from '@/use/auth/Auth.use';
import { useMarketplace } from '@/use/marketplace/Marketplace.use';
import { useModal } from '@wjb/vue/use/modal.use';
import { usePopup } from '@wjb/vue/use/popup.use';

import { type IPattern } from '@/models/Pattern.model';

const props = defineProps<{
    pattern: IPattern;
}>();

const auth = useAuth();
const router = useRouter();
const marketplace = useMarketplace();
const modal = useModal();
const popup = usePopup();

const authDetails = auth.details;

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

    .created-by {
        vertical-align: middle;
    }
}
</style>