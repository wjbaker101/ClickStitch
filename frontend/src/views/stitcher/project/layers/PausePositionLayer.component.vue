<template>
    <div v-if="pausePosition !== null" class="layer pause-position-layer-component">
        <div
            class="pause-position-indicator"
            :style="{
                'width': `${stitchSize}px`,
                'height': `${stitchSize}px`,
                'transform': `translate(${pausePosition.x * stitchSize}px, ${pausePosition.y * stitchSize}px)`,
            }"
        ></div>
    </div>
</template>

<script setup lang="ts">
import { useCurrentProject } from '../use/CurrentProject.use';

defineProps<{
    stitchSize: number;
}>();

const { pausePosition } = useCurrentProject();
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