<template>
    <ListItemComponent class="thread-details-component">
        <div class="flex align-items-center">
            <div>
                <div class="thread-colour text-centered" :style="threadStyle(thread.thread)">
                    {{ thread.thread.index }}
                </div>
                <span class="thread-text">
                    <strong>{{ thread.thread.name }}</strong> - <small>{{ thread.thread.description }}</small>
                </span>
            </div>
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
                    <ButtonComponent class="mini" title="Jump to Stitches">
                        <IconComponent icon="compass" gap="right" />
                        <span>Jump to Stitches</span>
                    </ButtonComponent>
                </div>
            </div>
        </template>
    </ListItemComponent>
</template>

<script setup lang="ts">
import type { StyleValue } from 'vue';

import ListItemComponent from '@/components/ListItem.component.vue';

import { isDark } from '@/helper/helper';

import type { IThreadDetails } from '@/models/GetProject.model';
import type { IPatternThread } from '@/models/Pattern.model';

defineProps<{
    thread: IThreadDetails;
}>();

const threadStyle = function (thread: IPatternThread): StyleValue {
    return {
        backgroundColor: thread.colour,
        color: isDark(thread.colour) ? '#fff' : '#000',
    };
};

const getPercentage = function (thread: IThreadDetails): string {
    return (thread.completedStitches.length / thread.stitches.length * 100).toFixed(1);
};
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

    .thread-actions {
        padding: 1rem;
    }
}
</style>