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
                <a v-if="pattern.externalShopUrl !== null" class="flex" :href="pattern.externalShopUrl" target="_blank">
                    <ButtonComponent class="secondary" @click.stop="">
                        <IconComponent icon="external-link" gap="right" />
                        <span>View Page</span>
                    </ButtonComponent>
                </a>
                <div v-if="authDetails !== null" class="flex">
                    <ButtonComponent title="Add to Your Dashboard" @click.stop="onAddToBasket(pattern)">
                        <IconComponent icon="plus" gap="right" />
                        <span>Add to Dashboard</span>
                    </ButtonComponent>
                </div>
            </div>
        </div>
    </CardComponent>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';

import PatternImageComponent from '@/components/shared/PatternImage.component.vue';
import PatternImagesModal from '@/views/stitcher/patterns/modal/PatternImagesModal.component.vue';

import { api } from '@/api/api';
import { useAuth } from '@/use/auth/Auth.use';
import { useModal } from '@wjb/vue/use/modal.use';
import { usePopup } from '@wjb/vue/use/popup.use';

import { type IPattern } from '@/models/Pattern.model';

const props = defineProps<{
    pattern: IPattern;
}>();

const auth = useAuth();
const modal = useModal();
const popup = usePopup();
const router = useRouter();

const authDetails = auth.details;

const onClick = function (): void {
    modal.show({
        component: PatternImagesModal,
        componentProps: {
            pattern: props.pattern,
        },
    });
};

const onAddToBasket = async function (pattern: IPattern): Promise<void> {
    await api.basket.quickAdd(pattern.reference);

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