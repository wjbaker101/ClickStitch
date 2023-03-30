import { ref } from 'vue';

import { IHoveredStitch } from '@/use/global-data/type/hovered-stitch.type';

const hoveredStitch = ref<IHoveredStitch | null>(null);

export const useGlobalData = function () {
    return {
        hoveredStitch,
    };
}