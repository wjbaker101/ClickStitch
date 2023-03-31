import { ref } from 'vue';

import { Position } from '@/class/Position.class';

export const useTransformation = function () {
    const offset = ref<Position>(Position.ZERO);
    const scale = ref<number>(1);

    return {
        offset,
        scale,
    };
};