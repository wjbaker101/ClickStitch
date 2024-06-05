<template>
    <div v-if="message" class="my-4 flex items-center gap-4 rounded-md p-4 shadow-md user-message-component bg-danger/80 text-light">
        <InfoIcon />
        <div>{{ message }}</div>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import { InfoIcon } from 'lucide-vue-next';

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