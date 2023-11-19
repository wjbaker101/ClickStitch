import { computed, type Ref } from 'vue';

export const useCanvasElement = function (cavnas: Ref<HTMLCanvasElement>) {
    const graphics = computed<CanvasRenderingContext2D>(() => cavnas.value.getContext('2d') as CanvasRenderingContext2D);

    return {
        graphics,
    };
};