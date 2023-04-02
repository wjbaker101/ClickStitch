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
            <div>
                <RouterLink :to="`/project/${project.pattern.reference}`" @click.native="onOpenInEditor">
                    <ButtonComponent>
                        <IconComponent icon="external-link" gap="right" />
                        <span>Open in editor</span>
                    </ButtonComponent>
                </RouterLink>
            </div>
        </div>
        <div v-if="getProject === null">
            <LoadingComponent itemName="extra details" />
        </div>
        <div v-else>
            <h3>Kit Details:</h3>
            <p>
                Suggested aida count: <strong>{{ getProject.aidaCount }}</strong>
                <br>
                Suggested fabric size: <strong>{{ fabricSize.in.width }} &times; {{ fabricSize.in.height }}</strong> inches (<strong>{{ fabricSize.cm.width }} &times; {{ fabricSize.cm.height}}</strong> cm)
            </p>
            <table class="hoverable">
                <thead>
                    <tr>
                        <th>Thread Code</th>
                        <th>Description</th>
                        <th>Count</th>
                        <th>Skeins*</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="threadCount in sortedThreadCounts">
                        <td><strong>{{ threadCount.thread.name }}</strong></td>
                        <td>{{ threadCount.thread.description }}</td>
                        <td class="thread-count">{{ threadCounts.get(threadCount.thread.index)?.count }}</td>
                        <td>{{ calculateSkeins(16, threadCount.count) }}</td>
                    </tr>
                </tbody>
            </table>
            <p>
                * Approximate, based on suggested aida count and using 2 strands of thread
                <br>
                <strong>It is recommended to buy extra skeins for high stitch count colours</strong>
            </p>
        </div>
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import { api } from '@/api/api';
import { calculateFabricSize, calculateSkeins, IFabricSize } from '@/helper/stitch.helper';
import { formatNumber } from '@/helper/helper';
import { useModal } from '@wjb/vue/use/modal.use';

import { IProject } from '@/models/Project.model';
import { IThread } from '@/models/Pattern.model';
import { IGetProject } from '@/models/GetProject.model';

const props = defineProps<{
    project: IProject;
}>();

const modal = useModal();

interface IThreadCount {
    readonly thread: IThread;
    count: number;
}

const getProject = ref<IGetProject | null>(null);

const threadCounts = new Map<number, IThreadCount>();
const sortedThreadCounts = ref<Array<IThreadCount>>([]);

const fabricSize = ref<IFabricSize>({} as IFabricSize);

const onOpenInEditor = function (): void {
    modal.hide();
};

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

    sortedThreadCounts.value = Array.from(threadCounts.values()).sort((a, b) => b.count - a.count);

    fabricSize.value = calculateFabricSize(props.project.pattern.width, props.project.pattern.height, getProject.value.aidaCount);
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