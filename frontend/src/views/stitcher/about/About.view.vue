<template>
    <ViewComponent class="about-view">
        <template #nav>
            <strong>About</strong>
        </template>
        <div class="content-width">
            <section class="collapse flex gap align-items-center">
                <CardComponent border="top" padded>
                    <h3>What is ClickStitch?</h3>
                    <p>ClickStitch is <em>your</em> companion to track and manage your cross-stitching progress!</p>
                    <p>Whether you're a beginner or an advanced stitcher, ClickStitch makes life easier.</p>
                    <p>Start tracking your progress in 3 simple steps:</p>
                    <ol>
                        <li>Upload your pattern</li>
                        <li>"Complete" stitches as you're stitch</li>
                        <li>Once finished, check your analytics!</li>
                    </ol>
                    <div class="text-centered">
                        <RouterLink to="/dashboard">
                            <ButtonComponent>Start Stitching!</ButtonComponent>
                        </RouterLink>
                    </div>
                </CardComponent>
                <div class="flex-2">
                    <img src="@/assets/clickstitch-on-devices.png">
                </div>
            </section>
            <section class="numbered text-centered">
                <CardComponent border="top" padded>
                    <CardComponent class="number flex align-items-center"></CardComponent>
                    <h2>Track Progress</h2>
                    <p><em>Highlight completed stitches</em> and view analytics of your progression.</p>
                </CardComponent>
                <CardComponent border="top" padded>
                    <CardComponent class="number flex align-items-center"></CardComponent>
                    <h2>Manage Inventory</h2>
                    <p><em>Record your floss</em>, make sure you never run out before starting a new project.</p>
                </CardComponent>
                <CardComponent border="top" padded>
                    <CardComponent class="number flex align-items-center"></CardComponent>
                    <h2>Multi-Device</h2>
                    <p>Patterns are stored in a secure database, so continue <em>stitching whereever you go</em>.</p>
                </CardComponent>
            </section>
            <section class="flex gap">
                <CardComponent border="top" padded>
                    <h3>How do I track progress?</h3>
                    <p>Make sure you add the pattern onto your account.</p>
                    <p>Visit the project editor page (via the dashboard), double click/ tap on individual squares to mark them as complete.</p>
                    <p>If you wish to complete multiple stitches in one go, hold and drag the middle mouse button then press the space bar.</p>
                </CardComponent>
            </section>
            <section>
                <CardComponent border="top" padded>
                    <h3>Please Note:</h3>
                    <em>
                        <p>Pictures of patterns on this website or the Etsy store may not be exact representations of the finished product. For example, due to different aida counts/ lighting/ hoop sizes.</p>
                        <p>However, a lot of care has been taken to make the digital patterns as realistic as possible, so I hope they turn out fantastic!</p>
                    </em>
                    <h3>Lastly...</h3>
                    <p>Any bugs or suggestions, please feel free to contact me!</p>
                    <p>Thanks for your time, happy stitching!</p>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
</script>

<style lang="scss">
@use '@/style/variables' as *;

.about-view {

    img {
        max-width: 100%;
        height: auto;
    }

    .video {
        max-width: 100%;
        border-radius: var(--wjb-border-radius);
        vertical-align: middle;
    }

    .collapse {
        @media screen and (max-width: 1024px) {
            display: block;

            & > :first-child {
                margin-bottom: 1rem;
            }
        }
    }

    .numbered {
        margin: 6rem 0;
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
        justify-items: stretch;
        gap: 3rem;
        counter-reset: index;

        & > * {
            position: relative;
        }
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
}
</style>