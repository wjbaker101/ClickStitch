import { onMounted, onUnmounted } from 'vue';

export const useInput = function <K extends keyof DocumentEventMap>(type: K, listener: (this: Document, ev: DocumentEventMap[K]) => any) {

    onMounted(() => document.addEventListener(type, listener));
    onUnmounted(() => document.removeEventListener(type, listener));

    return {};
};