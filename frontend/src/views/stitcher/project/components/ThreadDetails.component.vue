<template>
    <ListItemComponent class="thread-details-component">
        <div class="flex align-items-center">
            <div class="flex-auto">
                <div class="thread-colour text-centered" :style="threadStyle(thread.thread)">
                    {{ thread.thread.index }}
                </div>
                <span class="thread-text">
                    <strong>{{ thread.thread.name }}</strong> - <small>{{ thread.thread.description }}</small>
                </span>
            </div>
            <div class="flex-auto" v-if="inventoryThread !== null && inventoryThread.count === 0" title="Not found in inventory">
                <IconComponent icon="warning" gap="left" />
            </div>
            <div></div>
            <div class="flex-auto">
                {{ thread.completedStitches.length }} <small>/ {{ thread.stitches.length }}</small>
            </div>
        </div>
        <template #expanded>
            <div class="thread-actions flex gap align-items-center">
                <div class="flex-auto">
                    <strong>{{ getPercentage(thread) }}%</strong> Completed
                </div>
                <div></div>
                <div class="flex-auto">
                    <ButtonComponent class="mini" title="Jump to Stitches" @click="onJumpToStitch">
                        <IconComponent icon="compass" gap="right" />
                        <span>Jump to Stitches</span>
                    </ButtonComponent>
                </div>
            </div>
            <div class="compare-inventory-container">
                <template v-if="inventoryThread === null"></template>
                <template v-else-if="inventoryThread.count > 0">
                    <IconComponent icon="tick-circle" gap="right" />
                    <span>Found in inventory ({{ inventoryThread.count }} skeins)</span>
                </template>
                <template v-else>
                    <IconComponent icon="warning" gap="right" />
                    <span>Not found in inventory</span>
                </template>
            </div>
        </template>
    </ListItemComponent>
</template>

<script setup lang="ts">
import { computed, type StyleValue } from 'vue';

import ListItemComponent from '@/components/ListItem.component.vue';

import { isDark } from '@/helper/helper';
import { useEvents } from '@/use/events/Events.use';
import { useModal } from '@wjb/vue/use/modal.use';

import type { IThreadDetails } from '@/models/GetProject.model';
import type { IPatternThread } from '@/models/Pattern.model';
import type { IGetPatternInventory } from '@/models/GetPatternInventory.model';

const props = defineProps<{
    thread: IThreadDetails;
    inventory: IGetPatternInventory | null;
}>();

const events = useEvents();
const modal = useModal();

const threadStyle = function (thread: IPatternThread): StyleValue {
    return {
        backgroundColor: thread.colour,
        color: isDark(thread.colour) ? '#fff' : '#000',
    };
};

const getPercentage = function (thread: IThreadDetails): string {
    return (thread.completedStitches.length / thread.stitches.length * 100).toFixed(1);
};

const onJumpToStitch = function (): void {
    events.publish('StartJumpToStitches', {
        thread: props.thread,
    });
    modal.hide();
};

const inventoryThread = computed(() => props.inventory?.threads.get(props.thread.thread.index) || null);
</script>

<style lang="scss">
.thread-details-component {

    .thread-colour {
        width: 2rem;
        line-height: 2rem;
        margin-right: 0.5rem;
        display: inline-block;
        aspect-ratio: 1;
        vertical-align: middle;
        border-radius: var(--wjb-border-radius);
        box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1), 0 3px 8px -6px rgba(0, 0, 0, 0.1);
    }

    .thread-text {
        vertical-align: middle;
    }

    .thread-actions,
    .compare-inventory-container {
        margin: 1rem;
    }
}
</style>