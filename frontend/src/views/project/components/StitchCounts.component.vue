<template>
    <div class="stitch-count-component">
        <div class="thread" v-for="stitch in stitchCounts" :class="{ 'is-hovered': stitch[1].index === hoveredStitch?.thread.index }">
            <div class="thread-colour" :style="{ 'background-color': stitch[1].colour }"></div><small>{{ stitch[1].description }} <strong>({{ stitch[1].done }}/{{ stitch[1].total }})</strong></small>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

import { useCurrentProject } from '@/use/current-project/CurrentProject.use';
import { useGlobalData } from '@/use/global-data/global-data.use';
import { IThread } from '@/model/thread.model';

interface IStitchDetails {
    index: number;
    description: string;
    colour: string;
    done: number;
    total: number;
}

const currentProject = useCurrentProject();
const globalData = useGlobalData();

const project = currentProject.project;
const hoveredStitch = globalData.hoveredStitch;

const stitchCounts = computed(() => {
    const counts = new Map<number, IStitchDetails>();

    for (const stitch of project.value.canvas.stitches) {
        if (stitch.threadIndex === 0)
            continue;

        const thread = project.value.palette.threads.get(stitch.threadIndex) as IThread;

        const count = counts.get(stitch.threadIndex) ?? {
            index: stitch.threadIndex,
            description: thread.description,
            colour: thread.colour,
            done: 0,
            total: 0,
        };

        count.total++;
        if (stitch.isDone)
            count.done++;

        counts.set(stitch.threadIndex, count);
    }

    return Array.from(counts.entries()).sort((a, b) => b[1].total - a[1].total);
});
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