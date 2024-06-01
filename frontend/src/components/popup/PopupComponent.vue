<template>
    <Transition>
        <div
            v-if="options !== null"
            @click="onClick"
            class="fixed top-4 left-1/2 z-10 -translate-x-1/2 rounded-b-md px-8 py-4 shadow-xl bg-background-light border-0 border-solid border-t-2 border-t-[rgb(var(--colour))] cursor-pointer"
            :style="{
                '--colour': colour,
            }"
        >
            <IconComponent :icon="icon" class="mr-4 text-[rgb(var(--colour))] size-6 drop-shadow-sm" />
            <span class="align-middle">{{ options.message }}</span>
        </div>
    </Transition>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';

import { type IPopupEvent, usePopup } from '@/components/popup/Popup.use';

const popup = usePopup();

const options = ref<IPopupEvent | null>(null);
const timeout = ref<NodeJS.Timeout | null>(null);

const colour = computed<string>(() => {
    switch (options.value?.type) {
        case 'message':
            return 'var(--secondary)';
        case 'success':
            return 'var(--success)';
        case 'error':
            return 'var(--danger)';
        default:
            return 'var(--secondary)';
    }
});

const icon = computed(() => {
    switch (options.value?.type) {
        case 'message':
            return 'info';
        case 'success':
            return 'tick';
        case 'error':
            return 'warning';
        default:
            return '';
    }
});

onMounted(() => {
    popup.subscribe(event => {
        options.value = event;

        timeout.value = setTimeout(() => {
            options.value = null;
            timeout.value = null;
        }, 5000);
    });
});

const onClick = function (): void {
    options.value = null;

    if (timeout.value !== null) {
        clearTimeout(timeout.value);
        timeout.value = null;
    }
};
</script>

<style lang="scss" scoped>
.v-enter-from,
.v-leave-to {
    opacity: 0;
    transform: translateY(-150%) translateX(-50%) scale(0.8);
}
</style>