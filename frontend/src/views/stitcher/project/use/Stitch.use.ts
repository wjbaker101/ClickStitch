import { computed } from 'vue';

import { useMouse } from '@/views/stitcher/project/use/Mouse.use';
import { useTransformation } from '@/views/stitcher/project/use/Transformation.use';
import { useCurrentProject } from '@/views/stitcher/project/use/CurrentProject.use';

import { Position } from '@/class/Position.class';

const { mousePosition, selectStart, selectEnd } = useMouse();
const { offset, scale } = useTransformation();
const { project } = useCurrentProject();

export const useStitch = function () {

    const baseStitchSize = 60;
    const scaledStitchSize = computed<number>(() => baseStitchSize * scale.value);

    const viewportToStitchPosition = function (position: Position): Position {
        return position
            .translate(-offset.value.x, -offset.value.y)
            .scale(1.0 / scaledStitchSize.value, 1.0 / scaledStitchSize.value)
            .floor()
    };

    const mouseStitchPosition = computed<Position>(() => viewportToStitchPosition(mousePosition.value));

    const isMouseOverPattern = computed<boolean>(() => {
        if (project.value === null)
            return false;

        return mouseStitchPosition.value.x >= 0 &&
            mouseStitchPosition.value.y >= 0 &&
            mouseStitchPosition.value.x < project.value.project.pattern.width &&
            mouseStitchPosition.value.y < project.value.project.pattern.height;
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
        if (project.value === null || selectStart.value === null)
            return null;

        const x = Math.max(selectStart.value.x, endX.value);
        const y = Math.max(selectStart.value.y, endY.value);

        return Position
            .at(x, y)
            .min(0, 0)
            .max(project.value.project.pattern.width - 1, project.value.project.pattern.height - 1);
    });

    return {
        baseStitchSize,
        scaledStitchSize,
        mouseStitchPosition,
        isMouseOverPattern,
        stitchSelectStart,
        stitchSelectEnd,

        viewportToStitchPosition,
    };
};