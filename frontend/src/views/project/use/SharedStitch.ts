import { ref } from 'vue';

import { IStitch } from '@/models/Pattern.model';

const hoveredStitch = ref<IStitch | null>(null);
const percentageCompleted = ref<number | null>(null);

export const useSharedStitch = function () {
    return {
        hoveredStitch,
        percentageCompleted,
    };
};