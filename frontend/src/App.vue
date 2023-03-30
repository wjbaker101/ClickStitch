<template>
    <NavComponent />
    <RouterView></RouterView>
</template>

<script setup lang="ts">
import NavComponent from '@/components/Nav.component.vue';

import { useInput } from '@/use/input/input.use';

const input = useInput();

document.addEventListener('keydown', (event: KeyboardEvent) => {
    input.keysDown.add(event.key);
});

document.addEventListener('keyup', (event: KeyboardEvent) => {
    input.keysDown.delete(event.key);
});
</script>

<style lang="scss">
@use '@/style/variables' as *;
@use '@/style/main' as *;

#app {
    height: 100%;
}

body {
    $bg: #202628;

    background-color: var(--wjb-background-colour);
    background: linear-gradient(
        -70deg,
        var(--wjb-background-colour) 0%,
        var(--wjb-background-colour) 50%,
        $bg 50%,
        $bg 100%,
    );
    color: var(--wjb-text-colour);
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

    @include shadow-small();

    &:focus {
        outline-color: var(--wjb-primary);
    }
}

.content-width {
    position: relative;
    max-width: 1200px;
    margin: 0 auto;
    padding: 0 1rem;

    & > h1 {
        margin-top: 0;
        padding-top: 6rem;
    }
}
</style>