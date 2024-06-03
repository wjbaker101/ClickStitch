<template>
    <ListItemComponent class="creator-pattern-component">
        <div class="flex items-center gap-4">
            <img :src="pattern.bannerImageUrl ?? ''" class="h-auto rounded-md align-middle shadow-md max-w-[150px] bg-background">
            <h3>{{ pattern.title }}</h3>
        </div>
        <template #expanded>
            <FormComponent>
                <FormSectionComponent>
                    <div class="flex gap">
                        <h3>Edit Pattern Details</h3>
                        <div class="flex-auto">
                            <small>{{ pattern.reference }}</small>
                        </div>
                    </div>
                    <FormInputComponent label="Title">
                        <InputComponent type="text" placeholder="Amazing Pattern" v-model="form.title" />
                    </FormInputComponent>
                    <FormInputComponent label="Shop URL">
                        <InputComponent type="text" placeholder="https://etsy.com/shop/beautifulpatternsco/amazing_pattern" v-model="form.externalShopUrl" />
                    </FormInputComponent>
                    <FormInputComponent label="Aida Count">
                        <AidaSelectionComponent v-model="form.aidaCount" />
                    </FormInputComponent>
                </FormSectionComponent>
                <FormSectionComponent>
                    <BtnComponent @click="onUpdate" :loading="isLoading">
                        <IconComponent icon="tick" gap="right" />
                        <span>Update</span>
                    </BtnComponent>
                </FormSectionComponent>
                <FormSectionComponent>
                    <h3>Delete Pattern</h3>
                    <p>If any user has already added your pattern to their dashboard this action will NOT delete the pattern, but will instead hide it from new users.</p>
                    <p>If this pattern has not been added by any users yet, this action will permanently delete the pattern.</p>
                    <p>
                        <DeleteBtnComponent @delete="onDelete" />
                    </p>
                    <CardComponent v-if="deletionMessage" class="flex items-center gap-4" padded border="left">
                        <div class="flex-auto">
                            <IconComponent icon="info" gap="right" />
                        </div>
                        <div>
                            {{ deletionMessage }}
                            <br>
                            Refresh the page to update the list patterns.
                        </div>
                    </CardComponent>
                </FormSectionComponent>
            </FormComponent>
        </template>
    </ListItemComponent>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import BtnComponent from '@/components/BtnComponent.vue';
import InputComponent from '@/components/inputs/InputComponent.vue';
import DeleteBtnComponent from '@/components/DeleteBtnComponent.vue';
import ListItemComponent from '@/components/ListItemComponent.vue';
import AidaSelectionComponent from '@/components/aida-selection/AidaSelectionComponent.vue';

import { api } from '@/api/api';
import { usePopup } from '@/components/popup/Popup.use';

import { type IPattern } from '@/models/Pattern.model';

const props = defineProps<{
    pattern: IPattern;
}>();

interface IForm {
    title: string;
    externalShopUrl: string;
    aidaCount: number;
}

const popup = usePopup();

const form = ref<IForm>({
    title: props.pattern.title,
    externalShopUrl: props.pattern.externalShopUrl ?? '',
    aidaCount: props.pattern.aidaCount,
});

const isLoading = ref<boolean>(false);
const deletionMessage = ref<string | null>(null);

const onUpdate = async function (): Promise<void> {
    isLoading.value = true;

    const result = await api.patterns.update(props.pattern.reference, {
        title: form.value.title,
        externalShopUrl: form.value.externalShopUrl,
        aidaCount: form.value.aidaCount,
    });

    isLoading.value = false;

    if (result instanceof Error) {
        popup.error(result.message);
        return;
    }

    props.pattern.title = result.title;
    props.pattern.externalShopUrl = result.externalShopUrl;
    props.pattern.aidaCount = result.aidaCount;

    popup.success('Pattern updated!');
};

const onDelete = async function () {
    const result = await api.patterns.delete(props.pattern.reference);
    if (result instanceof Error)
        return;

    deletionMessage.value = result.message;

    popup.success(`${props.pattern.title} has been deleted.`);
};
</script>