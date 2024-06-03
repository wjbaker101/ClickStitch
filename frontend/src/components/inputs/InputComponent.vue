<template>
    <input v-model="value" class="rounded-md border-0 px-4 py-2 shadow-md outline-dashed outline-2 outline-transparent bg-background-light font-[inherit] focus:outline-primary focus:shadow-lg">
</template>

<script setup lang="ts">
const value = defineModel();
</script>