<template>
    <RouterView></RouterView>
    <ModalComponent />
    <PopupComponent />
    <ContextMenuComponent />
</template>

<script setup lang="ts">
import ContextMenuComponent from './components/context-menu/ContextMenuComponent.vue';
</script>

<style lang="scss">
@use '@/style/main' as *;

#app {
    height: 100%;
}

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

table {
    width: 100%;
    border-collapse: collapse;
    border-radius: var(--wjb-border-radius);
    overflow: hidden;

    th {
        text-align: left;
        background-color: var(--wjb-background-colour-dark);

        @apply shadow-md;

        &:first-child {
            border-bottom-left-radius: var(--wjb-border-radius);
        }

        &:last-child {
            border-bottom-right-radius: var(--wjb-border-radius);
        }
    }

    th,
    td {
        padding: 0.25rem 0.5rem;
    }

    @apply shadow-md;

    &.hoverable {
        tbody {
            tr:hover {
                background-color: var(--wjb-background-colour-dark);
            }
        }
    }
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