<template>
    <div class="my-4 image-upload-component">
        <label>
            <strong>{{ heading ?? 'Upload Image' }}</strong>
            <br>
            <small v-if="subtext">
                <em>{{ subtext }}</em>
            </small>
            <input class="hidden" type="file" @change="onChange">
            <div v-if="image !== null">
                <img :src="image" class="h-auto max-h-full max-w-full cursor-pointer rounded-md text-center shadow-md hoverable w-[250px]">
            </div>
            <div v-else class="cursor-pointer rounded-md p-4 text-center shadow-md outline-dashed outline-2 hoverable bg-background-light outline-secondary hover:outline-transparent">
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