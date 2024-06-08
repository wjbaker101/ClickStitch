<template>
    <div
        class="rounded-md p-4 list-item-component bg-background-light"
        :class="{
            'is-expanded': isExpanded,
            'is-hoverable': hover,
        }"
    >
        <div class="flex items-center gap-4">
            <div class="grow">
                <slot></slot>
            </div>
            <div v-if="slots.expanded" @click="onToggleOpen" class="cursor-pointer select-none">
                <template v-if="!isExpanded">
                    <span class="align-middle">More</span>
                    <SquareChevronDownIcon class="ml-1 mt-0.5" />
                </template>
                <template v-else>
                    <span class="align-middle">Hide</span>
                    <SquareChevronUpIcon class="ml-1 mt-0.5" />
                </template>
            </div>
        </div>
        <div class="mt-0 grid more-content grid-rows-[0fr]" v-if="slots.expanded">
            <div class="overflow-hidden">
                <slot name="expanded"></slot>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, useSlots } from 'vue';

import { SquareChevronDownIcon, SquareChevronUpIcon } from 'lucide-vue-next';

const props = defineProps<{
    hover?: boolean;
    isInitiallyOpen?: boolean;
}>();

const slots = useSlots();

const isExpanded = ref<boolean>(props.isInitiallyOpen === true);

const onToggleOpen = function (): void {
    isExpanded.value = !isExpanded.value;
};
</script>

<style lang="scss">
.list-item-component {

    & + .list-item-component {
        margin-top: 0.5rem;
    }

    &.is-hoverable:hover {
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1), 0 4px 12px -6px rgba(0, 0, 0, 0.2);
    }

    &.is-expanded {
        .more-content {
            grid-template-rows: 1fr;
            margin-top: 1rem;
        }
    }

    .more-content {

        section {
            margin-left: 0.5rem;
            padding-left: 1rem;
            border-left: 1px solid #ccc;

            & + section {
                margin-top: 0.5rem;
            }
        }
    }
}
</style>