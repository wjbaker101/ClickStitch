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

body {
    font-family: 'Mulish', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    color: var(--wjb-text-colour);
}

section {
    & + section {
        margin-top: 1rem;
    }
}

.content-width {
    position: relative;
    max-width: 1200px;
    margin: 0 auto;
    padding: 0 1rem;
}

a {
    text-decoration: none;
}
</style>