<template>
    <div class="context-menu-item-component" @click.self="onClick">
        {{ item.text }}
    </div>
</template>

<script setup lang="ts">
import type { IContextMenuItem } from '@/components/context-menu/types/ContextMenuSchema.type';

import { useEvents } from '@/use/events/Events.use';

const props = defineProps<{
    item: IContextMenuItem;
}>();

const events = useEvents();

const onClick = function (): void {
    props.item.action();
    events.publish('CloseContextMenu', {});
};
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