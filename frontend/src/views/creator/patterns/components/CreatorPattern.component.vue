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
                    <FormInputComponent label="Shop URL">
                        <input type="text" placeholder="https://etsy.com/shop/beautifulpatternsco/amazing_pattern" :value="pattern.externalShopUrl">
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
import ListItemComponent from '@/components/ListItem.component.vue';
import PatternImageComponent from '@/components/shared/PatternImage.component.vue';

import { usePopup } from '@wjb/vue/use/popup.use';

import { IPattern } from '@/models/Pattern.model';

defineProps<{
    pattern: IPattern;
}>();

const popup = usePopup();

const onUpdate = async function (): Promise<void> {
    popup.trigger({
        message: 'Pattern updated!',
        style: 'success',
    });
};
</script>

<style lang="scss">
</style>