<template>
    <SelectComponent :options="aidaCounts" />
</template>

<script setup lang="ts">
import SelectComponent, { type ISelectOption } from '@/components/inputs/SelectComponent.vue';

const aidaCounts = Array.from(Array(35).keys())
    .map((_, index) => index + 6)
    .map<ISelectOption<number>>(x =>({
        value: x,
        description: String(x),
    }));
</script>

<style lang="scss">
</style>