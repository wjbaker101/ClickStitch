<template>
    <select v-model="value" class="rounded-md border-0 px-4 py-2 shadow-md outline-dashed outline-2 outline-transparent bg-background-light font-[inherit] w-[150px] focus:outline-primary focus:shadow-lg">
        <option :value="null" disabled>Select option...</option>
        <option v-for="option in options" :value="option.value">{{ option.description }}</option>
    </select>
</template>

<script setup lang="ts" generic="TValue">
defineProps<{
    options: Array<ISelectOption<TValue>>;
}>();

const value = defineModel<TValue | null>();
</script>

<script lang="ts">
export interface ISelectOption<TValue> {
    readonly value: TValue;
    readonly description: string;
}
</script>

<style lang="scss">
</style>