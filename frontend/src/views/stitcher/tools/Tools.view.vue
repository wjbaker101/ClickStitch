<template>
    <ViewComponent class="tools-view">
        <template #nav>
            <strong>Tools</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <CanvasSizeCalculatorComponent />
                </CardComponent>
            </section>
            <section>
                <CardComponent border="top" padded>
                    <RequiredSkeinsCalculatorComponent />
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import CanvasSizeCalculatorComponent from '@/views/stitcher/tools/components/CanvasSizeCalculator.component.vue';
import RequiredSkeinsCalculatorComponent from '@/views/stitcher/tools/components/RequiredSkeinsCalculator.component.vue';
</script>

<style lang="scss">
.tools-view {}
</style>