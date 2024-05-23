<template>
    <div
        class="list-item-component"
        :class="{
            'is-expanded': isExpanded,
            'is-hoverable': hover,
        }"
    >
        <div class="flex gap align-items-center">
            <div>
                <slot></slot>
            </div>
            <div v-if="slots.expanded" class="expander flex-auto" @click="onToggleOpen">
                <IconComponent icon="arrow-triangle-down" gap="right" />
                <span>More</span>
            </div>
        </div>
        <div class="more-content" v-if="slots.expanded">
            <div>
                <slot name="expanded"></slot>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, useSlots } from 'vue';

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
    padding: 1rem;
    background-color: var(--wjb-background-colour-light);
    border-radius: var(--wjb-border-radius);

    & + .list-item-component {
        margin-top: 0.5rem;
    }

    &.is-hoverable:hover {
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1), 0 4px 12px -6px rgba(0, 0, 0, 0.2);
    }

    .expander {
        user-select: none;
        cursor: pointer;

        span {
            vertical-align: middle;
        }
    }

    &.is-expanded {
        .more-content {
            grid-template-rows: 1fr;
            margin-top: 1rem;
        }
    }

    .more-content {
        display: grid;
        grid-template-rows: 0fr;
        margin-top: 0rem;

        & > div {
            overflow: hidden;
        }

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