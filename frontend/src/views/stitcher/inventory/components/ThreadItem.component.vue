<template>
    <ListItemComponent class="thread-item-component">
        <div class="flex gap align-items-center">
            <div class="flex-auto">
                <img class="thread-image" :src="thread.image">
            </div>
            <div>
                <strong>{{ thread.code }}</strong>
            </div>
            <div class="flex-auto">
                <input class="counter" type="number" min="0" max="9999" v-model="count">
            </div>
        </div>
    </ListItemComponent>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import ListItemComponent from '@/components/ListItem.component.vue';

import { type IKnownThread } from '@/data/known-threads';

defineProps<{
    thread: IKnownThread;
}>();

const count = ref<number>(0);
</script>

<style lang="scss">
.thread-item-component {

    .counter {
        width: 7rem;
    }
}
</style>