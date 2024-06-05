<template>
    <ViewComponent>
        <template #nav>
            <strong>About</strong>
        </template>
        <div class="grid items-center gap-4 md:grid-cols-[1fr_2fr]">
            <CardComponent border="top" padded>
                <h3 class="mb-4 text-lg font-bold">What is ClickStitch?</h3>
                <p class="mb-2">ClickStitch is <em>your</em> companion to track and manage your cross-stitching progress!</p>
                <p class="mb-2">Whether you're a beginner or an advanced stitcher, ClickStitch makes life easier.</p>
                <p class="mb-2">Start tracking your progress in 3 simple steps:</p>
                <ol class="mb-4 list-inside list-decimal pl-4">
                    <li>Upload your pattern</li>
                    <li>Mark stitches as complete</li>
                    <li>Check your analytics!</li>
                </ol>
                <div class="text-center">
                    <RouterLink to="/dashboard">
                        <BtnComponent>Start Stitching!</BtnComponent>
                    </RouterLink>
                </div>
            </CardComponent>
            <div class="text-center">
                <img src="@/assets/clickstitch-on-devices.png" class="h-auto max-w-full">
            </div>
        </div>
        <NumberedSectionComponent class="my-24 text-center">
            <NumberedCardComponent>
                <h2 class="mt-2 mb-4 text-2xl font-bold">Track Progress</h2>
                <p><em>Highlight completed stitches</em> and view analytics of your progression.</p>
            </NumberedCardComponent>
            <NumberedCardComponent>
                <h2 class="mt-2 mb-4 text-2xl font-bold">Manage Inventory</h2>
                <p><em>Record your floss</em>, make sure you never run out before starting a new project.</p>
            </NumberedCardComponent>
            <NumberedCardComponent>
                <h2 class="mt-2 mb-4 text-2xl font-bold">Multi-Device</h2>
                <p>Patterns are stored in a secure database, so continue <em>stitching whereever you go</em>.</p>
            </NumberedCardComponent>
        </NumberedSectionComponent>
        <CardComponent border="top" padded class="mb-4">
            <h3 class="mb-4 text-lg font-bold">How do I track progress?</h3>
            <p>Upload the pattern via your <RouterLink to="/dashboard">Dashboard</RouterLink>.</p>

            <h4 class="my-4 font-bold text-md">📱 On Phones/Tablets:</h4>
            <ul class="list-inside list-disc pl-4">
                <li><KeyBinding>Drag</KeyBinding> to move around the pattern</li>
                <li><KeyBinding>Pinch</KeyBinding> to zoom in or out</li>
                <li><KeyBinding>Double tap</KeyBinding> to toggle (complete or uncomplete) an individual stitch</li>
                <li><KeyBinding>Long press</KeyBinding> on a stitch to show additional actions</li>
            </ul>

            <h4 class="my-4 font-bold text-md">💻 On Laptops/PCs:</h4>
            <ul class="list-inside list-disc pl-4">
                <li>Hold <KeyBinding>left mouse button</KeyBinding> and drag to move around the pattern</li>
                <li><KeyBinding>Scroll-wheel</KeyBinding> to zoom in or out</li>
                <li>Double click to toggle (complete or uncomplete) an individual stitch</li>
                <li>
                    Hold middle-click and drag to select multiple stitches...
                    <ul class="list-inside list-disc pl-4">
                        <li>Press <KeyBinding>space</KeyBinding> to <strong>complete</strong> all selected stitches</li>
                        <li>Hold <KeyBinding>shift</KeyBinding> and press <KeyBinding>space</KeyBinding> to <strong>uncomplete</strong> all selected stitches</li>
                    </ul>
                </li>
                <li><KeyBinding>Right-click</KeyBinding> on a stitch to show additional actions</li>
            </ul>
        </CardComponent>
        <div>
            <CardComponent border="top" padded>
                <h3 class="mb-4 text-lg font-bold">Lastly...</h3>
                <em>
                    <p>Images and renders of patterns may not 100% depict colours of threads used.</p>
                </em>
                <p>Any bugs or suggestions, please feel free to contact me! Links can be found in the footer ⭣.</p>
                <p>Thanks for your time, happy stitching! ❤️</p>
                <div class="text-center">
                    <RouterLink to="/dashboard">
                        <BtnComponent>Start Stitching!</BtnComponent>
                    </RouterLink>
                </div>
            </CardComponent>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import BtnComponent from '@/components/BtnComponent.vue';
import NumberedSectionComponent from '@/components/numbered-card/NumberedSectionComponent.vue';
import NumberedCardComponent from '@/components/numbered-card/NumberedCardComponent.vue';
import KeyBinding from '@/components/KeyBindingComponent.vue';
</script>