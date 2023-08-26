<template>
    <ListItemComponent class="thread-item-component">
        <div class="flex gap align-items-center">
            <div class="flex-auto">
                <img class="thread-image" src="@/assets/skein-icon.png">
            </div>
            <div>
                <strong>{{ thread.thread.brand }} {{ thread.thread.code }}</strong>
            </div>
            <div class="flex-auto">
                <input class="counter" type="number" min="0" max="9999" v-model="count">
            </div>
        </div>
    </ListItemComponent>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { watchDebounced } from '@vueuse/core';

import ListItemComponent from '@/components/ListItem.component.vue';

import { api } from '@/api/api';

import type { IInventoryThread } from '@/models/Inventory.model';

const props = defineProps<{
    thread: IInventoryThread;
}>();

const count = ref<number>(props.thread.count);

watchDebounced(count, async () => {
    const result = await api.inventory.updateThread(props.thread.thread.reference, {
        count: count.value,
    });
}, {
    debounce: 500,
});
</script>

<style lang="scss">
.thread-item-component {

    .thread-image {
        padding: 1rem;
    }

    .counter {
        width: 7rem;
    }
}
</style>