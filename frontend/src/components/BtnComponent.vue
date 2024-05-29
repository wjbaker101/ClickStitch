<template>
    <button
        class="relative cursor-pointer rounded-md border-0 px-4 py-2 align-middle text-base tracking-normal shadow-inner font-[inherit] text-light outline-2 outline-dashed outline-offset-1 outline-transparent
        before:content-[''] before:absolute before:shadow-md before:inset-0 before:rounded-md
        hover:before:shadow-xl
        [&.primary]:hover:bg-primary-dark [&.primary]:bg-primary [&.primary]:active:outline-primary [&.primary]:focus:outline-primary
        [&.secondary]:bg-secondary [&.secondary]:hover:bg-secondary-dark [&.secondary]:active:outline-secondary [&.secondary]:focus:outline-secondary
"
        :class="{
            [type]: true,
        }"
    >
        <slot></slot>
    </button>
</template>

<script setup lang="ts">
interface IProps {
    type: 'primary' | 'secondary';
}

withDefaults(defineProps<IProps>(), {
    type: 'primary',
});
</script>

<style lang="scss">
</style>