<template>
    <select v-model="value">
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