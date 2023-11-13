<template>
    <footer class="footer-component">
        <p>
            <div class="flex gap-small">
                <div class="flex-auto">Links:</div>
                <div class="flex-auto"><RouterLink class="link-component" to="/dashboard">Dashboard</RouterLink></div>
                <div class="flex-auto"><RouterLink class="link-component" to="/patterns">Patterns</RouterLink></div>
                <div class="flex-auto"><RouterLink class="link-component" to="/about">About</RouterLink></div>
                <div class="flex-auto"><RouterLink class="link-component" to="/settings">Settings</RouterLink></div>
            </div>
            <div class="flex gap-small">
                <div class="flex-auto">Follow on:</div>
                <div class="flex-auto"><LinkComponent :href="lemmyUrl" external>Lemmy</LinkComponent></div>
                <div class="flex-auto"><LinkComponent :href="instagramUrl" external>Instagram</LinkComponent></div>
                <div class="flex-auto"><LinkComponent :href="twitterUrl" external>Twitter</LinkComponent></div>
            </div>
        </p>
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
import { instagramUrl, twitterUrl, lemmyUrl } from '@/data/data';
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