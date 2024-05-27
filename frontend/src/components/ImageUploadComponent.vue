<template>
    <div class="image-upload-component my-4">
        <label>
            <strong>{{ heading ?? 'Upload Image' }}</strong>
            <br>
            <small v-if="subtext">
                <em>{{ subtext }}</em>
            </small>
            <input class="hidden" type="file" @change="onChange">
            <div v-if="image !== null">
                <img :src="image" class="hoverable w-[250px] max-w-full h-auto max-h-full shadow-md rounded-md text-center cursor-pointer">
            </div>
            <div v-else class="image-placeholder hoverable rounded-md text-center p-4 shadow-md cursor-pointer">
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

    .hoverable {
        outline: 2px dashed var(--wjb-tertiary);

        &:hover {
            outline-color: transparent;
        }
    }

    .image-placeholder {
        background-color: var(--wjb-background-colour-light);
    }
}
</style>