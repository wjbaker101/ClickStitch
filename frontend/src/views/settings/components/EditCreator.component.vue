<template>
    <h2>Creator</h2>
    <p>You are a creator, which means stitchers can track their progress for your patterns!</p>
    <div v-if="isLoading">
        <LoadingComponent itemName="creator" />
    </div>
    <FormComponent v-else class="edit-creator-component">
        <div class="flex gap-small">
            <FormSectionComponent class="flex-2">
                <h3>Edit Creator Details</h3>
                <FormInputComponent label="Name">
                    <input class="creator-name" type="text" placeholder="Beautiful Patterns Co." v-model="form.name">
                </FormInputComponent>
                <FormInputComponent label="Store Url">
                    <input type="text" placeholder="https://etsy.com/shop/beautifulpatternsco" v-model="form.storeUrl">
                </FormInputComponent>
            </FormSectionComponent>
            <PatternComponent :pattern="fakePattern" />
        </div>
        <FormSectionComponent>
            <ButtonComponent>Update</ButtonComponent>
        </FormSectionComponent>
    </FormComponent>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import dayjs from 'dayjs';

import LoadingComponent from '@wjb/vue/component/LoadingComponent.vue';
import PatternComponent from '@/views/marketplace/components/Pattern.component.vue';

import { api } from '@/api/api';

import { IPattern } from '@/models/Pattern.model';
import { ICreator } from '@/models/Creator.model';

interface IForm {
    name: string;
    storeUrl: string;
}

const form = ref<IForm>({
    name: '',
    storeUrl: '',
});

const fakePattern = computed<IPattern>(() => ({
    title: 'Preview',
    creator: {
        name: form.value.name,
        storeUrl: form.value.storeUrl,
        reference: '',
        createdAt: dayjs(),
    },
    bannerImageUrl: '',
    createdAt: dayjs(),
    externalShopUrl: '',
    height: 0,
    price: 0,
    reference: '',
    stitchCount: 0,
    threadCount: 0,
    thumbnailUrl: '',
    width: 0,
}));

const creator = ref<ICreator | null>(null);
const isLoading = ref<boolean>(false);

onMounted(async () => {
    isLoading.value = true;

    const creatorResult = await api.creators.getSelf();
    isLoading.value = false;
    if (creatorResult instanceof Error)
        return;

    creator.value = creatorResult;

    if (creator.value !== null) {
        form.value = {
            name: creator.value.name,
            storeUrl: creator.value.storeUrl,
        };
    }
});
</script>

<style lang="scss">
.edit-creator-component {

    .creator-name {
        width: 350px;
        max-width: 100%;
    }
}
</style>