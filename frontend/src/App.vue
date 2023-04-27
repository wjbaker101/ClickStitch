<template>
    <RouterView></RouterView>
    <ModalComponent />
    <PopupComponent />
</template>

<script setup lang="ts">
</script>

<style lang="scss">
@use '@/style/variables' as *;
@use '@/style/main' as *;

#app {
    height: 100%;
}

body {
    $bg: #202628;

    font-family: 'Mulish', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    background-color: var(--wjb-background-colour);
    background: linear-gradient(
        -70deg,
        var(--wjb-background-colour) 0%,
        var(--wjb-background-colour) 50%,
        $bg 50%,
        $bg 100%,
    );
    color: var(--wjb-text-colour);

    &.no-scroll {
        overflow: hidden;
    }
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

table {
    width: 100%;
    border-collapse: collapse;
    border-radius: var(--wjb-border-radius);
    overflow: hidden;

    th {
        text-align: left;
        background-color: var(--wjb-background-colour-dark);

        @include shadow-small();

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

    @include shadow-small();

    &.hoverable {
        tbody {
            tr:hover {
                background-color: var(--wjb-background-colour-dark);
            }
        }
    }
}
</style>