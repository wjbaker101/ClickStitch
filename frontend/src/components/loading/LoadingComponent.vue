<template>
    <div class="my-4 text-center">
        <svg
            width="70"
            height="70"
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 100 100"
            preserveAspectRatio="xMidYMid"
            class="max-w-full align-middle text-primary-dark animate-loading mx-auto"
        >
            <circle cx="50" cy="50" fill="none" stroke="currentColor" stroke-width="5" r="47" stroke-dasharray="115 80" stroke-linecap="round">
                <animateTransform attributeName="transform" type="rotate" repeatCount="indefinite" dur="1s" values="0 50 50;360 50 50" keyTimes="0;1" />
            </circle>
        </svg>
        <p v-if="itemName" class="mt-4">Loading {{ itemName }}...</p>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    itemName?: string;
}>();
</script>