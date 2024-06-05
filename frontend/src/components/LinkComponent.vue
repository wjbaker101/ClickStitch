<template>
    <a v-if="external" :href="href" target="_blank" class="decoration-0 text-primary-dark shadow-[0_1px_0] shadow-primary-dark hover:shadow-transparent">
        <ExternalLinkIcon class="mr-1 mb-1" />
        <slot></slot>
    </a>
    <RouterLink v-else :to="href" class="decoration-0 text-primary-dark shadow-[0_1px_0] shadow-primary-dark hover:shadow-transparent">
        <slot></slot>
    </RouterLink>
</template>

<script setup lang="ts">
import { ExternalLinkIcon } from 'lucide-vue-next';

defineProps<{
    href: string;
    external?: boolean;
}>();
</script>