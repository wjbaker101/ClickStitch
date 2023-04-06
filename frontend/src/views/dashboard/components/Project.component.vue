<template>
    <CardComponent class="project-component text-centered" border="top" hoverable @click="onClick">
        <PatternImageComponent :pattern="project.pattern" />
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

import PatternImageComponent from '@/components/shared/PatternImage.component.vue';
import ProjectModal from '@/views/dashboard/modal/ProjectModal.component.vue';

import { useModal } from '@wjb/vue/use/modal.use';

import { IProject } from '@/models/Project.model';

const props = defineProps<{
    project: IProject;
}>();

const modal = useModal();
const router = useRouter();

const onClick = function (): void {
    router.push({ path: `/project/${props.project.pattern.reference}` });
};

const onDetails = function (): void {
    modal.show({
        component: ProjectModal,
        componentProps: {
            project: props.project,
        },
    });
};
</script>

<style lang="scss">
@use '@/style/variables' as *;

.project-component {
    padding: 1rem;
    color: inherit;
    text-decoration: inherit;
}
</style>