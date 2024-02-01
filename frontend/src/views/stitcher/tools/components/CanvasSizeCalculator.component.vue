<template>
    <h2>Canvas Size Calculator</h2>
    <div class="canvas-size-calculator-component">
        <FormComponent>
            <FormSectionComponent>
                <FormInputComponent label="Width (Stitches)">
                    <input type="number" min="1" max="300" v-model="width">
                </FormInputComponent>
                <FormInputComponent label="Height (Stitches)">
                    <input type="number" min="1" max="300" v-model="height">
                </FormInputComponent>
                <FormInputComponent label="Aida Stitch Count">
                    <input type="number" v-model="stitchCount">
                </FormInputComponent>
            </FormSectionComponent>
        </FormComponent>
        <div class="flex-auto text-centered">
            <IconComponent class="arrow-right" icon="arrow-right" />
            <IconComponent class="arrow-down" icon="arrow-down" />
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
        <div class="preview-container">
            <div class="preview" :style="style">
                <small>Preview</small>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';

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
.canvas-size-calculator-component {
    display: grid;
    gap: 1rem;
    grid-template-columns: 1fr auto 1fr 1fr;
    align-items: center;

    .arrow-down {
        display: none;
    }

    .preview-container {
        height: 200px;
        display: flex;
        justify-content: center;
        align-items: center;
        margin: auto;
    }

    .preview {
        display: flex;
        justify-content: center;
        align-items: center;
        margin: auto;
        border: 1px solid var(--wjb-secondary);
        border-radius: var(--wjb-border-radius);
        background-color: var(--wjb-background-colour-light);
        background-image: var(--stitched-background-image);
    }

    @media screen and (max-width: 800px) {
        grid-template-columns: 1fr auto 1fr;
        grid-template-rows: auto auto;

        .preview-container {
            grid-column: 1 / 4;
        }
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