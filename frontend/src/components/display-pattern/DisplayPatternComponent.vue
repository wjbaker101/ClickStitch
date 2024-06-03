<template>
    <div class="mt-4 ml-4 grid rounded-b-md border-0 border-t-2 border-solid shadow-md display-pattern-component bg-background border-primary grid-cols-[auto_1fr] grid-rows-[1fr_auto]">
        <div
            :style="{ '--image': `url(${pattern.bannerImageUrl})` ?? '' }"
            class="relative -top-4 -left-4 isolate -mb-4 aspect-square overflow-hidden rounded-md shadow-md image-container w-[150px] bg-background leading-[150px]
                before:absolute before:content-[''] before:inset-0 before:blur-sm before:bg-[url(),var(--image)] before:bg-cover before:-z-[1] before:bg-center before:bg-no-repeat"
        >
            <img class="h-auto w-full align-middle bg-background" :src="pattern.bannerImageUrl ?? ''" :alt="pattern.title">
        </div>
        <div class="overflow-hidden">
            <h2 class="my-4">{{ pattern.title }}</h2>
            <div class="pb-4 text-gray-500">
                {{ pattern.width }}&times;{{ pattern.height }}
                <br>
                {{ formatNumber(pattern.stitchCount) }} stitches
                <br>
                <RouterLink v-if="pattern.creator !== null" class="text-colour" :to="`/creators/${pattern.creator.reference}`">
                    <IconComponent icon="user" gap="right" />
                    <span class="align-middle underline hover:no-underline">{{ pattern.creator?.name ?? 'You' }}</span>
                </RouterLink>
                <span v-else class="created-by">
                    <IconComponent icon="user" gap="right" />
                    <span class="align-middle">You</span>
                </span>
            </div>
        </div>
        <div v-if="project" class="col-start-1 col-end-3 grid grid-cols-2 gap-4 px-4 pb-4">
            <RouterLink :to="`/projects/${pattern.reference}`">
                <BtnComponent class="w-full primary">
                    <IconComponent icon="play" gap="right" />
                    <span class="align-middle">Stitch!</span>
                </BtnComponent>
            </RouterLink>
            <RouterLink :to="`/projects/${pattern.reference}/analytics`">
                <BtnComponent class="w-full" type="secondary">
                    <IconComponent icon="activity" gap="right" />
                    <span class="align-middle">Analytics</span>
                </BtnComponent>
            </RouterLink>
        </div>
        <div v-else class="col-start-1 col-end-3 px-4 pb-4 text-center">
            <BtnComponent class="full" v-if="!userHasPattern" title="Add to Your Dashboard" @click="onAddProject(pattern)">
                <IconComponent icon="plus" gap="right" />
                <span>Add to Dashboard</span>
            </BtnComponent>
            <div class="rounded-md border-2 border-dashed p-2 border-secondary" v-else>
                <IconComponent icon="tick" gap="right" />
                <span>In your Dashboard</span>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';

import BtnComponent from '@/components/BtnComponent.vue';

import { api } from '@/api/api';
import { formatNumber } from '@/helper/helper';
import { usePopup } from '@/components/popup/Popup.use';

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

    popup.success(`${props.pattern.title} has been added to your dashboard!`);

    await router.push({ path: '/dashboard' });
};
</script>