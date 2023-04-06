<template>
    <div class="view-component">
        <NavComponent />
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
import NavComponent from '@/components/Nav.component.vue';
</script>

<style lang="scss">
</style>