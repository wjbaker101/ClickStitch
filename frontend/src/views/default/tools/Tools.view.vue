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
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import CanvasSizeCalculatorComponent from '@/views/default/tools/components/CanvasSizeCalculator.component.vue';
</script>

<style lang="scss">
.tools-view {}
</style>