<template>
    <h2 class="mb-4 text-2xl font-bold">Creator</h2>
    <p>You are a creator, which means stitchers can track their progress for your patterns!</p>
    <div v-if="isLoading">
        <LoadingComponent itemName="creator" />
    </div>
    <FormComponent v-else>
        <h3 class="m-0 mb-4 text-lg font-bold">{{ creator === null ? 'Setup your Creator Details' : 'Edit Creator Details' }}</h3>
        <label class="mb-4 block">
            <strong class="block">Name</strong>
            <InputComponent class="max-w-full w-[350px]" type="text" placeholder="Beautiful Patterns Co." v-model="form.name" />
        </label>
        <label class="mb-4 block">
            <strong class="block">Store URL</strong>
            <InputComponent type="text" placeholder="https://etsy.com/shop/beautiful-patterns-co" v-model="form.storeUrl" />
        </label>
        <BtnComponent @click="onSubmit">
            <template v-if="creator === null">
                <PlusIcon class="mr-2" />
                <span class="align-middle">Create</span>
            </template>
            <template v-else>
                <CheckIcon class="mr-2" />
                <span class="align-middle">Update</span>
            </template>
        </BtnComponent>
    </FormComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import { PlusIcon, CheckIcon } from 'lucide-vue-next';
import BtnComponent from '@/components/BtnComponent.vue';
import FormComponent from '@/components/form/FormComponent.vue';
import InputComponent from '@/components/inputs/InputComponent.vue';
import LoadingComponent from '@/components/loading/LoadingComponent.vue';

import { api } from '@/api/api';
import { usePopup } from '@/components/popup/Popup.use';

import { type ICreator } from '@/models/Creator.model';

const popup = usePopup();

interface IForm {
    name: string;
    storeUrl: string;
}

const form = ref<IForm>({
    name: '',
    storeUrl: '',
});

const creator = ref<ICreator | null>(null);
const isLoading = ref<boolean>(false);

const setCreator = function (newCreator: ICreator | null): void {
    creator.value = newCreator;

    if (newCreator !== null) {
        form.value = {
            name: newCreator.name,
            storeUrl: newCreator.storeUrl,
        };
    }
};

onMounted(async () => {
    isLoading.value = true;

    const creatorResult = await api.creators.getSelf();
    isLoading.value = false;
    if (creatorResult instanceof Error)
        return;

    setCreator(creatorResult);
});

const onSubmit = async function (): Promise<void> {
    if (creator.value === null) {
        const creatorResult = await api.creators.createCreator({
            name: form.value.name,
            storeUrl: form.value.storeUrl,
        });
        if (creatorResult instanceof Error)
            return;

        setCreator(creatorResult);

        popup.success('Creator has been created!');
    }
    else {
        await api.creators.updateCreator(creator.value.reference, {
            name: form.value.name,
            storeUrl: form.value.storeUrl,
        });

        popup.success('Creator has been updated!');
    }
};
</script>