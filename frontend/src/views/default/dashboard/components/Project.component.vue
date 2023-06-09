<template>
    <CardComponent class="project-component text-centered" hoverable @click="onClick" header>
        <template #header>
            <h3>{{ project.pattern.title }}</h3>
        </template>
        <PatternImageComponent :image="project.pattern.bannerImageUrl" />
        <p>
            <IconComponent icon="user" gap="right" />
            <span class="created-by">{{ project.pattern.creator?.name }}</span>
        </p>
        <div class="flex gap">
            <RouterLink class="flex" :to="`/projects/${project.pattern.reference}/analytics`" @click.stop="">
                <ButtonComponent class="secondary">
                    <IconComponent icon="activity" gap="right" />
                    <span>Analytics</span>
                </ButtonComponent>
            </RouterLink>
            <div v-if="project.pattern.externalShopUrl" class="flex">
                <a class="flex" :href="project.pattern.externalShopUrl" @click.stop="">
                    <ButtonComponent>
                        <IconComponent icon="cart" gap="right" />
                        <span>Shop Link</span>
                    </ButtonComponent>
                </a>
            </div>
        </div>
    </CardComponent>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';

import PatternImageComponent from '@/components/shared/PatternImage.component.vue';
import ProjectModal from '@/views/default/dashboard/modal/ProjectModal.component.vue';

import { useModal } from '@wjb/vue/use/modal.use';

import { IProject } from '@/models/Project.model';

const props = defineProps<{
    project: IProject;
}>();

const modal = useModal();
const router = useRouter();

const onClick = function (): void {
    router.push({ path: `/projects/${props.project.pattern.reference}` });
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

    .created-by {
        vertical-align: middle;
    }
}
</style>