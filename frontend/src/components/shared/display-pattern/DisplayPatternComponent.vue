<template>
    <div class="display-pattern-component">
        <div class="image-container" :style="{ '--image': `url(${pattern.bannerImageUrl}` ?? '' }">
            <img :src="pattern.bannerImageUrl ?? ''" :alt="pattern.title">
        </div>
        <div>
            <h2>{{ pattern.title }}</h2>
            <div class="description">
                {{ pattern.width }}&times;{{ pattern.height }}
                <br>
                {{ formatNumber(pattern.stitchCount) }} stitches
                <br>
                <span class="created-by">
                    <IconComponent icon="user" gap="right" />
                    <span class="created-by-text">{{ pattern.creator?.name ?? 'You' }}</span>
                </span>
            </div>
        </div>
        <div v-if="project" class="actions flex gap">
            <RouterLink :to="`/projects/${pattern.reference}`" @click.stop="">
                <ButtonComponent class="primary">
                    <IconComponent icon="play" gap="right" />
                    <span>Stitch!</span>
                </ButtonComponent>
            </RouterLink>
            <RouterLink :to="`/projects/${pattern.reference}/analytics`" @click.stop="">
                <ButtonComponent class="secondary">
                    <IconComponent icon="activity" gap="right" />
                    <span>Analytics</span>
                </ButtonComponent>
            </RouterLink>
        </div>
    </div>
</template>

<script setup lang="ts">
import { formatNumber } from '@/helper/helper';

import type { IPattern } from '@/models/Pattern.model';
import type { IProject } from '@/models/Project.model';

defineProps<{
    pattern: IPattern;
    project?: IProject;
}>();
</script>

<style lang="scss">
$offset: 1rem;

.display-pattern-component {
    display: grid;
    grid-template-columns: auto 1fr;
    grid-template-rows: 1fr auto;
    margin-top: $offset;
    margin-left: $offset;
    border-bottom-left-radius: var(--wjb-border-radius);
    border-bottom-right-radius: var(--wjb-border-radius);
    background-color: var(--wjb-background-colour);
    border-top: 2px solid var(--wjb-primary);
    box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);

    .image-container {
        width: 150px;
        aspect-ratio: 1;
        position: relative;
        top: -$offset;
        left: -$offset;
        margin-bottom: -$offset;
        line-height: 150px;
        border-radius: var(--wjb-border-radius);
        background-color: var(--wjb-background-colour);
        box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);
        overflow: hidden;
        isolation: isolate;

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

    h2 {
        margin: 1rem 0;
    }

    img {
        max-width: 100%;
        height: auto;
        vertical-align: middle;
        background-color: var(--wjb-background-colour);
    }

    .actions {
        padding: 0 1rem 1rem 1rem;
        grid-column-start: 1;
        grid-column-end: 3;
    }

    .description {
        color: #777;
        padding-bottom: 1rem;
    }

    .created-by {
        color: var(--wjb-text-colour);
    }

    button {
        width: 100%;
    }
}
</style>