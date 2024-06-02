<template>
    <h2>Canvas Size Calculator</h2>
    <div class="grid items-center gap-4 canvas-size-calculator-component sm:grid-cols-[1fr_auto_1fr] md:grid-cols-[1fr_auto_1fr_1fr]">
        <FormComponent>
            <FormSectionComponent>
                <FormInputComponent label="Width (Stitches)">
                    <InputComponent type="number" min="1" max="300" v-model="width" />
                </FormInputComponent>
                <FormInputComponent label="Height (Stitches)">
                    <InputComponent type="number" min="1" max="300" v-model="height" />
                </FormInputComponent>
                <FormInputComponent label="Aida Stitch Count">
                    <InputComponent type="number" v-model="stitchCount" />
                </FormInputComponent>
            </FormSectionComponent>
        </FormComponent>
        <div class="flex-auto text-center">
            <IconComponent class="hidden sm:block" icon="arrow-right" />
            <IconComponent class="sm:hidden" icon="arrow-down" />
        </div>
        <FormComponent>
            <FormSectionComponent>
                <p>
                    <strong>Width</strong>
                    <br>
                    {{ (resultWidth * cmToInch).toFixed(1) }}cm / {{ resultWidth.toFixed(1) }}in
                </p>
                <p>
                    <strong>Height</strong>
                    <br>
                    {{ (resultHeight * cmToInch).toFixed(1) }}cm / {{ resultHeight.toFixed(1) }}in
                </p>
            </FormSectionComponent>
            <div class="text-centered">Make sure to add a few cm/inches as padding</div>
        </FormComponent>
        <div class="m-auto flex place-content-center preview-container h-[200px] sm:col-start-1 sm:col-end-4 md:col-start-4 md:col-end-4">
            <div class="m-auto flex flex-auto items-center rounded-md border-2 border-solid text-center bg-texture border-secondary" :style="style">
                <small>Preview</small>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';

import InputComponent from '@/components/inputs/InputComponent.vue';

const cmToInch = 2.54;
const previewSize = 200;

const width = ref<number>(50);
const height = ref<number>(50);
const stitchCount = ref<number>(16);

const resultWidth = computed<number>(() => width.value / stitchCount.value);
const resultHeight = computed<number>(() => height.value / stitchCount.value);

const ratio = computed<number>(() => width.value / height.value);

const style = computed<Record<string, string>>(() => {
    if (width.value >= height.value) {
        return {
            width: `${previewSize}px`,
            height: `${previewSize * height.value / width.value}px`,
        };
    }

    return {
        height: `${previewSize}px`,
        width: `${previewSize * ratio.value}px`,
    };
});
</script>

<style lang="scss">
</style>