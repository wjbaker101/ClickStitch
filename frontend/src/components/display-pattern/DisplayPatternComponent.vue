<template>
    <div class="mt-4 ml-4 grid rounded-b-md shadow-md display-pattern-component">
        <div class="relative -top-4 -left-4 isolate -mb-4 aspect-square overflow-hidden shadow-md image-container w-[150px]" :style="{ '--image': `url(${pattern.bannerImageUrl}` ?? '' }">
            <img class="h-auto w-full align-middle" :src="pattern.bannerImageUrl ?? ''" :alt="pattern.title">
        </div>
        <div class="overflow-hidden">
            <h2 class="my-4">{{ pattern.title }}</h2>
            <div class="pb-4 text-gray-500">
                {{ pattern.width }}&times;{{ pattern.height }}
                <br>
                {{ formatNumber(pattern.stitchCount) }} stitches
                <br>
                <RouterLink v-if="pattern.creator !== null" class="created-by" :to="`/creators/${pattern.creator.reference}`">
                    <IconComponent icon="user" gap="right" />
                    <span class="underline hover:no-underline">{{ pattern.creator?.name ?? 'You' }}</span>
                </RouterLink>
                <span v-else class="created-by">
                    <IconComponent icon="user" gap="right" />
                    <span class="align-middle">You</span>
                </span>
            </div>
        </div>
        <div v-if="project" class="col-start-1 col-end-3 grid grid-cols-2 gap-4 px-4 pb-4">
            <RouterLink :to="`/projects/${pattern.reference}`">
                <ButtonComponent class="w-full primary">
                    <IconComponent icon="play" gap="right" />
                    <span>Stitch!</span>
                </ButtonComponent>
            </RouterLink>
            <RouterLink :to="`/projects/${pattern.reference}/analytics`">
                <ButtonComponent class="w-full secondary">
                    <IconComponent icon="activity" gap="right" />
                    <span>Analytics</span>
                </ButtonComponent>
            </RouterLink>
        </div>
        <div v-else class="col-start-1 col-end-3 px-4 pb-4">
            <ButtonComponent class="full" v-if="!userHasPattern" title="Add to Your Dashboard" @click="onAddProject(pattern)">
                <IconComponent icon="plus" gap="right" />
                <span>Add to Dashboard</span>
            </ButtonComponent>
            <div class="rounded-md p-2 text-center added" v-else>
                <IconComponent icon="tick" gap="right" />
                <span>In your Dashboard</span>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';

import { api } from '@/api/api';
import { formatNumber } from '@/helper/helper';
import { usePopup } from '@wjb/vue/use/popup.use';

import type { IPattern } from '@/models/Pattern.model';
import type { IProject } from '@/models/Project.model';

const props = defineProps<{
    pattern: IPattern;
    project?: IProject;
    userHasPattern: boolean;
}>();

const popup = usePopup();
const router = useRouter();

const onAddProject = async function (pattern: IPattern): Promise<void> {
    await api.projects.add(pattern.reference);

    popup.trigger({
        message: `${props.pattern.title} has been added to your dashboard!`,
        style: 'success',
    });

    await router.push({ path: '/dashboard' });
};
</script>

<style lang="scss">
.display-pattern-component {
    grid-template-columns: auto 1fr;
    grid-template-rows: 1fr auto;
    background-color: var(--wjb-background-colour);
    border-top: 2px solid var(--wjb-primary);

    .image-container {
        line-height: 150px;
        border-radius: var(--wjb-border-radius);
        background-color: var(--wjb-background-colour);

        &::before {
            content: '';
            position: absolute;
            inset: 0;
            background-image: var(--image);
            background-position: center;
            background-repeat: no-repeat;
            background-size: cover;
            filter: blur(5px);
            z-index: -1;
        }
    }

    img {
        background-color: var(--wjb-background-colour);
    }

    .created-by {
        color: var(--wjb-text-colour);
    }

    .added {
        border: 2px dashed var(--wjb-secondary);
    }
}
</style>