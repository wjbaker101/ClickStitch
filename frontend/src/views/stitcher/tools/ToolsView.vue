<template>
    <ViewComponent class="tools-view">
        <template #nav>
            <strong>Tools</strong>
        </template>
        <div class="content-width">
            <CardComponent border="top" padded class="mb-4">
                <CanvasSizeCalculatorComponent />
            </CardComponent>
            <CardComponent border="top" padded>
                <RequiredSkeinsCalculatorComponent />
            </CardComponent>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import CanvasSizeCalculatorComponent from '@/views/stitcher/tools/components/CanvasSizeCalculatorComponent.vue';
import RequiredSkeinsCalculatorComponent from '@/views/stitcher/tools/components/RequiredSkeinsCalculatorComponent.vue';
</script>