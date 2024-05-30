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
        <svg
            width="15"
            height="15"
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 100 100"
            preserveAspectRatio="xMidYMid"
            class="align-middle inline-block mr-2"
            v-if="loading"
        >
            <circle cx="50" cy="50" fill="none" stroke="currentColor" stroke-width="10" r="47" stroke-dasharray="115 80" stroke-linecap="round">
                <animateTransform attributeName="transform" type="rotate" repeatCount="indefinite" dur="1s" values="0 50 50;360 50 50" keyTimes="0;1" />
            </circle>
        </svg>
        <slot></slot>
    </button>
</template>

<script setup lang="ts">
interface IProps {
    type: 'primary' | 'secondary';
    loading?: boolean;
}

withDefaults(defineProps<IProps>(), {
    type: 'primary',
});
</script>

<style lang="scss">
</style>