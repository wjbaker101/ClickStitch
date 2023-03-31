<template>
    <div class="stitch-count-component">
        <div class="thread" v-for="thread in palette.values()" :class="{ 'is-hovered': thread.index === hoveredStitch?.thread.index }">
            <div class="thread-colour" :style="{ 'background-color': thread.colour }"></div><small>{{ thread.description }} <strong>({{ 0 }}/{{ 0 }})</strong></small>
        </div>
    </div>
</template>

<script setup lang="ts">
import { useGlobalData } from '@/use/global-data/global-data.use';

import { IThread } from '@/model/thread.model';
import { IGetProject } from '@/models/GetProject.model';

const props = defineProps<{
    project: IGetProject;
}>();

const palette = new Map<number, IThread>();
for (const thread of props.project.threads) {
    palette.set(thread.index, thread);
}


const globalData = useGlobalData();

const hoveredStitch = globalData.hoveredStitch;
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