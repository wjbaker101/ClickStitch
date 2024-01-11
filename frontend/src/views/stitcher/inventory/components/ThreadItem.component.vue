<template>
    <ListItemComponent class="thread-item-component" :style="{ '--colour': thread.thread.colour }" :class="{ 'is-dark': isDark(thread.thread.colour) }">
        <div class="flex gap align-items-center">
            <div class="icon-container flex-auto">
                <IconComponent icon="skein" size="large" />
            </div>
            <div>
                <strong>{{ thread.thread.brand }} {{ thread.thread.code }}</strong>
            </div>
            <div class="flex-auto">
                <input class="counter" type="number" min="0" max="9999" step="1" v-model="count">
            </div>
        </div>
    </ListItemComponent>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { watchDebounced } from '@vueuse/core';

import ListItemComponent from '@/components/ListItem.component.vue';

import { api } from '@/api/api';
import { isDark } from '@/helper/helper';

import type { IInventoryThread } from '@/models/Inventory.model';

const props = defineProps<{
    thread: IInventoryThread;
}>();

const emit = defineEmits(['update']);

const count = ref<number>(props.thread.count);

watchDebounced(count, async () => {
    const result = await api.inventory.updateThread(props.thread.thread.reference, {
        count: count.value,
    });

    emit('update');
}, {
    debounce: 500,
});
</script>

<style lang="scss">
.thread-item-component {
    background: linear-gradient(to right, var(--colour), var(--wjb-background-colour-light));
    box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);
    color: var(--wjb-dark);

    .thread-image {
        padding: 1rem;
    }

    .counter {
        width: 7rem;
    }

    .icon-container {
        padding: 0.5rem;
        border-radius: 50%;
        background-color: var(--colour);
        color: inherit;
        box-shadow: 1px 2px 6px rgba(0, 0, 0, 0.3);

        .icon-skein {
            width: 2rem;
            height: 2rem;
        }
    }

    &.is-dark {
        color: var(--wjb-light);
    }
}
</style>