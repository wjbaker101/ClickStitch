<template>
    <CardComponent class="project-component text-centered" hoverable @click="onClick" header>
        <template #header>
            <h3>{{ project.pattern.title }}</h3>
        </template>
        <PatternImageComponent :image="project.pattern.bannerImageUrl" />
        <p></p>
        <div class="flex gap">
            <RouterLink class="flex" :to="`/project/${project.pattern.reference}/analytics`" @click.stop="">
                <ButtonComponent class="secondary">
                    <IconComponent icon="activity" gap="right" />
                    <span>Analytics</span>
                </ButtonComponent>
            </RouterLink>
            <div class="flex">
                <ButtonComponent @click.stop="onDetails">
                    <IconComponent icon="info" gap="right" />
                    <span>Details</span>
                </ButtonComponent>
            </div>
        </div>
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

    img {
        max-width: 100%;
        height: auto;
        border-radius: var(--wjb-border-radius);
        box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);
    }
}
</style>