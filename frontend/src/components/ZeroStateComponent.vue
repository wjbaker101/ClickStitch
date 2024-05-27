<template>
    <div class="text-center">
        <IconComponent :icon="icon" size="huge" />
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
import IconComponent, { type IconName } from '@wjb/vue/component/IconComponent.vue';

defineProps<{
    icon: IconName;
}>();
</script>

<style lang="scss">
</style>