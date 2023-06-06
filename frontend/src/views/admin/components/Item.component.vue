<template>
    <div class="item-component">
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
</script>

<style lang="scss">
.item-component {
    padding: 1rem;
    background-color: #f8f8f8;
    border-radius: var(--wjb-border-radius);

    & + .item-component {
        margin-top: 0.5rem;
    }

    &.hover:hover {
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1), 0 4px 12px -6px rgba(0, 0, 0, 0.2);
    }
}
</style>