<template>
    <div class="action-stitch-component">
        <code v-if="activeStitch !== null">[{{ activeStitch.x }},{{ activeStitch.y }}<template v-if="activeStitch.endX && activeStitch.endY">,{{ activeStitch.endX }},{{ activeStitch.endY }}</template>]</code>
        -
        <small>{{ thread?.description }}</small>
    </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

import { useCurrentProject } from '@/views/stitcher/project/use/CurrentProject.use';

import type { IPatternThread } from '@/models/Pattern.model';

const { activeStitch, palette } = useCurrentProject();

const thread = computed<IPatternThread | null>(() => {
    if (activeStitch.value === null)
        return null;

    return palette.value.get(activeStitch.value.threadIndex) as IPatternThread;
});
</script>