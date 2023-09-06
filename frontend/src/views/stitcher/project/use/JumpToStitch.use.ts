import { ref } from 'vue';

import type { IThreadDetails } from '@/models/GetProject.model';

const isEnabled = ref<boolean>(false);
const thread = ref<IThreadDetails | null>(null);

export const useJumpToStitches = function () {
    return {

        start(newThread: IThreadDetails): void {
            isEnabled.value = true;
            thread.value = newThread;
        },

        finish(): void {
            isEnabled.value = false;
            thread.value = null;
        },

    };
};