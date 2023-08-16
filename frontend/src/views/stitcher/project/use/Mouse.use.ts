import { ref } from 'vue';

import { Position } from '@/class/Position.class';

export const useMouse = function () {
    const mousePosition = ref<Position>(Position.ZERO);
    const prevMousePosition = ref<Position>(Position.ZERO);

    const isDragMoving = ref<boolean>(false);
    const isDragSelecting = ref<boolean>(false);

    const selectStart = ref<Position | null>(null);
    const selectEnd = ref<Position | null>(null);

    return {

        mousePosition,
        prevMousePosition,

        isDragMoving,
        isDragSelecting,

        selectStart,
        selectEnd,

    };
};