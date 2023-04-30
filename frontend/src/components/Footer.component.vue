<template>
    <footer class="footer-component">
        <div>
            Copyright &copy; ClickStitch 2023<!-- &ndash;{{ new Date().getFullYear() }} -->,
            <br>
            All rights reserved. Created by <a href="https://wjbaker.com">William Baker</a>.
        </div>
        <div>
            Icons by <a href="https://akveo.github.io/eva-icons/#/">Eva icons</a>, fonts by <a href="https://fonts.google.com/">Google Fonts</a>.
        </div>
    </footer>
</template>

<script setup lang="ts">
</script>

<style lang="scss">
.footer-component {
    width: 100%;
    font-size: 0.8rem;
    padding: 0.5rem;
    color: var(--wjb-text-colour);
    z-index: -1;

    a {
        color: var(--wjb-primary);
        font-weight: bold;
        text-decoration: none;

        &:hover {
            text-decoration: underline;
        }
    }
}
</style>