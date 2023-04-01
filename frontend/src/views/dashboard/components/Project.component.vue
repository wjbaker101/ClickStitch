<template>
    <CardComponent class="project-component text-centered" border="top" hoverable @click="onClick">
        <img :src="project.pattern.thumbnailUrl">
        <p></p>
        <strong>{{ project.pattern.title }}</strong>
        <p>{{ project.pattern.width }} &times; {{ project.pattern.height }}</p>
        <ButtonComponent class="mini" @click.stop="onDetails">
            <IconComponent icon="info" gap="right" />
            <span>Details</span>
        </ButtonComponent>
    </CardComponent>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';

import { IProject } from '@/models/Project.model';

const props = defineProps<{
    project: IProject;
}>();

const router = useRouter();

const onClick = function (): void {
    router.push({ path: `/project/${props.project.pattern.reference}` });
};

const onDetails = function (): void {};
</script>

<style lang="scss">
@use '@/style/variables' as *;

.project-component {
    padding: 1rem;
    color: inherit;
    text-decoration: inherit;

    img {
        border-radius: var(--wjb-border-radius);
        vertical-align: middle;

        @include shadow-small();
    }
}
</style>