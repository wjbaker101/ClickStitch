<template>
    <div
        class="list-item-component"
        :class="{
            'is-hoverable': hover,
        }"
    >
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    hover?: boolean;
}>();
</script>

<style lang="scss">
.list-item-component {
    padding: 1rem;
    background-color: var(--wjb-background-colour-light);
    border-radius: var(--wjb-border-radius);

    & + .list-item-component {
        margin-top: 0.5rem;
    }

    &.hover:hover {
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1), 0 4px 12px -6px rgba(0, 0, 0, 0.2);
    }
}
</style>