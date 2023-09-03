<template>
    <ListItemComponent>
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
                <div>
                    <strong>{{ getPercentage(thread) }}%</strong> Completed
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
</style>