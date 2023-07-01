<template>
    <div class="stitch-count-component" :class="{ 'is-open': isOpen }">
        <div class="thread" v-for="threadCount in sortedThreadCounts" :class="{ 'is-hovered': threadCount.thread.index === hoveredStitch?.threadIndex }">
            <div class="thread-colour" :style="{ 'background-color': threadCount.thread.colour }"></div><small>{{ threadCount.thread.description }} <strong>({{ threadCount.completeCount }}/{{ threadCount.count }})</strong></small>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed, watch } from 'vue';

import { sum } from '@/helper/array.helper';
import { useSharedStitch } from '@/views/default/project/use/SharedStitch';

import { IThread } from '@/models/Pattern.model';
import { IGetProject } from '@/models/GetProject.model';

const props = defineProps<{
    project: IGetProject;
    isOpen: boolean;
}>();

interface IThreadCount {
    readonly thread: IThread;
    count: number;
    completeCount: number;
}

const sharedStitch = useSharedStitch();

const hoveredStitch = sharedStitch.hoveredStitch;

const palette = new Map<number, IThread>();
for (const thread of props.project.threads) {
    palette.set(thread.thread.index, thread.thread);
}

const threadCounts = computed<Map<number, IThreadCount>>(() => {
    const _threadCounts = new Map<number, IThreadCount>();

    for (const thread of props.project.threads) {
        _threadCounts.set(thread.thread.index, {
            thread: thread.thread,
            count: thread.stitches.length + thread.completedStitches.length,
            completeCount: thread.completedStitches.length,
        });
    }

    return _threadCounts;
});

const sortedThreadCounts = computed<Array<IThreadCount>>(() => Array.from(threadCounts.value.values()).sort((a, b) => b.count - a.count));

watch(sortedThreadCounts, () => {
    sharedStitch.percentageCompleted.value = sum(sortedThreadCounts.value, x => x.completeCount) / sum(sortedThreadCounts.value, x => x.count) * 100;
}, { immediate: true });
</script>

<style lang="scss">
@use '@/style/variables' as *;

.stitch-count-component {
    position: fixed;
    padding: 1rem;
    inset: auto 1rem 5.5rem auto;
    color: var(--wjb-text-colour-opposite);
    background-color: var(--wjb-primary);
    background: linear-gradient(
        -5deg,
        transparentize($primary-dark, 0.05),
        transparentize($primary, 0.05),
    );
    backdrop-filter: blur(2px);
    border-left: 3px solid var(--wjb-tertiary);
    border-radius: var(--wjb-border-radius);
    z-index: 1;
    opacity: 0;
    pointer-events: none;

    @include shadow-large();

    &.is-open {
        opacity: 1;
        pointer-events: all;
    }

    .thread {
        border-radius: var(--wjb-border-radius);

        .thread-colour {
            width: 1rem;
            margin-right: 0.25rem;
            aspect-ratio: 1;
            display: inline-block;
            border-radius: var(--wjb-border-radius);
            vertical-align: middle;
        }

        &.is-hovered {
            @include shadow-small();
        }
    }

    @media screen and (max-width: 720px) {
        left: 50%;
        right: 1rem;
        left: 1rem;
    }
}
</style>