<template>
    <div class="project-component">
        <div class="image-container" :style="{ '--image': `url(${project.pattern.bannerImageUrl}` ?? '' }">
            <img :src="project.pattern.bannerImageUrl ?? ''" :alt="project.pattern.title">
        </div>
        <div>
            <h2>{{ project.pattern.title }}</h2>
            <div class="description">
                {{ project.pattern.width }}&times;{{ project.pattern.height }}
                <br>
                {{ formatNumber(project.pattern.stitchCount) }} stitches
                <br>
                <span class="created-by">
                    <IconComponent icon="user" gap="right" />
                    <span class="created-by-text">{{ project.pattern.creator?.name ?? 'You' }}</span>
                </span>
            </div>
        </div>
        <div class="actions flex gap">
            <RouterLink :to="`/projects/${project.pattern.reference}`" @click.stop="">
                <ButtonComponent class="primary">
                    <IconComponent icon="play" gap="right" />
                    <span>Stitch!</span>
                </ButtonComponent>
            </RouterLink>
            <RouterLink :to="`/projects/${project.pattern.reference}/analytics`" @click.stop="">
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

import { type IProject } from '@/models/Project.model';

defineProps<{
    project: IProject;
}>();
</script>

<style lang="scss">
@use '@/style/variables' as *;

$offset: 1rem;

.project-component {
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
        padding: 1rem;
        grid-column-start: 1;
        grid-column-end: 3;
    }

    .description {
        color: #777;
    }

    .created-by {
        color: var(--wjb-text-colour);
    }

    button {
        width: 100%;
    }
}

.project-component2 {

    $image-width: 150px;
    $offset: 2rem;
    $padding: 2rem;

    min-height: $image-width;
    margin-left: 1rem;
    margin-top: $offset;
    padding: $padding 1rem $padding 1rem;
    border-bottom-left-radius: var(--wjb-border-radius);
    border-bottom-right-radius: var(--wjb-border-radius);
    background-color: var(--wjb-background-colour);
    border-top: 2px solid var(--wjb-primary);
    box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);
    color: inherit;
    text-decoration: inherit;

    h2 {
        margin: 0;
    }

    .created-by {
        color: var(--wjb-text-colour);
    }

    .created-by-text {
        vertical-align: middle;
    }

    .image-container {
        width: 150px;
        aspect-ratio: 1;
        float: left;
        position: relative;
        line-height: 150px;
        margin-top: -$offset + -$padding;
        margin-left: -$offset;
        margin-right: 1rem;
        margin-bottom: 1rem;
        border-radius: var(--wjb-border-radius);
        background-color: var(--wjb-background-colour);
        box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);
        overflow: hidden;

        &::before {
            content: '';
            position: absolute;
            inset: 0;
            background-image: var(--image);
            background-position: center;
            background-repeat: no-repeat;
            background-size: cover;
            filter: blur(5px);
        }
    }

    img {
        max-width: 100%;
        height: auto;
        position: relative;
        z-index: 1;
        vertical-align: middle;
        background-color: var(--wjb-background-colour);
    }

    .description {
        color: #777;
    }

    button {
        width: 100%;
    }
}
</style>