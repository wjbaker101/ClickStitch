<template>
    <ListItemComponent class="thread-item-component">
        <div class="flex gap align-items-center">
            <div class="flex-auto">
                <img class="thread-image" :src="thread.image">
            </div>
            <div>
                <strong>{{ thread.code }}</strong>
            </div>
            <div class="flex-auto"></div>
        </div>
    </ListItemComponent>
</template>

<script setup lang="ts">
import ListItemComponent from '@/components/ListItem.component.vue';

import { type IKnownThread } from '@/data/known-threads';

defineProps<{
    thread: IKnownThread;
}>();
</script>

<style lang="scss">
</style>