import { ref } from 'vue';

const stitches = ref<boolean>(true);
const backStitches = ref<boolean>(true);

export const useLayers = function () {
    return {
        stitches,
        backStitches,
    };
};