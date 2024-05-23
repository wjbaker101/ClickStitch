<template>
    <div class="image-upload-component">
        <label>
            <strong>{{ heading ?? 'Upload Image' }}</strong>
            <br>
            <small v-if="subtext">
                <em>{{ subtext }}</em>
            </small>
            <input type="file" @change="onChange">
            <div v-if="image !== null">
                <img class="image hoverable" :src="image">
            </div>
            <div v-else class="image-placeholder hoverable text-centered">
                <p>Click to Upload Image</p>
            </div>
        </label>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

defineProps<{
    heading?: string;
    subtext?: string;
}>();

const emit = defineEmits(['choose']);

export interface IOnImageUploadChoose {
    readonly asFile: File;
    readonly asString: string;
}

const image = ref<string | null>(null);

const onChange = function (event: Event): void {
    const reader = new FileReader();
    reader.onload = function(){
        image.value = reader.result as string;

        const payload: IOnImageUploadChoose = {
            asFile: element?.files?.[0] as File,
            asString: image.value,
        };
        emit('choose', payload);
    };

    const element = event.target as HTMLInputElement;

    if (element.files === null)
        return;

    reader.readAsDataURL(element.files[0]);
};
</script>

<style lang="scss">
.image-upload-component {
    margin: 1rem 0;

    .image-container {
        margin-top: 1rem;
    }

    .image {
        width: 250px;
        max-width: 100%;
        height: auto;
        max-height: 250px;
    }

    input {
        display: none;
    }

    .hoverable {
        border-radius: var(--wjb-border-radius);
        outline: 2px dashed transparent;
        outline: 2px dashed var(--wjb-tertiary);
        box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);
        cursor: pointer;

        &:hover {
            outline-color: transparent;
        }
    }

    .image-placeholder {
        background-color: var(--wjb-background-colour-light);
        padding: 1rem;
    }
}
</style>