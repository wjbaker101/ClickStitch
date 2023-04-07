<template>
    <div class="view-component">
        <NavComponent v-if="!hideNav">
            <slot name="nav"></slot>
        </NavComponent>
        <div class="page-content">
            <slot></slot>
        </div>
        <FooterComponent v-if="!hideFooter" />
    </div>
</template>

<script setup lang="ts">
import NavComponent from '@/components/Nav.component.vue';
import FooterComponent from '@/components/Footer.component.vue';

defineProps<{
    hideNav?: boolean;
    hideFooter?: boolean;
}>();
</script>

<style lang="scss">
.view-component {
    .page-content {
        min-height: calc(100vh - var(--footer-height));
    }
}
</style>