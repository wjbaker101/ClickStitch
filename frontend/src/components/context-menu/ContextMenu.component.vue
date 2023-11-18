<template>
    <div class="context-menu-component">
        <div class="header">
            {{ schema.header }}
        </div>
        <template v-for="item in schema.items">
            <ContextMenuItemComponent v-if="item.type === 'item'" :item="item" />
            <ContextMenuSeparatorComponent v-else-if="item.type === 'separator'" />
        </template>
    </div>
</template>

<script setup lang="ts">
import ContextMenuItemComponent from '@/components/context-menu/ContextMenuItem.component.vue';
import ContextMenuSeparatorComponent from '@/components/context-menu/ContextMenuSeparator.component.vue';

import type { IContextMenuSchema } from '@/components/context-menu/types/ContextMenuSchema.type';

defineProps<{
    schema: IContextMenuSchema;
}>();
</script>

<style lang="scss">
.context-menu-component {
    position: fixed;
    top: 50%;
    left: 50%;
    border-radius: var(--wjb-border-radius);
    border: 1px solid var(--wjb-background-colour-dark);
    background-color: var(--wjb-background-colour);
    box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);

    .header {
        font-weight: bold;
        padding: 0.25rem 0.5rem;
        border-bottom: 1px solid var(--wjb-primary);
        border-top-left-radius: var(--wjb-border-radius);
        border-top-right-radius: var(--wjb-border-radius);
    }
}
</style>