<template>
    <div class="mt-4 ml-4 grid rounded-b-md border-0 border-t-2 border-solid shadow-md display-pattern-component bg-background border-primary grid-cols-[auto_1fr] grid-rows-[1fr_auto]">
        <div
            :style="{ '--image': `url(${pattern.bannerImageUrl})` ?? '' }"
            class="relative -top-4 -left-4 isolate -mb-4 aspect-square overflow-hidden rounded-md shadow-md image-container w-[150px] bg-background leading-[150px]
                before:absolute before:content-[''] before:inset-0 before:blur-sm before:bg-[url(),var(--image)] before:bg-cover before:-z-[1] before:bg-center before:bg-no-repeat"
        >
            <img class="inline-block h-auto w-full align-middle bg-background" :src="pattern.bannerImageUrl ?? ''" :alt="pattern.title">
        </div>
        <div class="overflow-hidden">
            <h3 class="my-4 text-lg font-bold">{{ pattern.title }}</h3>
            <div class="pb-4 text-gray-500">
                {{ pattern.width }}&times;{{ pattern.height }}
                <br>
                {{ formatNumber(pattern.stitchCount) }} stitches
                <br>
                <RouterLink v-if="pattern.creator !== null" class="text-colour" :to="`/creators/${pattern.creator.reference}`">
                    <UserIcon class="mr-1" />
                    <span class="align-middle underline hover:no-underline">{{ pattern.creator?.name ?? 'You' }}</span>
                </RouterLink>
                <span v-else class="created-by">
                    <UserIcon class="mr-1" />
                    <span class="align-middle">You</span>
                </span>
            </div>
        </div>
        <div v-if="project" class="col-start-1 col-end-3 grid grid-cols-2 gap-4 px-4 pb-4">
            <RouterLink :to="`/projects/${pattern.reference}`">
                <BtnComponent class="w-full primary">
                    <CirclePlayIcon class="mr-2" />
                    <span class="align-middle">Stitch!</span>
                </BtnComponent>
            </RouterLink>
            <RouterLink :to="`/projects/${pattern.reference}/analytics`">
                <BtnComponent class="w-full" type="secondary">
                    <LineChartIcon class="mr-2" />
                    <span class="align-middle">Analytics</span>
                </BtnComponent>
            </RouterLink>
        </div>
        <div v-else class="col-start-1 col-end-3 px-4 pb-4 text-center">
            <BtnComponent class="full" v-if="!userHasPattern" title="Add to Your Dashboard" @click="onAddProject(pattern)">
                <PlusIcon class="mr-2" />
                <span class="align-middle">Add to Dashboard</span>
            </BtnComponent>
            <div class="rounded-md border-2 border-dashed p-2 border-secondary" v-else>
                <CheckIcon class="mr-2" />
                <span>In your Dashboard</span>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';

import { UserIcon, CirclePlayIcon, LineChartIcon, PlusIcon, CheckIcon } from 'lucide-vue-next';
import BtnComponent from '@/components/BtnComponent.vue';

import { api } from '@/api/api';
import { formatNumber } from '@/helper/helper';
import { useAuth } from '@/use/auth/Auth.use';
import { usePopup } from '@/components/popup/Popup.use';

import type { IPattern } from '@/models/Pattern.model';
import type { IProject } from '@/models/Project.model';

const props = defineProps<{
    pattern: IPattern;
    project?: IProject;
    userHasPattern: boolean;
}>();

const auth = useAuth();
const popup = usePopup();
const router = useRouter();

const onAddProject = async function (pattern: IPattern): Promise<void> {
    if (auth.details.value === null) {
        popup.message('To add this pattern, please sign up!');
        await router.push({ path: '/signup' });
        return;
    }

    const result = await api.projects.add(pattern.reference);

    if (result instanceof Error) {
        popup.error(result.message);
        return;
    }

    popup.success(`${props.pattern.title} has been added to your dashboard!`);
    await router.push({ path: '/dashboard' });
};
</script>