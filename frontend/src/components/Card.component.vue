<template>
    <div
        class="card-component"
        :class="{
            [`border-${border}`]: true,
            'is-hoverable': hoverable,
            'is-padded': padded,
        }"
    >
        <header v-if="header">
            <slot name="header"></slot>
        </header>
        <slot></slot>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    border?: 'top' | 'right' | 'bottom' | 'left';
    hoverable?: boolean;
    padded?: boolean;
    header?: boolean;
}>();
</script>

<style lang="scss">
@use '@/style/variables' as *;

.card-component {
    background-color: var(--wjb-background-colour);
    border-width: 0;
    border-style: solid;
    border-color: var(--wjb-primary);
    outline: 2px dashed transparent;
    outline-offset: 1px;
    border-radius: var(--wjb-border-radius);
    box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);

    &.is-hoverable {
        cursor: pointer;

        &:hover {
            outline: 2px dashed var(--wjb-tertiary);
        }
    }

    &.is-padded {
        padding: 2rem 1rem;

        & > *:first-child {
            margin-top: 0;
        }

        & > *:last-child {
            margin-bottom: 0;
        }
    }

    &.border-top {
        border-top-width: 2px;
        border-bottom-left-radius: var(--wjb-border-radius);
        border-bottom-right-radius: var(--wjb-border-radius);
    }

    &.border-right {
        border-right-width: 2px;
        border-top-left-radius: var(--wjb-border-radius);
        border-bottom-left-radius: var(--wjb-border-radius);
    }

    &.border-bottom {
        border-bottom-width: 2px;
        border-top-left-radius: var(--wjb-border-radius);
        border-top-right-radius: var(--wjb-border-radius);
    }

    &.border-left {
        border-left-width: 2px;
        border-top-right-radius: var(--wjb-border-radius);
        border-bottom-right-radius: var(--wjb-border-radius);
    }

    & > header {
        margin: -1rem;
        margin-bottom: 1rem;
        background-color: var(--wjb-primary);
        background: linear-gradient(
            -5deg,
            transparentize($primary-dark, 0.1),
            transparentize($primary, 0.1),
        );
        backdrop-filter: blur(2px);
        color: var(--wjb-light);
        text-shadow: 1px 1px rgba(0, 0, 0, 0.6);
        box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);
        border: 1px solid var(--wjb-primary-dark);
        border-radius: var(--wjb-border-radius);

        h3 {
            line-height: 1em;
            margin: 1rem;
        }
    }
}
</style>