<template>
    <ListItemComponent
        :style="{
            '--colour': thread.thread.colour,
        }"
        :class="{
            'is-dark': isDark(thread.thread.colour),
        }"
        class="shadow-md thread-item-component text-dark bg-gradient-to-r from-[var(--colour)] to-background-light [&.is-dark]:text-light"
    >
        <div class="flex items-center gap-4">
            <div class="p-2 rounded-full bg-[var(--colour)] text-inherit shadow-md">
                <SkeinIcon class="size-8" />
            </div>
            <div class="grow">
                <strong>{{ thread.thread.brand }} {{ thread.thread.code }}</strong>
            </div>
            <div>
                <TextboxComponent type="number" min="0" max="9999" step="1" v-model="count" class="w-28" />
            </div>
        </div>
    </ListItemComponent>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { watchDebounced } from '@vueuse/core';

import SkeinIcon from '@/components/icons/SkeinIcon.vue';
import ListItemComponent from '@/components/ListItemComponent.vue';
import TextboxComponent from '@/components/inputs/InputComponent.vue';

import { api } from '@/api/api';
import { isDark } from '@/helper/helper';

import type { IInventoryThread } from '@/models/Inventory.model';

const props = defineProps<{
    thread: IInventoryThread;
}>();

const emit = defineEmits(['update']);

const count = ref<number>(props.thread.count);

watchDebounced(count, async () => {
    await api.inventory.updateThread(props.thread.thread.reference, {
        count: count.value,
    });

    emit('update');
}, {
    debounce: 500,
});
</script>