<template>
    <ViewComponent>
        <template #nav>
            <strong>Tools</strong>
        </template>
        <CardComponent border="top" padded class="mb-4">
            <CanvasSizeCalculatorComponent />
        </CardComponent>
        <CardComponent border="top" padded>
            <RequiredSkeinsCalculatorComponent />
        </CardComponent>
    </ViewComponent>
</template>

<script setup lang="ts">
import CanvasSizeCalculatorComponent from '@/views/stitcher/tools/components/CanvasSizeCalculatorComponent.vue';
import RequiredSkeinsCalculatorComponent from '@/views/stitcher/tools/components/RequiredSkeinsCalculatorComponent.vue';
</script>