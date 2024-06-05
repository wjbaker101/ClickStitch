<template>
    <h2 class="mb-4 text-2xl font-bold">Canvas Size Calculator</h2>
    <div class="grid items-center gap-4 canvas-size-calculator-component sm:grid-cols-[1fr_auto_1fr] md:grid-cols-[1fr_auto_1fr_1fr]">
        <FormComponent>
            <label class="mb-4 block">
                <strong class="block">Width (Stitches)</strong>
                <InputComponent type="number" min="1" max="300" v-model="width" />
            </label>
            <label class="mb-4 block">
                <strong class="block">Height (Stitches)</strong>
                <InputComponent type="number" min="1" max="300" v-model="height" />
            </label>
            <label>
                <strong class="block">Aida Stitch Count</strong>
                <InputComponent type="number" v-model="stitchCount" />
            </label>
        </FormComponent>
        <div class="flex-auto text-center">
            <ArrowRightIcon class="hidden sm:block" />
            <ArrowDownIcon class="sm:hidden" />
        </div>
        <div>
            <FormComponent>
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
            </FormComponent>
            <div class="text-center">Make sure to add a few cm/inches as padding</div>
        </div>
        <div class="m-auto flex place-content-center h-[200px] sm:col-start-1 sm:col-end-4 md:col-start-4 md:col-end-4">
            <div class="m-auto flex flex-auto rounded-md border-2 border-solid text-center bg-texture border-secondary" :style="style">
                <small class="m-auto">Preview</small>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';

import { ArrowDownIcon, ArrowRightIcon } from 'lucide-vue-next';
import FormComponent from '@/components/form/FormComponent.vue';
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