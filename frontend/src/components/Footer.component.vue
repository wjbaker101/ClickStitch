<template>
    <footer class="footer-component">
        <div>
            Copyright &copy; ClickStitch 2023<!-- &ndash;{{ new Date().getFullYear() }} -->,
            <br>
            All rights reserved.
        </div>
        <div>
            Icons by <LinkComponent href="https://akveo.github.io/eva-icons/#/" external>Eva icons</LinkComponent>, fonts by <LinkComponent href="https://fonts.google.com/" external>Google Fonts</LinkComponent>.
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
}
</style>