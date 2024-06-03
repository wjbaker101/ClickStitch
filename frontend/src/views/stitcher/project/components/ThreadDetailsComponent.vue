<template>
    <ListItemComponent class="thread-details-component" :isInitiallyOpen="isInitiallyOpen">
        <div class="flex align-items-center">
            <div class="flex-auto">
                <div class="mr-2 inline-grid place-items-center rounded-md text-center align-middle shadow-md size-8" :style="threadStyle(thread.thread)">
                    {{ thread.thread.index }}
                </div>
                <span class="align-middle">
                    <strong>{{ thread.thread.name }}</strong> - <small>{{ thread.thread.description }}</small>
                </span>
            </div>
            <div class="flex-auto text-warning" v-if="inventoryThread !== null && requireSkeinsDifference > 0" title="Not found in inventory">
                <IconComponent icon="warning" gap="left" />
            </div>
            <div></div>
            <div class="flex-auto">
                {{ completedCount }} <small>/ {{ inCompletedCount + completedCount }}</small>
            </div>
        </div>
        <template #expanded>
            <div class="m-4 flex items-center gap">
                <div class="flex-auto">
                    <strong>{{ percentageCompleted.toFixed(1) }}%</strong> Completed
                </div>
                <div></div>
                <div class="flex-auto">
                    <section>
                        <BtnComponent title="Jump to Stitches" @click="onJumpToStitch">
                            <IconComponent icon="compass" gap="right" />
                            <span class="align-middle">Jump to Stitches</span>
                        </BtnComponent>
                    </section>
                    <section>
                        <CheckBoxComponent label="Highlight" v-model="shouldHighlightThread" />
                    </section>
                </div>
            </div>
            <div class="m-4">
                <template v-if="inventoryThread === null"></template>
                <template v-else-if="inventoryThread.count === 0">
                    <IconComponent icon="tick-circle" gap="right" />
                    <span>Not found in inventory (<strong>{{ requiredSkeins }}</strong> skein{{ requiredSkeins > 1 ? 's' : '' }} recommended*)</span>
                </template>
                <template v-else-if="requireSkeinsDifference > 0">
                    <IconComponent icon="warning" gap="right" />
                    <span>Not enough in inventory (<strong>{{ requiredSkeins }}</strong> skein{{ requiredSkeins > 1 ? 's' : '' }} recommended*, found {{ inventoryThread.count }})</span>
                </template>
                <template v-else>
                    <IconComponent icon="tick-circle" gap="right" />
                    <span>Found in inventory</span>
                </template>
            </div>
        </template>
    </ListItemComponent>
</template>

<script setup lang="ts">
import { computed, type StyleValue } from 'vue';

import BtnComponent from '@/components/BtnComponent.vue';
import CheckBoxComponent from '@/components/inputs/CheckBoxComponent.vue';
import ListItemComponent from '@/components/ListItemComponent.vue';

import { isDark } from '@/helper/helper';
import { calculateRequiredSkeins } from '@/helper/stitch.helper';
import { useEvents } from '@/use/events/Events.use';
import { useModal } from '@/components/modals/Modal.use';
import { useHighlightedThread } from '../use/HighlightedThread.use';
import { useCurrentProject } from '@/views/stitcher/project/use/CurrentProject.use';

import type { IThreadDetails } from '@/models/GetProject.model';
import type { IPattern, IPatternThread } from '@/models/Pattern.model';
import type { IGetPatternInventory } from '@/models/GetPatternInventory.model';

const props = defineProps<{
    thread: IThreadDetails;
    inventory: IGetPatternInventory | null;
    pattern: IPattern;
}>();

const events = useEvents();
const modal = useModal();
const highlightedThread = useHighlightedThread();
const { stitches, backStitches } = useCurrentProject();

const highlightedThreadIndex = highlightedThread.threadIndex;
const isInitiallyOpen = highlightedThread.threadIndex.value === props.thread.thread.index;
const shouldHighlightThread = computed<boolean>({
    get: () => {
        return highlightedThreadIndex.value === props.thread.thread.index;
    },
    set: (isTrue) => {
        if (isTrue)
            highlightedThreadIndex.value = props.thread.thread.index;
        else
            highlightedThreadIndex.value = null;
    },
});

const currentStitches = computed(() => stitches.value.filter(x => x.threadIndex === props.thread.thread.index));
const currentBackStitches = computed(() => backStitches.value.filter(x => x.threadIndex === props.thread.thread.index));

const inCompletedCount = computed(() => {
    const stitches = currentStitches.value.filter(x => x.stitchedAt === null).length;
    const backStitches = currentBackStitches.value.filter(x => !x.isCompleted).length;

    return stitches + backStitches;
});

const completedCount = computed(() => {
    const stitches = currentStitches.value.filter(x => x.stitchedAt !== null).length;
    const backStitches = currentBackStitches.value.filter(x => x.isCompleted).length;

    return stitches + backStitches;
});

const percentageCompleted = computed<number>(() => {
    return completedCount.value / (inCompletedCount.value + completedCount.value) * 100;
});

const threadStyle = function (thread: IPatternThread): StyleValue {
    return {
        backgroundColor: thread.colour,
        color: isDark(thread.colour) ? '#fff' : '#000',
    };
};

const onJumpToStitch = function (): void {
    events.publish('StartJumpToStitches', {
        thread: props.thread,
    });
    modal.hide();
};

const inventoryThread = computed(() => props.inventory?.threads.get(props.thread.thread.index) || null);

const requiredSkeins = computed(() => calculateRequiredSkeins(inCompletedCount.value + completedCount.value, props.pattern.aidaCount));

const requireSkeinsDifference = computed(() => {
    if (inventoryThread.value === null)
        return 0;

    return requiredSkeins.value - inventoryThread.value.count;
});
</script>