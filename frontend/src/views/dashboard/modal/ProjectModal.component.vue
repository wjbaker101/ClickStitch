<template>
    <div class="project-modal-component">
        <h2>{{ project.pattern.title }}</h2>
        <div class="flex gap align-items-center">
            <div class="flex-auto">
                <img :src="project.pattern.thumbnailUrl">
            </div>
            <div>
                <strong>Size:</strong> {{ project.pattern.width }}&times;{{ project.pattern.height }}
                <br>
                <strong>Threads:</strong> {{ project.pattern.threadCount }}
                <br>
                <strong>Stitches:</strong> {{ formatNumber(project.pattern.stitchCount) }}
            </div>
        </div>
        <div v-if="getProject === null">
            <LoadingComponent itemName="extra details" />
        </div>
        <div v-else>
            <h3>Threads:</h3>
            <ul class="threads">
                <li v-for="threadCount in threadCounts.values()">
                    <strong>{{ threadCount.thread.name }}</strong> &mdash; {{ threadCount.thread.description }} <strong class="thread-count">({{ threadCounts.get(threadCount.thread.index)?.count }})</strong>
                </li>
            </ul>
        </div>
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import { api } from '@/api/api';
import { formatNumber } from '@/helper/helper';

import { IProject } from '@/models/Project.model';
import { IThread } from '@/models/Pattern.model';
import { IGetProject } from '@/models/GetProject.model';

const props = defineProps<{
    project: IProject;
}>();

interface IThreadCount {
    readonly thread: IThread;
    count: number;
}

const getProject = ref<IGetProject | null>(null);

const threadCounts = new Map<number, IThreadCount>();

onMounted(async () => {
    const result = await api.projects.get(props.project.pattern.reference);
    if (result instanceof Error)
        return;

    getProject.value = result;

    const palette = new Map<number, IThread>();
    for (const thread of getProject.value.threads) {
        palette.set(thread.index, thread);
    }

    for (const stitch of getProject.value.stitches) {
        if (!threadCounts.has(stitch.threadIndex)) {
            threadCounts.set(stitch.threadIndex, {
                thread: palette.get(stitch.threadIndex) as IThread,
                count: 0,
            });
        }

        const threadCount = threadCounts.get(stitch.threadIndex) as IThreadCount;
        threadCount.count ++;
    }
});
</script>

<style lang="scss">
@use '@/style/variables' as *;

.project-modal-component {
    width: 720px;
    max-width: 100%;

    img {
        border-radius: var(--wjb-border-radius);
        vertical-align: middle;

        @include shadow-small();
    }

    .thread-count {
        color: var(--wjb-primary);
    }
}
</style>