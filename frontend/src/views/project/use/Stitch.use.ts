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
    const baseStitchSize = 15;
    const stitchSize = computed<number>(() => Math.round(baseStitchSize * scale.value));

    const mouseStitchPosition = computed<Position>(() => mousePosition.value
        .translate(-offset.value.x, -offset.value.y)
        .scale(1.0 / stitchSize.value, 1.0 / stitchSize.value)
        .floor());

    const isMouseOverPattern = computed<boolean>(() => {
        return mouseStitchPosition.value.x > 0 &&
            mouseStitchPosition.value.y > 0 &&
            mouseStitchPosition.value.x < pattern.width &&
            mouseStitchPosition.value.y < pattern.height;
    });

    const stitchSelectStart = computed<Position | null>(() => {
        if (selectStart.value === null)
            return null;

        return Position.at(
            Math.min(selectStart.value.x, selectEnd.value?.x ?? mouseStitchPosition.value.x),
            Math.min(selectStart.value.y, selectEnd.value?.y ?? mouseStitchPosition.value.y));
    });

    const stitchSelectEnd = computed<Position | null>(() => {
        if (selectStart.value === null)
            return null;

        return Position.at(
            Math.max(selectStart.value.x, selectEnd.value?.x ?? mouseStitchPosition.value.x),
            Math.max(selectStart.value.y, selectEnd.value?.y ?? mouseStitchPosition.value.y));
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