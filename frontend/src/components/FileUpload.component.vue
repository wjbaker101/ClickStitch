<template>
    <div class="file-upload-component">
        <label>
            <strong>{{ heading ?? 'Upload File' }}</strong>
            <input type="file" @change="onChange">
            <div class="file-placeholder hoverable text-centered">
                <template v-if="fileName !== null">
                    <p>
                        <strong>Click to Change File</strong>
                    </p>
                    <em class="file-name">{{ fileName }}</em>
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

const fileName = ref<string | null>(null);
const image = ref<string | null>(null);

const onChange = function (event: Event): void {
    const reader = new FileReader();
    reader.onload = function(){
        image.value = reader.result as string;
    };

    const element = event.target as HTMLInputElement;

    if (element.files === null)
        return;

    const file = element.files[0];

    reader.readAsDataURL(file);
    fileName.value = file.name;
};
</script>

<style lang="scss">
.file-upload-component {
    margin: 1rem 0;

    input {
        display: none;
    }

    .file-name {
        padding-left: 0.5rem;
    }

    .file-placeholder {
        background-color: var(--wjb-background-colour-light);
        padding: 1rem;
        border-radius: var(--wjb-border-radius);
        outline: 2px dashed var(--wjb-tertiary);
        box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);
        cursor: pointer;

        &:hover {
            outline-color: transparent;
        }
    }
}
</style>