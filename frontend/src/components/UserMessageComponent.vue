<template>
    <div v-if="message" class="user-message-component flex gap-4 items-center my-4 p-4 shadow-md rounded-md bg-danger/80 text-light">
        <IconComponent class="flex-auto" icon="info" />
        <div>{{ message }}</div>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

const message = ref<string | null>(null);
const timeout = ref<NodeJS.Timeout | null>(null);

defineExpose({

    set(newMessage: string, forever: boolean = false): void {
        message.value = newMessage;

        if (forever)
            return;

        timeout.value = setTimeout(() => {
            message.value = null;
        }, 8000);
    },

    clear(): void {
        if (timeout.value !== null)
            clearTimeout(timeout.value);

        message.value = null;
    },

});
</script>

<style lang="scss">
</style>