<template>
    <NavComponent v-if="!hideNav">
        <slot name="nav"></slot>
    </NavComponent>
    <div class="h-full grid grid-cols-[1fr_min(1200px,calc(100%-2rem))_1fr] grid-rows-[1fr_auto]">
        <div class="col-start-2 pt-[calc(1.25rem+58px)] md:pt-[calc(1.5rem+74px)]">
            <slot></slot>
        </div>
        <FooterComponent v-if="!hideFooter" class="col-span-3 row-start-2" />
    </div>
</template>

<script setup lang="ts">
import NavComponent from '@/components/nav/NavComponent.vue';
import FooterComponent from '@/components/FooterComponent.vue';

defineProps<{
    hideNav?: boolean;
    hideFooter?: boolean;
}>();
</script>