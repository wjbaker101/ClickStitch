<template>
    <div v-if="message" class="user-message-component tw-flex tw-gap-4 tw-items-center tw-my-4 tw-p-4 tw-shadow-md tw-rounded-md tw-bg-danger/80 tw-text-light">
        <IconComponent icon="info" />
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