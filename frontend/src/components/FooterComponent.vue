<template>
    <footer class="p-2 pt-4 text-xs/loose">
        <div class="mb-4">
            <div class="inline-block">Links:</div>
            <LinkComponent href="/dashboard" class="ml-2">Dashboard</LinkComponent>
            <LinkComponent href="/patterns" class="ml-2">Patterns</LinkComponent>
            <LinkComponent href="/inventory" class="ml-2">Inventory</LinkComponent>
            <LinkComponent href="/about" class="ml-2">About</LinkComponent>
            <LinkComponent href="/settings" class="ml-2">Settings</LinkComponent>
            <br>
            <div class="inline-block">Follow on:</div>
            <LinkComponent :href="lemmyUrl" external class="ml-2">Lemmy</LinkComponent>
            <LinkComponent :href="instagramUrl" external class="ml-2">Instagram</LinkComponent>
            <LinkComponent :href="twitterUrl" external class="ml-2">Twitter</LinkComponent>
        </div>
        <div>
            Copyright &copy; ClickStitch 2023 &ndash; {{ new Date().getFullYear() }},
            <br>
            All rights reserved.
            <br>
            Icons by <LinkComponent href="https://akveo.github.io/eva-icons/#/" external>Eva icons</LinkComponent>, fonts by <LinkComponent href="https://fonts.google.com/" external>Google Fonts</LinkComponent>.
        </div>
    </footer>
</template>

<script setup lang="ts">
const instagramUrl = 'https://www.instagram.com/ClickStitchStudio/';
const twitterUrl = 'https://twitter.com/ClickStitchApp';
const lemmyUrl = 'https://sh.itjust.works/u/ClickStitch?page=1&sort=New&view=Posts';
</script>