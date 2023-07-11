<template>
    <ViewComponent class="new-pattern-view">
        <template #nav>
            <strong>New Pattern</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <h2>Create a New Pattern</h2>
                    <RouterLink class="link-component" to="/patterns">
                        <IconComponent class="flex-auto" icon="arrow-left" gap="right" />
                        <small>Back to Patterns</small>
                    </RouterLink>
                    <FormComponent>
                        <FormSectionComponent>
                            <h3>Pattern Details</h3>
                            <FormInputComponent label="Title">
                                <input type="text" placeholder="My Amazing Pattern">
                            </FormInputComponent>
                            <ImageUploadComponent heading="Banner Image/ Thumbnail" subtext="Recommended size: 1500x1000px" />
                            <FormInputComponent label="Shop Url">
                                <small><em>A link to the pattern where stitchers can buy it</em></small>
                                <input type="text" placeholder="https://etsy.com/shop/beautifulpatternsco/amazing_pattern">
                            </FormInputComponent>
                            <div class="flex gap">
                                <FileUploadComponent class="flex-2" heading="Pattern Schematic" @choose="onPatternChoose" />
                                <div v-if="isValid !== null || isLoading" class="pattern-upload-details flex align-items-center text-centered">
                                    <template v-if="isLoading">
                                        <LoadingComponent />
                                    </template>
                                    <template v-else-if="isValid === true">
                                        <IconComponent class="flex-auto" icon="tick-circle" size="large" gap="right" />
                                        <span class="text-left">Pattern is Valid!</span>
                                    </template>
                                    <template v-else-if="isValid === false">
                                        <IconComponent class="flex-auto" icon="cross-circle" size="large" gap="right" />
                                        <span class="text-left">Pattern is invalid, please check it is a supported format and try again.</span>
                                    </template>
                                </div>
                            </div>
                        </FormSectionComponent>
                        <FormSectionComponent>
                            <p>
                                Once you're happy with the details, click the button below.
                                <br>
                                These details will be changable later, so don't worry if you spot a mistake after submitting!
                            </p>
                            <ButtonComponent>
                                <IconComponent icon="plus" gap="right" />
                                <span>Create</span>
                            </ButtonComponent>
                        </FormSectionComponent>
                    </FormComponent>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import FileUploadComponent from '@/components/FileUpload.component.vue';
import ImageUploadComponent from '@/components/ImageUpload.component.vue';

import { api } from '@/api/api';

const isLoading = ref<boolean>(false);
const isValid = ref<boolean | null>(null);

const onPatternChoose = function (file: File): void {
    isLoading.value = true;

    const reader = new FileReader();

    reader.onload = async function (): Promise<void> {
        const result = await api.patterns.verify(reader.result as string);
        if (result instanceof Error)
            return;

        isValid.value = result;
        isLoading.value = false;
    };

    reader.readAsText(file);
};
</script>

<style lang="scss">
.new-pattern-view {

    .pattern-upload-details {
        margin-top: 1rem;
    }
}
</style>