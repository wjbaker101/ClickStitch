<template>
    <div class="display-pattern-component tw-grid tw-mt-4 tw-ml-4 tw-rounded-b-md tw-shadow-md">
        <div class="image-container tw-w-[150px] tw-aspect-square tw-relative -tw-top-4 -tw-left-4 -tw-mb-4 tw-shadow-md tw-overflow-hidden tw-isolate" :style="{ '--image': `url(${pattern.bannerImageUrl}` ?? '' }">
            <img class="tw-w-full tw-h-auto tw-align-middle" :src="pattern.bannerImageUrl ?? ''" :alt="pattern.title">
        </div>
        <div>
            <h2 class="tw-my-4">{{ pattern.title }}</h2>
            <div class="tw-pb-4 tw-text-gray-500">
                {{ pattern.width }}&times;{{ pattern.height }}
                <br>
                {{ formatNumber(pattern.stitchCount) }} stitches
                <br>
                <RouterLink v-if="pattern.creator !== null" class="created-by" :to="`/creators/${pattern.creator.reference}`">
                    <IconComponent icon="user" gap="right" />
                    <span class="tw-underline hover:tw-no-underline">{{ pattern.creator?.name ?? 'You' }}</span>
                </RouterLink>
                <span v-else class="created-by">
                    <IconComponent icon="user" gap="right" />
                    <span class="tw-align-middle">You</span>
                </span>
            </div>
        </div>
        <div v-if="project" class="tw-grid tw-grid-cols-2 tw-gap-4 tw-px-4 tw-pb-4 tw-col-start-1 tw-col-end-3">
            <RouterLink :to="`/projects/${pattern.reference}`">
                <ButtonComponent class="primary tw-w-full">
                    <IconComponent icon="play" gap="right" />
                    <span>Stitch!</span>
                </ButtonComponent>
            </RouterLink>
            <RouterLink :to="`/projects/${pattern.reference}/analytics`">
                <ButtonComponent class="secondary tw-w-full">
                    <IconComponent icon="activity" gap="right" />
                    <span>Analytics</span>
                </ButtonComponent>
            </RouterLink>
        </div>
        <div v-else class="tw-px-4 tw-pb-4 tw-col-start-1 tw-col-end-3">
            <ButtonComponent class="tw-w-full" v-if="!userHasPattern" title="Add to Your Dashboard" @click="onAddProject(pattern)">
                <IconComponent icon="plus" gap="right" />
                <span>Add to Dashboard</span>
            </ButtonComponent>
            <div class="added tw-p-2 tw-text-center tw-rounded-md" v-else>
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