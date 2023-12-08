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
                    <p>Upload the pattern via your <RouterLink to="/dashboard">Dashboard</RouterLink>.</p>

                    <h4>📱 On Phones/Tablets:</h4>
                    <ul>
                        <li><kbd>Drag</kbd> to move around the pattern</li>
                        <li><kbd>Pinch</kbd> to zoom in or out</li>
                        <li><kbd>Double tap</kbd> to toggle (complete or uncomplete) an individual stitch</li>
                        <li><kbd>Long press</kbd> on a stitch to show additional actions</li>
                    </ul>

                    <h4>💻 On Laptops/PCs:</h4>
                    <ul>
                        <li>Hold <kbd>left mouse button</kbd> and drag to move around the pattern</li>
                        <li><kbd>Scroll-wheel</kbd> to zoom in or out</li>
                        <li>Double click to toggle (complete or uncomplete) an individual stitch</li>
                        <li>
                            Hold middle-click and drag to select multiple stitches...
                            <ul>
                                <li>Press <kbd>space</kbd> to <strong>complete</strong> all selected stitches</li>
                                <li>Hold <kbd>shift</kbd> and press <kbd>space</kbd> to <strong>uncomplete</strong> all selected stitches</li>
                            </ul>
                        </li>
                        <li><kbd>Right-click</kbd> on a stitch to show additional actions</li>
                    </ul>
                </CardComponent>
            </section>
            <section>
                <CardComponent border="top" padded>
                    <h3>Lastly...</h3>
                    <em>
                        <p>Images and renders of patterns may not 100% depict colours of threads used.</p>
                    </em>
                    <p>Any bugs or suggestions, please feel free to contact me! Links can be found in the footer ⭣.</p>
                    <p>Thanks for your time, happy stitching! ❤️</p>
                    <div class="text-centered">
                        <RouterLink to="/dashboard">
                            <ButtonComponent>Start Stitching!</ButtonComponent>
                        </RouterLink>
                    </div>
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