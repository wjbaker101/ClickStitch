<template>
    <div class="view-component">
        <NavComponent v-if="!hideNav" />
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
import NavComponent from '@/components/Nav.component.vue';

defineProps<{
    hideNav?: boolean;
}>();
</script>

<style lang="scss">
</style>