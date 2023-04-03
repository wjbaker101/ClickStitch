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

defineExpose({

    set(newMessage: string): void {
        message.value = newMessage;

        setTimeout(() => {
            message.value = null;
        }, 8000);
    },

});
</script>

<style lang="scss">
@use '@/style/variables' as *;

.user-message-component {
    margin: 1rem 0;
    padding: 1rem;
    background-color: transparentize($danger, 0.2);
    border-radius: var(--wjb-border-radius);

    @include shadow-small();
}
</style>