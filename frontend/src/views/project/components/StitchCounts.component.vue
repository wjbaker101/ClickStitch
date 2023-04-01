<template>
    <div class="stitch-count-component">
        <div class="thread" v-for="threadCount in threadCounts.values()" :class="{ 'is-hovered': threadCount.thread.index === hoveredStitch?.threadIndex }">
            <div class="thread-colour" :style="{ 'background-color': threadCount.thread.colour }"></div><small>{{ threadCount.thread.description }} <strong>({{ 0 }}/{{ threadCount.count }})</strong></small>
        </div>
    </div>
</template>

<script setup lang="ts">
import { useSharedStitch } from '@/views/project/use/SharedStitch';

import { IThread } from '@/models/Pattern.model';
import { IGetProject } from '@/models/GetProject.model';

const props = defineProps<{
    project: IGetProject;
}>();

interface IThreadCount {
    readonly thread: IThread;
    count: number;
}

const sharedStitch = useSharedStitch();

const palette = new Map<number, IThread>();
for (const thread of props.project.threads) {
    palette.set(thread.index, thread);
}

const threadCounts = new Map<number, IThreadCount>();
for (const stitch of props.project.stitches) {
    if (!threadCounts.has(stitch.threadIndex)) {
        threadCounts.set(stitch.threadIndex, {
            thread: palette.get(stitch.threadIndex) as IThread,
            count: 0,
        });
    }

    const threadCount = threadCounts.get(stitch.threadIndex) as IThreadCount;
    threadCount.count++;
}

const hoveredStitch = sharedStitch.hoveredStitch;
</script>

<style lang="scss">
@use '@/style/variables' as *;

.stitch-count-component {
    position: fixed;
    padding: 1rem;
    left: 0.5rem;
    bottom: 0.5rem;
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

    @include shadow-large();

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
}
</style>