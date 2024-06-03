<template>
    <div class="test-view">
        <div class="container flex border-solid align-items-center border-1 border-primary">
            <div class="any-width">
                <img width="400" height="400" class="full-image" src="@/assets/clickstitch-phone-layered.png">
                <div style="padding: 2rem 6rem 0 6rem;" class="flex text-left gap align-items-center">
                    <div class="flex-auto">
                        <img width="100" height="100" src="@/assets/logo-large.png">
                    </div>
                    <div>
                        <h1 style="margin: 0; margin-bottom: 1rem;"><code>v1.0 Launch</code></h1>
                        <p style="margin: 0;">
                            <a href="#" class="text-primary">https://clickstitch.app</a>
                        </p>
                    </div>
                </div>
            </div>
        </div>
        <div class="container flex border-solid align-items-center border-1 border-primary">
            <img width="50" height="50" class="logo" src="@/assets/logo-large.png">
            <div class="any-width">
                <img width="500" height="500" class="full-image" src="@/assets/clickstitch-phone-layered.png" style="opacity: 0.3; position: absolute; top: 50%; left: 50%; translate: -50% -50%;">
                <p class="rounded-md border-solid background text-light border-1 border-primary-dark bg-primary/60" style="margin: 0 -5rem;">ClickStitch is <em>your</em> companion to track and manage your cross-stitching progress! Whether you're a beginner or an advanced stitcher, ClickStitch makes life easier.</p>
            </div>
        </div>
        <div class="container flex border-solid align-items-center border-1 border-primary">
            <img width="50" height="50" class="logo" src="@/assets/logo-large.png">
            <CardComponent class="flex-auto" border="top" padded>
                <CardComponent class="flex border-2 border-solid number align-items-center border-primary"></CardComponent>
                <h2>Track Progress</h2>
                <p><em>Highlight completed stitches</em> and view analytics of your progression!</p>
            </CardComponent>
        </div>
        <div class="container flex border-solid align-items-center border-1 border-primary">
            <img width="50" height="50" class="logo" src="@/assets/logo-large.png">
            <CardComponent class="flex-auto" border="top" padded>
                <CardComponent class="flex border-2 border-solid number align-items-center border-primary"></CardComponent>
                <h2>Manage Inventory</h2>
                <p><em>Record your floss</em>, make sure you never run out before starting a new project!</p>
            </CardComponent>
        </div>
        <div class="container flex border-solid align-items-center border-1 border-primary">
            <img width="50" height="50" class="logo" src="@/assets/logo-large.png">
            <CardComponent class="flex-auto" border="top" padded>
                <CardComponent class="flex border-2 border-solid number align-items-center border-primary"></CardComponent>
                <h2>Multi-Device</h2>
                <p>Patterns are securely stored online, so continue <em>stitching everywhere you go!</em></p>
            </CardComponent>
        </div>
        <div class="container flex border-solid align-items-center border-1 border-primary">
            <div class="any-width">
                <img width="250" height="250" src="@/assets/logo-large.png">
                <div>
                    <h1>Start your Journey:</h1>
                    <p>
                        <a href="#" class="text-primary">https://clickstitch.app</a>
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
            top: 3rem;
            left: 3rem;
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

            &::before {
                counter-increment: index;
                content: counter(index);
            }
        }

        .full-image {
            height: auto;
        }

        p.background {
            padding: 2rem 1rem;
            font-weight: bold;
            box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);
            backdrop-filter: blur(5px);
            text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.5);
            scale: 0.5;
            font-size: 2rem;
            line-height: 3.2rem;
        }
    }
}
</style>