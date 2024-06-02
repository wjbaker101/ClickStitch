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

select {
    width: 150px;
    border: 0;
    padding: 0.5rem 1rem;
    line-height: 1em;
    font: inherit;
    letter-spacing: inherit;
    border-radius: var(--wjb-border-radius);
    outline-offset: -2px;
    outline: 2px dashed transparent;
    background-color: var(--wjb-background-colour-light);
    color: var(--wjb-text-colour);

    @apply shadow-md;

    &:focus {
        outline-color: var(--wjb-primary);
    }
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