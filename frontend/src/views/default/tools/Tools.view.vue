<template>
    <ViewComponent class="tools-view">
        <template #nav>
            <strong>Tools</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <h2>Canvas Size Calculator</h2>
                    <div class="flex align-items-center gap">
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
                        <div class="flex-auto">
                            <IconComponent icon="arrow-right" />
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
                        </FormComponent>
                        <div>
                            <div>
                                <div class="example" :style="style"></div>
                            </div>
                        </div>
                    </div>
                    <p class="text-centered">Make sure to add a few cm/inches as extra</p>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
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
.tools-view {
    .example {
        margin: auto;
        border: 1px solid var(--wjb-secondary);
        border-radius: var(--wjb-border-radius);
        background-color: var(--wjb-background-colour-light);
    }
}
</style>