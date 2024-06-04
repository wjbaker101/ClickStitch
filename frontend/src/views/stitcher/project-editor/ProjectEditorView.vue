<template>
    <ViewComponent class="project-editor-view">
        <template #nav>
            <strong>Edit Project</strong>
        </template>
        <div class="content-width">
            <LoadingComponent v-if="isLoading" itemName="project" />
            <CardComponent v-else border="top" padded>
                <h2>Edit Project</h2>
                <LinkComponent :href="`/projects/${patternReference}`">
                    <IconComponent class="flex-auto" icon="arrow-left" gap="right" />
                    <small>Back to Project</small>
                </LinkComponent>
                <FormComponent>
                    <label class="mb-4 block">
                        <strong class="block">Title</strong>
                        <InputComponent type="text" placeholder="My Amazing Pattern" v-model="title" />
                    </label>
                    <label class="mb-4 block">
                        <strong class="block">Aida Count</strong>
                        <AidaSelectionComponent v-model="aidaCount" />
                    </label>
                    <BtnComponent @click="onUpdate" :loading="isUpdating">
                        <IconComponent icon="tick" gap="right" />
                        <span class="align-middle">Update</span>
                    </BtnComponent>
                </FormComponent>
            </CardComponent>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';

import BtnComponent from '@/components/BtnComponent.vue';
import FormComponent from '@/components/form/FormComponent.vue';
import InputComponent from '@/components/inputs/InputComponent.vue';
import LoadingComponent from '@/components/loading/LoadingComponent.vue';
import AidaSelectionComponent from '@/components/aida-selection/AidaSelectionComponent.vue';

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