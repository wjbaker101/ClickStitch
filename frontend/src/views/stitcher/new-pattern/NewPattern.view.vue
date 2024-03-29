<template>
    <ViewComponent class="new-pattern-view">
        <template #nav>
            <strong>New Pattern</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <h2>Create a New Pattern</h2>
                    <RouterLink class="link-component" to="/dashboard">
                        <IconComponent class="flex-auto" icon="arrow-left" gap="right" />
                        <small>Back to Dashboard</small>
                    </RouterLink>
                    <FormComponent>
                        <FormSectionComponent>
                            <h3>Pattern Details</h3>
                            <FormInputComponent label="Title">
                                <input type="text" placeholder="My Amazing Pattern" v-model="title">
                            </FormInputComponent>
                            <FormInputComponent label="Aida Count">
                                <select v-model="aidaCount">
                                    <option :value="null" disabled>Select option...</option>
                                    <option v-for="count in 35" :value="count + 5">{{ count + 5 }}</option>
                                </select>
                            </FormInputComponent>
                            <div class="flex gap">
                                <FileUploadComponent class="flex-2" heading="Pattern Schematic" @choose="onPatternChoose">
                                    <template #subtext>
                                        <RouterLink class="link-component" to="/supported-pattern-formats">
                                            <small>View supported formats here</small>
                                        </RouterLink>
                                    </template>
                                </FileUploadComponent>
                                <div v-if="isValid !== null || isLoading" class="pattern-upload-details flex align-items-center text-centered">
                                    <template v-if="isLoading">
                                        <LoadingComponent itemName="asd" />
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
                            <p>Once you're happy with the details, click the button below.</p>
                            <p>
                                <em>(This may take a while, in some cases up to a few minutes).</em>
                            </p>
                            <ButtonComponent @click="onCreate" :loading="isCreationLoading">
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
import { useRouter } from 'vue-router';

import FileUploadComponent from '@/components/FileUpload.component.vue';

import { api } from '@/api/api';
import { usePopup } from '@wjb/vue/use/popup.use';

const router = useRouter();
const popup = usePopup();

const isLoading = ref<boolean>(false);
const isValid = ref<boolean | null>(null);

const isCreationLoading = ref<boolean>(false);

const title = ref<string>('');
const aidaCount = ref<number | null>(null);
const patternData = ref<string | null>(null);

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
        popup.trigger({
            message: 'Please enter a valid title.',
            style: 'error',
        });
        return;
    }
    if (aidaCount.value === null) {
        popup.trigger({
            message: 'Please enter a valid aida count.',
            style: 'error',
        });
        return;
    }
    if (patternData.value === null) {
        popup.trigger({
            message: 'Please upload a valid pattern schematic.',
            style: 'error',
        });
        return;
    }

    isCreationLoading.value = true;

    await api.patterns.create(new File([], ''), patternData.value, {
        title: title.value,
        externalShopUrl: null,
        aidaCount: aidaCount.value,
        price: 1,
    });

    isCreationLoading.value = false;

    popup.trigger({
        message: `Pattern '${title.value}' has been created!`,
        style: 'success',
    });

    router.push({ path: '/dashboard', });
};
</script>

<style lang="scss">
.new-pattern-view {

    .pattern-upload-details {
        margin-top: 1rem;
    }
}
</style>