<template>
    <div class="zero-state-component text-centered">
        <IconComponent :icon="icon" size="huge" />
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
import IconComponent, { IconName } from '@wjb/vue/component/IconComponent.vue';

defineProps<{
    icon: IconName;
}>();
</script>

<style lang="scss">
.zero-state-component {}
</style>