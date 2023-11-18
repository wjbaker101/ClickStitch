<template>
    <div
        ref="contextMenuElement"
        v-if="schema !== null"
        class="context-menu-component"
        :class="{ 'is-visible': isVisible }"
        :style="{
            '--x': position.x,
            '--y': position.y,
        }"
        @contextmenu="onOpenContextMenu"
    >
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
import { onMounted, onUnmounted, ref } from 'vue';

import ContextMenuItemComponent from '@/components/context-menu/ContextMenuItem.component.vue';
import ContextMenuSeparatorComponent from '@/components/context-menu/ContextMenuSeparator.component.vue';

import { useEvents } from '@/use/events/Events.use';

import { Position } from '@/class/Position.class';
import type { IContextMenuSchema } from '@/components/context-menu/types/ContextMenuSchema.type';
import type { IOpenContextMenuEvent } from '@/use/events/types/EventsMap.type';

const events = useEvents();

const contextMenuElement = ref<HTMLDivElement>({} as HTMLDivElement);

const position = ref<Position>(Position.ZERO);
const isVisible = ref<boolean>(false);
const schema = ref<IContextMenuSchema | null>(null);

const onOpenContextMenu = function (event: MouseEvent): void {
    event.preventDefault();
};

const onDocumentClick = function (event: MouseEvent): void {
    if (contextMenuElement.value.contains(event.target as Node | null))
        return;

    isVisible.value = false;
};

onMounted(() => {
    events.subscribe('OpenContextMenu', (event: IOpenContextMenuEvent) => {
        schema.value = event.schema;
        position.value = Position.at(event.x + 3, event.y + 3);
        isVisible.value = true;
    });

    document.addEventListener('mousedown', onDocumentClick);
});

onUnmounted(() => {
    document.addEventListener('mousedown', onDocumentClick);
});
</script>

<style lang="scss">
.context-menu-component {
    position: fixed;
    top: calc(var(--y) * 1px);
    left: calc(var(--x) * 1px);
    border-radius: var(--wjb-border-radius);
    border: 1px solid var(--wjb-background-colour-dark);
    background-color: var(--wjb-background-colour);
    box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);
    opacity: 0;
    pointer-events: none;
    transition: opacity 0.1s;

    &.is-visible {
        opacity: 1;
        pointer-events: all;
    }

    .header {
        font-weight: bold;
        padding: 0.25rem 0.5rem;
        border-bottom: 1px solid var(--wjb-primary);
        border-top-left-radius: var(--wjb-border-radius);
        border-top-right-radius: var(--wjb-border-radius);
    }
}
</style>