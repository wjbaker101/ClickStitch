<template>
    <ViewComponent class="about-view">
        <template #nav>
            <strong>About</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <h3>What is ClickStitch?</h3>
                    <p>ClickStitch is your companion to track your cross-stitching progress and find fun digital patterns!</p>
                    <p>From beginner to advanced options, you don't have to miss out.</p>
                    <p>Start tracking your progress in 3 simple steps:</p>
                    <ol>
                        <li>Purchase your favourite patterns from the <LinkComponent :href="etsyStoreUrl" external>Etsy Store</LinkComponent>. You'll recieve digital cross-stitch kit information to start.</li>
                        <li>Once you have the materials required, create an account (if you haven't already) on <LinkComponent :href="websiteUrl">clickstitch.app</LinkComponent>.</li>
                        <li>Add the pattern to your account and start "completing" stitches as-you-go!</li>
                    </ol>
                </CardComponent>
            </section>
            <section>
                <CardComponent border="top" padded>
                    <h3>How do I track progress?</h3>
                    <p>Make sure you add the pattern onto your account.</p>
                    <p>Visit the project editor page (via the dashboard), double click/ tap on individual squares to mark them as complete.</p>
                    <p>If you wish to complete multiple stitches in one go, hold and drag the middle mouse button then press the space bar.</p>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { etsyStoreUrl, websiteUrl } from '@/data/data';
</script>

<style lang="scss">
</style>