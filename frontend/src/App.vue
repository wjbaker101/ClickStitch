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

.textured-background {
    background-color: var(--wjb-background-colour);
    background-attachment: fixed;
    background-image: var(--stitched-background-image);
}

input {
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

    &[type=text] {
        width: 100%;
    }

    &:focus {
        outline-color: var(--wjb-primary);
    }
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

kbd {
    --colour: color-mix(in srgb, var(--wjb-primary) 10%, transparent);

    padding: 0.125rem;
    background-color: var(--colour);
    border: 1px solid var(--colour);
    border-radius: var(--wjb-border-radius);
    line-height: 1.725em;
}
</style>