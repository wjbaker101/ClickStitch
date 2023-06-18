<template>
    <ListItemComponent>
        <div class="flex align-items-center gap">
            <PatternImageComponent width="150" height="100" class="flex-auto" :image="pattern.bannerImageUrl" />
            <h3>{{ pattern.title }}</h3>
        </div>
        <template #expanded>
            <FormComponent>
                <FormSectionComponent>
                    <div class="flex gap">
                        <h4>Edit Pattern Details</h4>
                        <div class="flex-auto">
                            <small>{{ pattern.reference }}</small>
                        </div>
                    </div>
                    <FormInputComponent label="Title">
                        <input type="text" placeholder="Amazing Pattern" v-model="form.title">
                    </FormInputComponent>
                    <FormInputComponent label="Shop URL">
                        <input type="text" placeholder="https://etsy.com/shop/beautifulpatternsco/amazing_pattern" v-model="form.externalShopUrl">
                    </FormInputComponent>
                </FormSectionComponent>
                <FormSectionComponent>
                    <ButtonComponent @click="onUpdate">
                        <IconComponent icon="tick" gap="right" />
                        <span>Update</span>
                    </ButtonComponent>
                </FormSectionComponent>
            </FormComponent>
        </template>
    </ListItemComponent>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import ListItemComponent from '@/components/ListItem.component.vue';
import PatternImageComponent from '@/components/shared/PatternImage.component.vue';

import { api } from '@/api/api';
import { usePopup } from '@wjb/vue/use/popup.use';

import { IPattern } from '@/models/Pattern.model';

const props = defineProps<{
    pattern: IPattern;
}>();

interface IForm {
    title: string;
    externalShopUrl: string;
}

const popup = usePopup();

const form = ref<IForm>({
    title: props.pattern.title,
    externalShopUrl: props.pattern.externalShopUrl ?? '',
});

const onUpdate = async function (): Promise<void> {
    const result = await api.patterns.update(props.pattern.reference, {
        title: form.value.title,
        externalShopUrl: form.value.externalShopUrl,
    });
    if (result instanceof Error) {
        popup.trigger({
            message: result.message,
            style: 'error',
        });
        return;
    }

    popup.trigger({
        message: 'Pattern updated!',
        style: 'success',
    });
};
</script>

<style lang="scss">
</style>