<template>
    <div class="image-upload-component">
        <label>
            <strong>{{ heading ?? 'Upload Image' }}</strong>
            <br>
            <input type="file" @change="onChange">
        </label>
        <div class="image-container">
            <img v-if="image !== null" class="image" :src="image">
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

defineProps<{
    heading?: string;
}>();

const image = ref<string | null>(null);

const onChange = function (event: Event): void {
    const reader = new FileReader();
    reader.onload = function(){
        image.value = reader.result as string;
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
}
</style>