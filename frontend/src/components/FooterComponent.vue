<template>
    <footer class="p-2 footer-component text-xs/loose">
        <section>
            <div class="flex gap-small">
                <div class="flex-auto">Links:</div>
                <div class="flex-auto"><RouterLink class="link-component" to="/dashboard">Dashboard</RouterLink></div>
                <div class="flex-auto"><RouterLink class="link-component" to="/patterns">Patterns</RouterLink></div>
                <div class="flex-auto"><RouterLink class="link-component" to="/inventory">Inventory</RouterLink></div>
                <div class="flex-auto"><RouterLink class="link-component" to="/about">About</RouterLink></div>
                <div class="flex-auto"><RouterLink class="link-component" to="/settings">Settings</RouterLink></div>
            </div>
            <div class="flex gap-small">
                <div class="flex-auto">Follow on:</div>
                <div class="flex-auto"><LinkComponent :href="lemmyUrl" external>Lemmy</LinkComponent></div>
                <div class="flex-auto"><LinkComponent :href="instagramUrl" external>Instagram</LinkComponent></div>
                <div class="flex-auto"><LinkComponent :href="twitterUrl" external>Twitter</LinkComponent></div>
            </div>
        </section>
        <section>
            Copyright &copy; ClickStitch 2023 &ndash; {{ new Date().getFullYear() }},
            <br>
            All rights reserved.
        </section>
        <div>
            Icons by <LinkComponent href="https://akveo.github.io/eva-icons/#/" external>Eva icons</LinkComponent>, fonts by <LinkComponent href="https://fonts.google.com/" external>Google Fonts</LinkComponent>.
        </div>
    </footer>
</template>

<script setup lang="ts">
const instagramUrl = 'https://www.instagram.com/ClickStitchStudio/';
const twitterUrl = 'https://twitter.com/ClickStitchApp';
const lemmyUrl = 'https://sh.itjust.works/u/ClickStitch?page=1&sort=New&view=Posts';
</script>

<style lang="scss">
.footer-component {
    color: var(--wjb-text-colour);
}
</style>