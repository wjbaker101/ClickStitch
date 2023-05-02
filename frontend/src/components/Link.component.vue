<template>
    <a class="link-component" :href="href" target="_blank">
        <IconComponent v-if="external" icon="external-link" gap="right" />
        <slot></slot>
    </a>
</template>

<script setup lang="ts">
defineProps<{
    href: string;
    external?: boolean;
}>();
</script>

<style lang="scss">
.link-component {
    text-decoration: none;
    color: var(--wjb-primary-dark);
    box-shadow: 0 1px 0 var(--wjb-primary-dark);

    &:hover {
        box-shadow: 0 1px 0 transparent;
    }
}
</style>