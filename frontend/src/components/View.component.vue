<template>
    <div class="view-component">
        <NavComponent v-if="!hideNav">
            <slot name="nav"></slot>
        </NavComponent>
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