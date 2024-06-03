<template>
    <a v-if="external" :href="href" target="_blank" class="decoration-0 text-primary-dark shadow-[0_1px_0] shadow-primary-dark hover:shadow-transparent">
        <IconComponent v-if="external" icon="external-link" gap="right" />
        <slot></slot>
    </a>
    <RouterLink v-else :to="href" class="decoration-0 text-primary-dark shadow-[0_1px_0] shadow-primary-dark hover:shadow-transparent">
        <slot></slot>
    </RouterLink>
</template>

<script setup lang="ts">
defineProps<{
    href: string;
    external?: boolean;
}>();
</script>