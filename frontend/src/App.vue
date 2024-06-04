<template>
    <RouterView></RouterView>
    <PopupComponent />
    <SideModalComponent />
    <ContextMenuComponent />
</template>

<script setup lang="ts">
import ContextMenuComponent from './components/context-menu/ContextMenuComponent.vue';
import PopupComponent from '@/components/popup/PopupComponent.vue';
import SideModalComponent from '@/components/modals/SideModalComponent.vue';
</script>

<style lang="scss">
@use '@/style/main' as *;

a {
    text-decoration: none;
}
</style>