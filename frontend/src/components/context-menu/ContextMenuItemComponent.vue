<template>
    <div @click.self="onClick" class="relative cursor-pointer select-none p-2 context-menu-item-component min-w-[120px] hover:bg-background-dark">
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