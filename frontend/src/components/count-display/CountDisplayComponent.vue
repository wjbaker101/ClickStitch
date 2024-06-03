<template>
    <div class="mr-1 inline-block rounded-full p-2 text-center font-bold shadow-md size-10 bg-secondary text-light">{{ count ?? '-' }}</div>{{ description }}
</template>

<script setup lang="ts">
defineProps<{
    count: number | null | undefined;
    description: string;
}>();
</script>