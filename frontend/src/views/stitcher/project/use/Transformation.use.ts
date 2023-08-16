import { type Ref, onMounted, onUnmounted, ref } from 'vue';

import { Position } from '@/class/Position.class';

export const useTransformation = function (component: Ref<HTMLDivElement>) {
    const width = ref<number>(0);
    const height = ref<number>(0);

    const offset = ref<Position>(Position.ZERO);
    const scale = ref<number>(0.5);

    const onResize = function (): void {
        width.value = component.value.offsetWidth;
        height.value = component.value.offsetHeight;
    };

    onMounted(() => {
        onResize();
        window.addEventListener('resize', onResize);
    });

    onUnmounted(() => {
        window.removeEventListener('resize', onResize);
    });

    return {
        width,
        height,

        offset,
        scale,
    };
};