<template>
    <div class="add-thread-modal-component">
        <h2>Add Thread</h2>
        <FormComponent>
            <FormSectionComponent>
                <FormInputComponent label="Code">
                    <input type="text" placeholder="DMC 1234" v-model="code">
                </FormInputComponent>
                <FormInputComponent label="Description">
                    <input type="text" placeholder="Sunny Sky Blue" v-model="description">
                </FormInputComponent>
            </FormSectionComponent>
            <FormSectionComponent>
                <ButtonComponent @click="onSubmit">
                    <IconComponent icon="tick" gap="right" />
                    <span>Submit</span>
                </ButtonComponent>
            </FormSectionComponent>
        </FormComponent>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import { api } from '@/api/api';

const code = ref<string>('');
const description = ref<string>('');

const onSubmit = async function (): Promise<void> {
    if (code.value.length < 3)
        return;
    if (description.value.length < 3)
        return;

    const result = await api.admin.createThread({
        code: code.value,
        description: description.value,
    });
    if (result instanceof Error)
        return;


};
</script>

<style lang="scss">
.add-thread-modal-component {
    width: 720px;
    max-width: 100%;
}
</style>