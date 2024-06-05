<template>
    <input v-model="value" class="rounded-md px-4 py-2 shadow-md outline-dashed outline-2 outline-offset-0 outline-transparent bg-background-light text-colour focus:outline-primary focus:shadow-lg">
</template>

<script setup lang="ts">
const value = defineModel();
</script>