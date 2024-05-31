<template>
    <input
        type="checkbox"
        v-model="isChecked"
        @change="emit('change', { isChecked })"
        class="rounded-md border-0 align-middle text-base tracking-normal outline-dashed outline-2 outline-offset-1 outline-transparent font-[inherit] size-6 accent-primary focus:outline-primary-dark"
    >
    <span v-if="label" class="pl-1 align-middle">{{ label }}</span>
</template>

<script setup lang="ts">
defineProps<{
    label?: string;
}>();

const isChecked = defineModel<boolean>();

const emit = defineEmits(['change']);
</script>

<style lang="scss">
</style>