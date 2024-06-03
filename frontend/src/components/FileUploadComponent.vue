<template>
    <div class="my-4 file-upload-component">
        <label>
            <strong>{{ heading ?? 'Upload File' }}</strong>
            <br>
            <slot name="subtext"></slot>
            <input type="file" @change="onChange" class="hidden">
            <div class="cursor-pointer rounded-md p-4 shadow-md outline-dashed outline-2 file-placeholder text-centered bg-background-light outline-secondary hover:outline-transparent">
                <template v-if="fileName !== null">
                    <p>
                        <strong>Click to Change File</strong>
                    </p>
                    <em class="pl-2">{{ fileName }}</em>
                </template>
                <p v-else>Click to Choose a File</p>
            </div>
        </label>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

defineProps<{
    heading?: string;
}>();

const emit = defineEmits(['choose']);

const fileName = ref<string | null>(null);

const onChange = function (event: Event): void {
    const element = event.target as HTMLInputElement;
    if (element.files === null)
        return;

    const file = element.files[0];

    fileName.value = file.name;

    emit('choose', file);
};
</script>