<template>
    <Transition>
        <div v-if="options" @click.self="onClose" class="fixed inset-0 z-10 bg-black/40 backdrop-blur-sm grid grid-cols-[1fr_minmax(30%,450px)] py-2 pl-2">
            <div class="relative col-start-2 rounded-l-md p-4 shadow-xl bg-texture">
                <div @click="onClose" class="absolute top-2 right-2 grid cursor-pointer place-content-center rounded-full border-solid text-center shadow-md border-1 border-primary size-8 bg-background hover:bg-background-dark">
                    <XIcon />
                </div>
                <component :is="options.component" v-bind="options.componentProps" />
            </div>
        </div>
    </Transition>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import { XIcon } from 'lucide-vue-next';
import { type IShowModalEvent, useModal } from '@/components/modals/Modal.use';

const modal = useModal();

const options = ref<IShowModalEvent | null>(null);

onMounted(() => {
    modal.subscribeOnShow(event => {
        options.value = event;
    })

    modal.subscribeOnHide(() => {
        options.value = null;
    });
});

const onClose = function (): void {
    modal.hide();
};
</script>

<style lang="scss" scoped>
.v-enter-from,
.v-leave-to {
    opacity: 0;

    & > * {
        transform: translateX(100%);
    }
}
</style>