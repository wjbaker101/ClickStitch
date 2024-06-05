<template>
    <nav
        class="fixed top-1 right-1 left-1 grid items-center gap-4 rounded-md border-solid px-4 py-2 shadow-lg
            backdrop-blur nav-component grid-cols-[1fr_auto_1fr] text-light z-[1] text-shadow border-1
            border-primary-dark bg-gradient-to-tl from-primary-dark/90 to-primary/90
            md:top-2 md:right-2 md:left-2 md:py-4"
    >
        <div class="flex items-center gap-2 justify-self-start">
            <div class="inline-grid flex-auto place-content-center rounded-full shadow-md bg-background-light size-10">
                <img src="@/assets/logo.png">
            </div>
            <strong class="hidden md:block">ClickStitch</strong>
        </div>
        <div class="justify-self-center text-center">
            <slot></slot>
        </div>
        <div class="justify-self-end md:hidden">
            <BtnComponent class="-m-2" @click="onToggleMenu">
                <MenuIcon />
            </BtnComponent>
        </div>
        <div
            :class="{
                'is-menu-open': isMenuOpen,
            }"
            class="absolute -bottom-1 flex flex-auto translate-y-full border-1 border-solid border-primary-dark flex-col
                justify-self-end rounded-md p-2 shadow-lg backdrop-blur z-[1] links opacity-0 pointer-events-none
                bg-image-inherit
                md:static md:translate-y-0 md:flex-row md:gap-4 md:p-0 md:shadow-none md:bg-none md:border-transparent md:pointer-events-auto md:opacity-100
                [&.is-menu-open]:opacity-100 [&.is-menu-open]:pointer-events-auto"
        >
            <RouterLink
                v-for="link in links.filter(x => x.isVisible)"
                :to="link.path"
                class="flex-auto rounded-md px-6 py-3 font-bold text-inherit decoration-0 outline-dashed outline-2
                    outline-transparent hover:bg-primary-dark focus:outline-secondary md:p-0"
            >
                <IconComponent :icon="link.iconName" gap="right" class="drop-shadow-icon" />
                <span class="align-middle">{{ link.title }}</span>
            </RouterLink>
        </div>
    </nav>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import { MenuIcon } from 'lucide-vue-next';
import BtnComponent from '@/components/BtnComponent.vue';

import { linkFactory } from './link-factory';
import { subdomain } from '@/setup/router/router-helper';

const isMenuOpen = ref<boolean>(false);

const onToggleMenu = function (): void {
    isMenuOpen.value = !isMenuOpen.value;
};

const links = linkFactory.get(subdomain);
</script>