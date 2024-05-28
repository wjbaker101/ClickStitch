<template>
    <ListItemComponent :style="{ '--colour': thread.thread.colour }" :class="{ 'is-dark': isDark(thread.thread.colour) }" class="shadow-md thread-item-component text-dark [&.is-dark]:text-light">
        <div class="flex items-center gap-4">
            <div class="flex-auto p-2 rounded-full bg-[var(--colour)] text-inherit shadow-md">
                <IconComponent icon="skein" size="large" class="size-8" />
            </div>
            <div>
                <strong>{{ thread.thread.brand }} {{ thread.thread.code }}</strong>
            </div>
            <div class="flex-auto">
                <input type="number" min="0" max="9999" step="1" v-model="count" class="w-28">
            </div>
        </div>
    </ListItemComponent>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { watchDebounced } from '@vueuse/core';

import ListItemComponent from '@/components/ListItemComponent.vue';

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
}
</style>