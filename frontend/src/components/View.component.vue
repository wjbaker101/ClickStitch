<template>
    <div class="view-component">
        <NavComponent v-if="!hideNav">
            <slot name="nav"></slot>
        </NavComponent>
        <slot></slot>
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
    min-height: calc(100vh - var(--footer-height));

    .footer-component {
        position: absolute;
        bottom: 0;
    }
}
</style>