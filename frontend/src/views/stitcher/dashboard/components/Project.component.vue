<template>
    <RouterLink class="project-component" :to="`/projects/${project.pattern.reference}`">
        <img :src="project.pattern.bannerImageUrl ?? ''" :alt="project.pattern.title">
        <h2>{{ project.pattern.title }}</h2>
        <p class="description">
            {{ project.pattern.width }}&times;{{ project.pattern.height }}
            <br>
            {{ formatNumber(project.pattern.stitchCount) }} stitches
            <br>
            <span class="created-by">
                <IconComponent icon="user" gap="right" />
                <span class="created-by-text">{{ project.pattern.creator?.name ?? 'You' }}</span>
            </span>
        </p>
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
    </RouterLink>
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

.project-component {

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

    img {
        float: left;
        margin-top: -$offset + -$padding;
        margin-left: -$offset;
        margin-right: 1rem;
        margin-bottom: 1rem;
        border-radius: var(--wjb-border-radius);
        background-color: var(--wjb-background-colour);
        box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);
    }

    .description {
        color: #777;
    }

    button {
        width: 100%;
    }
}
</style>