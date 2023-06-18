<template>
    <ListItemComponent>
        <div class="flex align-items-center gap">
            <PatternImageComponent width="150" height="100" class="flex-auto" :image="pattern.bannerImageUrl" />
            <h3>{{ pattern.title }}</h3>
        </div>
        <template #expanded>
            <FormComponent>
                <FormSectionComponent>
                    <h4>Edit Pattern Details</h4>
                    <FormInputComponent label="Shop URL">
                        <input type="text" placeholder="https://etsy.com/shop/beautifulpatternsco/amazing_pattern" :value="pattern.externalShopUrl">
                    </FormInputComponent>
                </FormSectionComponent>
            </FormComponent>
        </template>
    </ListItemComponent>
</template>

<script setup lang="ts">
import ListItemComponent from '@/components/ListItem.component.vue';
import PatternImageComponent from '@/components/shared/PatternImage.component.vue';

import { IPattern } from '@/models/Pattern.model';

defineProps<{
    pattern: IPattern;
}>();
</script>

<style lang="scss">
</style>