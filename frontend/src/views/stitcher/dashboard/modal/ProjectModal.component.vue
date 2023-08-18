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
                <RouterLink :to="`/projects/${project.pattern.reference}`" @click.native="closeModal">
                    <ButtonComponent>
                        <IconComponent icon="external-link" gap="right" />
                        <span>Open in editor</span>
                    </ButtonComponent>
                </RouterLink>
                <p></p>
                <a v-if="project.pattern.externalShopUrl !== null" :href="project.pattern.externalShopUrl" target="_blank">
                    <ButtonComponent>
                        <IconComponent icon="cart" gap="right" />
                        <span>Open Shop Link</span>
                    </ButtonComponent>
                </a>
                <p></p>
                <RouterLink :to="`/projects/${project.pattern.reference}/analytics`" @click.native="closeModal">
                    <ButtonComponent>
                        <IconComponent icon="activity" gap="right" />
                        <span>Analytics</span>
                    </ButtonComponent>
                </RouterLink>
            </div>
        </div>
        <!-- <UserMessageComponent ref="userMessageComponent" />
        <div v-if="isLoading">
            <LoadingComponent itemName="extra details" />
        </div>
        <div v-else-if="getProject !== null">
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
        </div> -->
    </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import UserMessageComponent from '@/components/UserMessage.component.vue';

import { api } from '@/api/api';
import { calculateFabricSize, calculateSkeins, type IFabricSize } from '@/helper/stitch.helper';
import { formatNumber } from '@/helper/helper';
import { useModal } from '@wjb/vue/use/modal.use';

import { type IProject } from '@/models/Project.model';
import { type IPatternThread } from '@/models/Pattern.model';
import { type IGetProject } from '@/models/GetProject.model';

const props = defineProps<{
    project: IProject;
}>();

const modal = useModal();

const userMessageComponent = ref<InstanceType<typeof UserMessageComponent>>({} as InstanceType<typeof UserMessageComponent>);

interface IThreadCount {
    readonly thread: IPatternThread;
    count: number;
}

const getProject = ref<IGetProject | null>(null);
const isLoading = ref<boolean>(false);

const threadCounts = new Map<number, IThreadCount>();
const sortedThreadCounts = ref<Array<IThreadCount>>([]);

const fabricSize = ref<IFabricSize>({} as IFabricSize);

const closeModal = function (): void {
    modal.hide();
};

onMounted(async () => {
    isLoading.value = true;

    const result = await api.projects.get(props.project.pattern.reference);

    isLoading.value = false;

    if (result instanceof Error) {
        userMessageComponent.value.set(result.message, true);
        return;
    }

    getProject.value = result;

    const palette = new Map<number, IPatternThread>();
    for (const thread of getProject.value.threads) {
        palette.set(thread.thread.index, thread.thread);
    }

    for (const thread of getProject.value.threads) {
        threadCounts.set(thread.thread.index, {
            thread: thread.thread,
            count: thread.stitches.length + thread.completedStitches.length,
        });
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