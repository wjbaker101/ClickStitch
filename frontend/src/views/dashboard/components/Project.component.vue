<template>
    <RouterLink class="project-component text-centered" :to="`/project/${project.pattern.reference}`">
        <CardComponent border="top">
            <img :src="project.pattern.thumbnailUrl">
            <p></p>
            <strong>{{ project.pattern.title }}</strong>
            <p>{{ project.pattern.width }} &times; {{ project.pattern.height }}</p>
        </CardComponent>
    </RouterLink>
</template>

<script setup lang="ts">
import { IProject } from '@/models/Project.model';

defineProps<{
    project: IProject;
}>();
</script>

<style lang="scss">
@use '@/style/variables' as *;

.project-component {
    color: inherit;
    text-decoration: inherit;

    .card-component {
        padding: 1rem;
    }

    img {
        border-radius: var(--wjb-border-radius);
        vertical-align: middle;

        @include shadow-small();
    }
}
</style>