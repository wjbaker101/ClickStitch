<template>
    <h2>Required Skeins Calculator</h2>
    <div class="grid items-center gap-4 sm:grid-cols-[1fr_auto_1fr]">
        <FormComponent>
            <label class="mb-4 block">
                <strong class="block">Number of Stitches</strong>
                <InputComponent type="number" min="1" max="300" v-model="numberOfStitches" />
            </label>
            <label>
                <strong class="block">Aida Count</strong>
                <AidaSelectionComponent v-model="aidaCount" />
            </label>
        </FormComponent>
        <div class="text-center">
            <ArrowRightIcon class="hidden sm:block" />
            <ArrowDownIcon class="sm:hidden" />
        </div>
        <div class="text-center">
            <SkeinIcon class="size-12" />
            <span class="pl-4">{{ skeins }} skein(s)</span>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';

import { ArrowDownIcon, ArrowRightIcon } from 'lucide-vue-next';
import SkeinIcon from '@/components/icons/SkeinIcon.vue';
import FormComponent from '@/components/form/FormComponent.vue';
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