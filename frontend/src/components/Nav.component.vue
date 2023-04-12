<template>
    <nav class="nav-component flex gap align-items-center">
        <strong class="flex-auto">ClickStitch</strong>
        <div class="custom align-items-center">
            <slot></slot>
        </div>
        <div class="menu text-right">
            <ButtonComponent class="mini" @click="onToggleMenu">
                <IconComponent icon="menu" />
            </ButtonComponent>
        </div>
        <div class="links flex gap flex-auto" :class="{ 'is-menu-open': isMenuOpen }">
            <RouterLink class="flex-auto" to="/dashboard">
                <IconComponent icon="home" gap="right" />
                <span>Dashboard</span>
            </RouterLink>
            <RouterLink class="flex-auto" to="/marketplace">
                <IconComponent icon="download" gap="right" />
                <span>Marketplace</span>
            </RouterLink>
            <RouterLink class="flex-auto" to="/settings">
                <IconComponent icon="settings" gap="right" />
                <span>Settings</span>
            </RouterLink>
        </div>
    </nav>
</template>

<script setup lang="ts">
import { ref } from 'vue';

const isMenuOpen = ref<boolean>(false);

const onToggleMenu = function (): void {
    isMenuOpen.value = !isMenuOpen.value;
};
</script>

<style lang="scss">
@use '@/style/variables' as *;

.nav-component {
    position: fixed;
    inset: 0.5rem 0.5rem auto 0.5rem;
    padding: 1rem;
    line-height: 1em;
    background-color: var(--wjb-primary);
    background: linear-gradient(
        -5deg,
        transparentize($primary-dark, 0.05),
        transparentize($primary, 0.05),
    );
    backdrop-filter: blur(2px);
    border-radius: 2rem;
    z-index: 1;

    @include shadow-large();

    a {
        color: inherit;
        text-decoration: none;
        font-weight: bold;

        span {
            vertical-align: middle;
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
        .menu {
            display: unset;
        }

        .links {
            display: block;
            position: fixed;
            padding: 1rem;
            inset: 4rem 1rem auto 1rem;
            background-color: var(--wjb-primary);
            background: linear-gradient(
                -5deg,
                transparentize($primary-dark, 0.05),
                transparentize($primary, 0.05),
            );
            backdrop-filter: blur(2px);
            border-radius: var(--wjb-border-radius);
            z-index: 1;
            opacity: 0;
            pointer-events: none;

            &.is-menu-open {
                opacity: 1;
                pointer-events: all;
            }

            & > *{
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