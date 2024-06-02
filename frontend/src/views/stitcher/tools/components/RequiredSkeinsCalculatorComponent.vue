<template>
    <h2>Required Skeins Calculator</h2>
    <div class="grid items-center gap-4 sm:grid-cols-[1fr_auto_1fr]">
        <FormComponent>
            <FormSectionComponent>
                <FormInputComponent label="Number of Stitches">
                    <InputComponent type="number" min="1" max="300" v-model="numberOfStitches" />
                </FormInputComponent>
                <FormInputComponent label="Aida Count">
                    <AidaSelectionComponent v-model="aidaCount" />
                </FormInputComponent>
            </FormSectionComponent>
        </FormComponent>
        <div class="text-center">
            <IconComponent class="hidden sm:block" icon="arrow-right" />
            <IconComponent class="sm:hidden" icon="arrow-down" />
        </div>
        <div class="text-center">
            <IconComponent icon="skein" size="large" />
            <span class="pl-4">{{ skeins }} skein(s)</span>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';

import InputComponent from '@/components/inputs/InputComponent.vue';
import AidaSelectionComponent from '@/components/aida-selection/AidaSelectionComponent.vue';

import { calculateRequiredSkeins } from '@/helper/stitch.helper';

const numberOfStitches = ref<number>(0);
const aidaCount = ref<number | null>(null);

const skeins = computed<number>(() => {
    if (aidaCount.value === null)
        return 0;

    return calculateRequiredSkeins(numberOfStitches.value, aidaCount.value);
});
</script>

<style lang="scss">
</style>