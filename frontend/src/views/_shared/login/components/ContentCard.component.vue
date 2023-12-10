<template>
    <CardComponent class="content-card-component" padded>
        <img class="logo" width="90" height="90" src="@/assets/logo-text.png">
        <slot></slot>
    </CardComponent>
</template>

<script setup lang="ts">
</script>

<style lang="scss">
.content-card-component {
    position: relative;
    background-color: var(--wjb-primary);
    background: linear-gradient(
        -25deg,
        var(--wjb-primary-dark),
        var(--wjb-primary),
    );
    color: var(--wjb-light);
    border-color: var(--wjb-tertiary);

    .logo {
        position: absolute;
        top: 0;
        left: 50%;
        translate: -50% -50%;
        padding: 0.5rem;
        background-color: #efefef;
        border-radius: 50%;
        border: 3px solid var(--wjb-primary);
        box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgb(0, 0, 0);
    }
}
</style>