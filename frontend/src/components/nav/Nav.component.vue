<template>
    <nav class="nav-component">
        <div class="flex gap align-items-center">
            <div class="logo-container flex-auto">
                <img src="@/assets/logo.png">
            </div>
            <strong>ClickStitch</strong>
        </div>
        <div class="custom align-items-center">
            <slot></slot>
        </div>
        <div class="menu text-right">
            <ButtonComponent class="mini" @click="onToggleMenu">
                <IconComponent icon="menu" />
            </ButtonComponent>
        </div>
        <div class="links flex gap flex-auto" :class="{ 'is-menu-open': isMenuOpen }">
            <RouterLink class="flex-auto" v-for="link in links.filter(x => x.isVisible)" :to="link.path">
                <IconComponent :icon="link.iconName" gap="right" />
                <span>{{ link.title }}</span>
            </RouterLink>
        </div>
    </nav>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import { linkFactory } from './link-factory';
import { subdomain } from '@/setup/router/router-helper';

const isMenuOpen = ref<boolean>(false);

const onToggleMenu = function (): void {
    isMenuOpen.value = !isMenuOpen.value;
};

const links = linkFactory.get(subdomain);
</script>

<style lang="scss">
@use '@/style/variables' as *;

.nav-component {
    $inset: 0.5rem;

    display: grid;
    grid-template-columns: repeat(3, 1fr);
    align-items: center;
    gap: 1rem;
    position: fixed;
    justify-content: center;
    inset: $inset $inset auto $inset;
    padding: 2rem 1rem;
    line-height: 1em;
    background-color: var(--wjb-primary);
    background: linear-gradient(
        -5deg,
        transparentize($primary-dark, 0.1),
        transparentize($primary, 0.1),
    );
    backdrop-filter: blur(2px);
    color: var(--wjb-light);
    text-shadow: 1px 1px rgba(0, 0, 0, 0.6);
    box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 12px 24px -12px rgba(0, 0, 0, 0.5);
    border: 1px solid var(--wjb-primary-dark);
    border-radius: var(--wjb-border-radius);
    z-index: 1;

    svg {
        filter: drop-shadow(1px 1px rgba(0, 0, 0, 0.6));
    }

    a {
        color: inherit;
        text-decoration: none;
        font-weight: bold;

        span {
            vertical-align: middle;
        }
    }

    .logo-container {
        width: 40px;
        height: 40px;
        margin: -1rem -0.5rem -1rem 0;
        padding: 0.25rem;
        background-color: var(--wjb-light);
        border-radius: 50%;

        @include shadow-small();

        img {
            max-width: 100%;
        }
    }

    .menu {
        display: none;
    }

    .links {
        justify-content: right;
    }

    .custom {
        display: flex;
        justify-content: center;
        align-items: center;
        line-height: 1.2em;
    }

    @media screen and (max-width: 720px) {
        padding: 1rem;
        grid-template-columns: auto 1fr auto;

        .logo-text {
            display: none;
        }

        .menu {
            display: unset;
        }

        .links {
            display: block;
            position: fixed;
            padding: 1rem;
            inset: 4rem 0 auto 0;
            background-color: var(--wjb-primary);
            background: linear-gradient(
                -5deg,
                transparentize($primary-dark, 0.1),
                transparentize($primary, 0.1),
            );
            backdrop-filter: blur(2px);
            text-shadow: 1px 1px rgba(0, 0, 0, 0.6);
            box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 12px 24px -12px rgba(0, 0, 0, 0.5);
            border: 1px solid var(--wjb-primary-dark);
            border-radius: var(--wjb-border-radius);
            z-index: 1;
            opacity: 0;
            pointer-events: none;

            &.is-menu-open {
                opacity: 1;
                pointer-events: all;
            }

            & > * {
                display: block;
                padding: 1rem;
                border-radius: var(--wjb-border-radius);

                &:hover {
                    background-color: var(--wjb-primary-dark);
                }
            }
        }
    }
}
</style>