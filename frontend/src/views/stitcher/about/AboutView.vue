<template>
    <ViewComponent class="about-view">
        <template #nav>
            <strong>About</strong>
        </template>
        <div class="content-width">
            <section class="grid items-center gap-4 md:grid-cols-[1fr_2fr]">
                <CardComponent border="top" padded>
                    <h3>What is ClickStitch?</h3>
                    <p>ClickStitch is <em>your</em> companion to track and manage your cross-stitching progress!</p>
                    <p>Whether you're a beginner or an advanced stitcher, ClickStitch makes life easier.</p>
                    <p>Start tracking your progress in 3 simple steps:</p>
                    <ol>
                        <li>Upload your pattern</li>
                        <li>Mark stitches as complete</li>
                        <li>Check your analytics!</li>
                    </ol>
                    <div class="text-center">
                        <RouterLink to="/dashboard">
                            <ButtonComponent>Start Stitching!</ButtonComponent>
                        </RouterLink>
                    </div>
                </CardComponent>
                <div class="text-center">
                    <img src="@/assets/clickstitch-on-devices.png" class="h-auto max-w-full">
                </div>
            </section>
            <NumberedSectionComponent class="my-24 text-center">
                <NumberedCardComponent>
                    <h2>Track Progress</h2>
                    <p><em>Highlight completed stitches</em> and view analytics of your progression.</p>
                </NumberedCardComponent>
                <NumberedCardComponent>
                    <h2>Manage Inventory</h2>
                    <p><em>Record your floss</em>, make sure you never run out before starting a new project.</p>
                </NumberedCardComponent>
                <NumberedCardComponent>
                    <h2>Multi-Device</h2>
                    <p>Patterns are stored in a secure database, so continue <em>stitching whereever you go</em>.</p>
                </NumberedCardComponent>
            </NumberedSectionComponent>
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
                    <div class="text-center">
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
import NumberedSectionComponent from '@/components/numbered-card/NumberedSectionComponent.vue';
import NumberedCardComponent from '@/components/numbered-card/NumberedCardComponent.vue';
</script>

<style lang="scss">
</style>