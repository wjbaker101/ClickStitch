import { Ref, computed } from 'vue';

import { Position } from '@/class/Position.class';
import { IPattern } from '@/models/Pattern.model';

export const useStitch = function ({ pattern, scale, mousePosition, offset, selectStart, selectEnd }: {
    pattern: IPattern;
    scale: Ref<number>;
    mousePosition: Ref<Position>;
    offset: Ref<Position>;
    selectStart: Ref<Position | null>,
    selectEnd: Ref<Position | null>,
}) {
    const baseStitchSize = 60;
    const stitchSize = computed<number>(() => baseStitchSize * scale.value);

    const mouseStitchPosition = computed<Position>(() => mousePosition.value
        .translate(-offset.value.x, -offset.value.y)
        .scale(1.0 / stitchSize.value, 1.0 / stitchSize.value)
        .floor());

    const isMouseOverPattern = computed<boolean>(() => {
        return mouseStitchPosition.value.x >= 0 &&
            mouseStitchPosition.value.y >= 0 &&
            mouseStitchPosition.value.x < pattern.width &&
            mouseStitchPosition.value.y < pattern.height;
    });

    const endX = computed(() => selectEnd.value?.x ?? mouseStitchPosition.value.x);
    const endY = computed(() => selectEnd.value?.y ?? mouseStitchPosition.value.y);

    const stitchSelectStart = computed<Position | null>(() => {
        if (selectStart.value === null)
            return null;

        return Position.at(
            Math.min(selectStart.value.x, endX.value),
            Math.min(selectStart.value.y, endY.value));
    });

    const stitchSelectEnd = computed<Position | null>(() => {
        if (selectStart.value === null)
            return null;

        const x = Math.max(selectStart.value.x, endX.value);
        const y = Math.max(selectStart.value.y, endY.value);

        return Position
            .at(x, y)
            .min(0, 0)
            .max(pattern.width - 1, pattern.height - 1);
    });

    return {
        baseStitchSize,
        stitchSize,
        mouseStitchPosition,
        isMouseOverPattern,
        stitchSelectStart,
        stitchSelectEnd,
    };
};