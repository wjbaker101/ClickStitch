<template>
    <ViewComponent class="project-editor-view">
        <template #nav>
            <strong>Edit Project</strong>
        </template>
        <div class="content-width">
            <LoadingComponent v-if="isLoading" itemName="project" />
            <section v-else>
                <CardComponent border="top" padded>
                    <h2>Edit Project</h2>
                    <RouterLink class="link-component" :to="`/projects/${patternReference}`">
                        <IconComponent class="flex-auto" icon="arrow-left" gap="right" />
                        <small>Back to Project</small>
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
                        </FormSectionComponent>
                        <FormSectionComponent>
                            <BtnComponent @click="onUpdate" :loading="isUpdating">
                                <IconComponent icon="tick" gap="right" />
                                <span class="align-middle">Update</span>
                            </BtnComponent>
                        </FormSectionComponent>
                    </FormComponent>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';

import BtnComponent from '@/components/BtnComponent.vue';
import LoadingComponent from '@/components/loading/LoadingComponent.vue';

import { api } from '@/api/api';

const route = useRoute();

const patternReference = route.params.patternReference as string;

const isLoading = ref<boolean>(false);

const title = ref<string>('');
const aidaCount = ref<number | null>(null);

const isUpdating = ref<boolean>(false);

const onUpdate = async function (): Promise<void> {
    if (aidaCount.value === null)
        return;

    isUpdating.value = true;
    const result = await api.patterns.update(patternReference, {
        title: title.value,
        externalShopUrl: null,
        aidaCount: aidaCount.value,
    });
    isUpdating.value = false;

    if (result instanceof Error)
        return;

    title.value = result.title;
    aidaCount.value = result.aidaCount;
};

onMounted(async () => {
    isLoading.value = true;
    const result = await api.patterns.get(patternReference);
    isLoading.value = false;

    if (result instanceof Error)
        return;

    title.value = result.title;
    aidaCount.value = result.aidaCount;
});
</script>

<style lang="scss">
.project-editor-view {
}
</style>