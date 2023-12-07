<template>
    <div class="test-view">
        <div class="container flex align-items-center">
            <div class="flex-auto">
                <img width="250" height="250" src="@/assets/logo-large.png">
                <h1><code>v1.0 Launch</code></h1>
                <p>
                    <a href="#">https://clickstitch.app</a>
                </p>
            </div>
        </div>
        <div class="container flex align-items-center">
            <img width="50" height="50" class="logo" src="@/assets/logo-large.png">
            <div class="any-width">
                <img width="400" height="400" class="full-image" src="@/assets/clickstitch-phone-layered.png">
                <p style="padding: 0 3rem;">ClickStitch is your companion to track and manage your cross-stitching progress! Whether you're a beginner or an advanced stitcher, ClickStitch makes life easier.</p>
            </div>
        </div>
        <div class="container flex align-items-center">
            <img width="50" height="50" class="logo" src="@/assets/logo-large.png">
            <CardComponent class="flex-auto" border="top" padded>
                <CardComponent class="number flex align-items-center"></CardComponent>
                <h2>Track Progress</h2>
                <p><em>Complete stitches</em> as you go, highlighting what you have done and what you have left.</p>
            </CardComponent>
        </div>
        <div class="container flex align-items-center">
            <img width="50" height="50" class="logo" src="@/assets/logo-large.png">
            <CardComponent class="flex-auto" border="top" padded>
                <CardComponent class="number flex align-items-center"></CardComponent>
                <h2>View Analytics</h2>
                <p><em>View your progression</em> of past and present projects, with statistics like stitches per day.</p>
            </CardComponent>
        </div>
        <div class="container flex align-items-center">
            <img width="50" height="50" class="logo" src="@/assets/logo-large.png">
            <CardComponent class="flex-auto" border="top" padded>
                <CardComponent class="number flex align-items-center"></CardComponent>
                <h2>Manage Inventory</h2>
                <p><em>Record your floss</em>, make sure you never run out before starting a new project.</p>
            </CardComponent>
        </div>
        <div class="container flex align-items-center">
            <div class="any-width">
                <img width="250" height="250" src="@/assets/logo-large.png">
                <div>
                    <h1>Start your Journey:</h1>
                    <p>
                        <a href="#">https://clickstitch.app</a>
                    </p>
                    <p>
                        Please check description & leave a like ❤️
                    </p>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
</script>

<style lang="scss">
.test-view {
    counter-reset: index;

    .container {
        width: 1080px;
        position: relative;
        aspect-ratio: 1;
        border: 1px solid var(--wjb-primary);
        justify-content: center;

        &:not(:last-child ) {
            &::after {
                content: 'Swipe! 🞂';
                position: absolute;
                right: 1rem;
                top: 50%;
                translate: 0 -50%;
                font-weight: bold;
                font-size: 1.5rem;
            }
        }

        & > * {
            scale: 2;
            max-width: 250px;
            text-align: center;

            &.any-width {
                max-width: 50%;
            }
        }

        .logo {
            position: absolute;
            top: 2rem;
            left: 2rem;
        }

        .number {
            width: 4rem;
            position: absolute;
            padding: 1rem;
            top: 0;
            left: 50%;
            translate: -50% -50%;
            aspect-ratio: 1;
            border-radius: 50%;
            justify-content: center;
            font-weight: bold;
            font-size: 2rem;
            color: var(--wjb-secondary);
            border: 2px solid var(--wjb-primary);

            &::before {
                counter-increment: index;
                content: counter(index);
            }
        }

        .full-image {
            height: auto;
        }

        a {
            color: var(--wjb-primary);
        }
    }
}
</style>