import { ref } from 'vue';

import { Position } from '@/class/Position.class';

const width = ref<number>(0);
const height = ref<number>(0);

const offset = ref<Position>(Position.ZERO);
const scale = ref<number>(0.5);

export const useTransformation = function () {
    const zoom = function (delta: number, centerX: number, centerY: number): void {
        const factor = delta < 0 ? 1.25 : 0.8;

        const newScale = scale.value * factor;
        if (newScale > 1.1 || newScale < 0.1)
            return;

        scale.value = newScale;

        const dx = (centerX - offset.value.x) * (factor - 1);
        const dy = (centerY - offset.value.y) * (factor - 1);

        offset.value = offset.value.translate(-dx, -dy);
    };

    return {
        width,
        height,

        offset,
        scale,

        zoom,
    };
};