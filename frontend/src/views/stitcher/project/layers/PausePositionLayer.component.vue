<template>
    <div class="layer pause-position-layer-component">
        <div
            class="pause-position-indicator"
            :style="{
                'width': `${stitchSize}px`,
                'height': `${stitchSize}px`,
                'transform': `translate(${x * stitchSize}px, ${y * stitchSize}px)`,
            }"
        ></div>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    x: number;
    y: number;
    stitchSize: number;
}>();
</script>

<style lang="scss">
.pause-position-layer-component {

    .pause-position-indicator {
        border-radius: 50%;
        border: 3px solid #ffb400;
        background-color: transparent;
        box-shadow: 1px 2px 12px #000;
    }
}
</style>