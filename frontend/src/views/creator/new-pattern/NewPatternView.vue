<template>
    <ViewComponent>
        <template #nav>
            <strong>New Pattern</strong>
        </template>
        <CardComponent border="top" padded>
            <h2 class="mb-4 text-2xl font-bold">Create a New Pattern</h2>
            <LinkComponent href="/patterns">
                <ArrowLeftIcon class="mr-1" />
                <small class="align-middle">Back to Patterns</small>
            </LinkComponent>
            <FormComponent class="mb-4">
                <h3 class="m-0 mb-4">Pattern Details</h3>
                <label class="mb-4 block">
                    <strong class="block">Title</strong>
                    <InputComponent type="text" placeholder="My Amazing Pattern" v-model="title" />
                </label>
                <ImageUploadComponent heading="Banner Image/ Thumbnail" subtext="Recommended size: 1500x1000px" @choose="onBannerImageChoose" />
                <label class="block">
                    <strong class="block">Shop URL</strong>
                    <small class="block">
                        <em>A link to the pattern where stitchers can buy it</em>
                    </small>
                    <InputComponent type="text" placeholder="https://etsy.com/shop/beautifulpatternsco/amazing_pattern" v-model="externalShopUrl" />
                </label>
                <div class="flex flex-row items-center gap-4">
                    <FileUploadComponent heading="Pattern Schematic" @choose="onPatternChoose" class="grow">
                        <template #subtext>
                            <LinkComponent href="/supported-pattern-formats">
                                <small>View supported formats here</small>
                            </LinkComponent>
                        </template>
                    </FileUploadComponent>
                    <div v-if="isValid !== null || isLoading" class="mt-4 max-w-sm text-center">
                        <template v-if="isLoading">
                            <LoadingComponent itemName="schema" />
                        </template>
                        <template v-else-if="isValid === true">
                            <CircleCheckBig class="!size-12" />
                            <br>
                            <span class="text-left">Pattern is Valid!</span>
                        </template>
                        <template v-else-if="isValid === false">
                            <CircleXIcon class="!size-12" />
                            <br>
                            <span class="text-left">Pattern is invalid, please check it is a supported format and try again.</span>
                        </template>
                    </div>
                </div>
            </FormComponent>
            <FormComponent>
                <p>
                    Once you're happy with the details, click the button below.
                    <br>
                    These details will be changable later, so don't worry if you spot a mistake after submitting!
                </p>
                <p>
                    <em>This may take a while, in some cases up to a few minutes.</em>
                </p>
                <BtnComponent @click="onCreate" :loading="isCreationLoading">
                    <PlusIcon class="mr-2" />
                    <span class="align-middle">Create</span>
                </BtnComponent>
            </FormComponent>
        </CardComponent>
    </ViewComponent>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';

import { ArrowLeftIcon, CircleCheckBig, CircleXIcon, PlusIcon } from 'lucide-vue-next';
import BtnComponent from '@/components/BtnComponent.vue';
import FormComponent from '@/components/form/FormComponent.vue';
import InputComponent from '@/components/inputs/InputComponent.vue';
import LoadingComponent from '@/components/loading/LoadingComponent.vue';
import FileUploadComponent from '@/components/FileUploadComponent.vue';
import ImageUploadComponent, { type IOnImageUploadChoose } from '@/components/ImageUploadComponent.vue';

import { api } from '@/api/api';
import { usePopup } from '@/components/popup/Popup.use';

const router = useRouter();
const popup = usePopup();

const isLoading = ref<boolean>(false);
const isValid = ref<boolean | null>(null);

const isCreationLoading = ref<boolean>(false);

const bannerImage = ref<File | null>(null);
const title = ref<string>('');
const externalShopUrl = ref<string>('');
const patternData = ref<string | null>(null);

const onBannerImageChoose = function (payload: IOnImageUploadChoose) {
    bannerImage.value = payload.asFile;
};

const onPatternChoose = function (file: File): void {
    isLoading.value = true;

    const reader = new FileReader();

    reader.onload = async function (): Promise<void> {
        const result = await api.patterns.verify(reader.result as string);
        if (result instanceof Error)
            return;

        isValid.value = result;
        isLoading.value = false;

        if (isValid.value)
            patternData.value = reader.result as string;
    };

    reader.readAsText(file);
};

const onCreate = async function (): Promise<void> {
    if (title.value.length < 3) {
        popup.error('Please enter a valid title.');
        return;
    }
    if (bannerImage.value === null) {
        popup.error('Please choose a banner/thumbnail image.');
        return;
    }
    if (patternData.value === null) {
        popup.error('Please upload a valid pattern schematic.');
        return;
    }

    isCreationLoading.value = true;

    await api.patterns.create(bannerImage.value, patternData.value, {
        title: title.value,
        externalShopUrl: externalShopUrl.value,
        aidaCount: 16,
        price: 1,
    });

    isCreationLoading.value = false;

    popup.success(`Pattern '${title.value}' has been created!`);

    router.push({ path: '/patterns', });
};
</script>