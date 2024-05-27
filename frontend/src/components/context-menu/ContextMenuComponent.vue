<template>
    <div
        ref="contextMenuElement"
        v-if="schema !== null"
        class="context-menu-component fixed rounded-md shadow-xl opacity-0 pointer-events-none [&.is-visible]:opacity-100 [&.is-visible]:pointer-events-auto"
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

import ContextMenuItemComponent from '@/components/context-menu/ContextMenuItemComponent.vue';
import ContextMenuSeparatorComponent from '@/components/context-menu/ContextMenuSeparatorComponent.vue';

import { useEvent, useEvents } from '@/use/events/Events.use';

import { Position } from '@/class/Position.class';
import type { IContextMenuSchema } from '@/components/context-menu/types/ContextMenuSchema.type';
import type { IOpenContextMenuEvent } from '@/use/events/types/EventsMap.type';

const events = useEvents();

const contextMenuElement = ref<HTMLDivElement | null>(null);

const position = ref<Position>(Position.ZERO);
const isVisible = ref<boolean>(false);
const schema = ref<IContextMenuSchema | null>(null);

const onOpenContextMenu = function (event: MouseEvent): void {
    event.preventDefault();
};

const onDocumentClick = function (event: MouseEvent): void {
    if (contextMenuElement.value?.contains(event.target as Node | null))
        return;

    events.publish('CloseContextMenu', {});
};

onMounted(() => {
    document.addEventListener('mousedown', onDocumentClick);
});

onUnmounted(() => {
    document.addEventListener('mousedown', onDocumentClick);
});

useEvent('OpenContextMenu', (event: IOpenContextMenuEvent) => {
    schema.value = event.schema;
    position.value = Position.at(event.x + 3, event.y + 3);
    isVisible.value = true;
});

useEvent('CloseContextMenu', () => {
    isVisible.value = false;
});
</script>

<style lang="scss">
.context-menu-component {
    top: calc(var(--y) * 1px);
    left: calc(var(--x) * 1px);
    border: 1px solid var(--wjb-background-colour-dark);
    background-color: var(--wjb-background-colour);
    transition: opacity 0.1s;

    .header {
        font-weight: bold;
        padding: 0.25rem 0.5rem;
        border-bottom: 1px solid var(--wjb-primary);
        border-top-left-radius: var(--wjb-border-radius);
        border-top-right-radius: var(--wjb-border-radius);
    }
}
</style>