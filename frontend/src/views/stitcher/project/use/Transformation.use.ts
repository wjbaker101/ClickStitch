import { ref } from 'vue';

import { Position } from '@/class/Position.class';

export const useTransformation = function () {
    const width = ref<number>(0);
    const height = ref<number>(0);

    const offset = ref<Position>(Position.ZERO);
    const scale = ref<number>(0.5);

    return {
        width,
        height,

        offset,
        scale,
    };
};