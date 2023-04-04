<template>
    <div v-if="message" class="user-message-component flex gap align-items-center">
        <div class="flex-auto">
            <IconComponent icon="info" />
        </div>
        <div>
            {{ message }}
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

const message = ref<string | null>(null);
const timeout = ref<number | null>(null);

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
@use '@/style/variables' as *;

.user-message-component {
    max-width: 450px;
    margin: 1rem 0;
    padding: 1rem;
    background-color: transparentize($danger, 0.2);
    border-radius: var(--wjb-border-radius);

    @include shadow-small();
}
</style>