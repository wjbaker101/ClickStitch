import { ref, watch } from 'vue';

import { useEvents } from '@/use/events/Events.use';

const events = useEvents();

const threadIndex = ref<number | null>(null);

watch(threadIndex, () => {
    events.publish('HighlightThread', {
        threadIndex: threadIndex.value,
    });
});

export const useHighlightedThread = function() {
    return {
        threadIndex,
    };
};