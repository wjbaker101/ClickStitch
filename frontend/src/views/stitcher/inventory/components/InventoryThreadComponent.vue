<template>
    <CardComponent
        padded
        border="top"
        :style="{
            '--colour': thread.thread.colour,
        }"
        :class="{
            'is-dark': isDark(thread.thread.colour),
        }"
        class="grid items-center gap-4 grid-cols-[auto_1fr] border-[var(--colour)] group"
    >
        <div class="rounded-full bg-[var(--colour)] w-min p-4 md:p-6 shadow-md text-dark group-[.is-dark]:text-light">
            <SkeinIcon class="size-8 drop-shadow-icon md:size-12" />
        </div>
        <div>
            <InputComponent type="number" v-model="thread.count" class="w-full" @update:modelValue="onUpdate" />
        </div>
        <div class="col-span-2 text-center">
            <h2 class="font-bold text-md md:text-lg">{{ thread.thread.brand }} {{ thread.thread.code }}</h2>
        </div>
    </CardComponent>
</template>

<script setup lang="ts">
import SkeinIcon from '@/components/icons/SkeinIcon.vue';
import InputComponent from '@/components/inputs/InputComponent.vue';

import { api } from '@/api/api';
import { isDark } from '@/helper/helper';

import type { IInventoryThread } from '@/models/Inventory.model';

const props = defineProps<{
    thread: IInventoryThread;
}>();

const onUpdate = async function (): Promise<void> {
    const result = await api.inventory.updateThread(props.thread.thread.reference, {
        count: props.thread.count,
    });

    if (result instanceof Error)
        return;
};
</script>

<style lang="scss">
</style>