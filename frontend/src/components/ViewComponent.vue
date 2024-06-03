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
import NavComponent from '@/components/nav/NavComponent.vue';
import FooterComponent from '@/components/FooterComponent.vue';

defineProps<{
    hideNav?: boolean;
    hideFooter?: boolean;
}>();
</script>

<style lang="scss">
.view-component {
    .page-content {
        padding-top: calc(1.5rem + 82px);
        min-height: calc(100vh - 162px);

        @media screen and (max-width: 720px) {
            padding-top: calc(1.5rem + 59px);
        }
    }
}
</style>