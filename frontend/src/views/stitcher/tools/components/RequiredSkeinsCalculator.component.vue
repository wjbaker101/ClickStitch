<template>
    <h2>Required Skeins Calculator</h2>
    <div class="required-skeins-calculator-component flex align-items-center gap">
        <FormComponent>
            <FormSectionComponent>
                <FormInputComponent label="Number of Sitches">
                    <input type="number" min="1" max="300" v-model="numberOfStitches">
                </FormInputComponent>
                <FormInputComponent label="Aida Count">
                    <select v-model="aidaCount">
                        <option :value="null" disabled>Select option...</option>
                        <option v-for="count in 35" :value="count + 5">{{ count + 5 }}</option>
                    </select>
                </FormInputComponent>
            </FormSectionComponent>
        </FormComponent>
        <div class="flex-auto text-centered">
            <IconComponent class="arrow-right" icon="arrow-right" />
            <IconComponent class="arrow-down" icon="arrow-down" />
        </div>
        <div class="text-centered">
            <IconComponent icon="skein" size="large" />
            <span class="skeins-count">{{ skeins }} skein(s)</span>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';

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
.required-skeins-calculator-component {

    .arrow-down {
        display: none;
    }

    .skeins-count {
        padding-left: 1rem;
    }

    @media screen and (max-width: 600px) {
        display: block;

        .arrow-down {
            display: unset;
        }

        .arrow-right {
            display: none;
        }
    }
}
</style>