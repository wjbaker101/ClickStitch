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
            <section class="flex gap">
                <CardComponent border="top" padded>
                    <h3>How do I track progress?</h3>
                    <p>Make sure you add the pattern onto your account.</p>
                    <p>Visit the project editor page (via the dashboard), double click/ tap on individual squares to mark them as complete.</p>
                    <p>If you wish to complete multiple stitches in one go, hold and drag the middle mouse button then press the space bar.</p>
                </CardComponent>
                <video class="video flex-auto" src="@/assets/how-to-track-your-progress.mp4" width="600" autoplay loop></video>
            </section>
            <section>
                <CardComponent border="top" padded>
                    <h3>Please Note:</h3>
                    <em>
                        <p>Pictures of patterns on this website or the Etsy store may not be exact representations of the finished product. For example, due to different aida counts/ lighting/ hoop sizes.</p>
                        <p>However, a lot of care has been taken to make the digital patterns as realistic as possible, so I hope they turn out fantastic!</p>
                    </em>
                </CardComponent>
            </section>
            <section>
                <CardComponent border="top" padded>
                    <h3>Lastly:</h3>
                    <p>Any bugs or suggestions, please feel free to contact me!</p>
                    <p>Thanks for your time, happy stitching!</p>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { etsyStoreUrl, websiteUrl } from '@/data/data';
</script>

<style lang="scss">
@use '@/style/variables' as *;

.about-view {
    .video {
        border-radius: var(--wjb-border-radius);
        vertical-align: middle;

        @include shadow-medium();
    }
}
</style>