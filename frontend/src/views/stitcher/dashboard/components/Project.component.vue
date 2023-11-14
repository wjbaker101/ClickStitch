<template>
    <CardComponent class="project-component text-centered" hoverable @click="onClick" header>
        <template #header>
            <h3>{{ project.pattern.title }}</h3>
        </template>
        <PatternImageComponent :image="project.pattern.bannerImageUrl" />
        <p>
            <IconComponent icon="user" gap="right" />
            <span class="created-by">{{ project.pattern.creator?.name ?? 'You' }}</span>
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

import { type IProject } from '@/models/Project.model';

const props = defineProps<{
    project: IProject;
}>();

const router = useRouter();

const onClick = function (): void {
    router.push({ path: `/projects/${props.project.pattern.reference}` });
};
</script>

<style lang="scss">
@use '@/style/variables' as *;

.project-component {
    padding: 1rem;
    color: inherit;
    text-decoration: inherit;

    .created-by {
        vertical-align: middle;
    }
}
</style>