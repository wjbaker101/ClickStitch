<template>
    <textarea
        v-model="value"
        class="w-full rounded-md px-4 py-2 shadow-md outline-dashed outline-2 outline-transparent transition-[outline-color] bg-background-light min-h-24 focus:outline-primary"
    >
    </textarea>
</template>

<script setup lang="ts">
const value = defineModel<string>();
</script>

<style lang="scss">
</style>