import { ref } from 'vue';

import { Position } from '@/class/Position.class';

const mousePosition = ref<Position>(Position.ZERO);
const prevMousePosition = ref<Position>(Position.ZERO);

const isDragMoving = ref<boolean>(false);
const isDragSelecting = ref<boolean>(false);

const selectStart = ref<Position | null>(null);
const selectEnd = ref<Position | null>(null);

export const useMouse = function () {
    return {

        mousePosition,
        prevMousePosition,

        isDragMoving,
        isDragSelecting,

        selectStart,
        selectEnd,

    };
};