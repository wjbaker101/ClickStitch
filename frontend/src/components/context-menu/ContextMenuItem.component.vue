<template>
    <div class="context-menu-item-component" @click.self="item.action">
        {{ item.text }}
    </div>
</template>

<script setup lang="ts">
import type { IContextMenuItem } from '@/components/context-menu/types/ContextMenuSchema.type';

defineProps<{
    item: IContextMenuItem;
}>();
</script>

<style lang="scss">
.context-menu-item-component {
    position: relative;
    min-width: 120px;
    padding: 0.5rem;
    cursor: pointer;
    user-select: none;

    &:hover {
        background-color: var(--wjb-background-colour-dark);
    }
}
</style>