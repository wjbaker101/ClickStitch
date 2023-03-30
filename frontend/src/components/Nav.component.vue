<template>
    <nav class="nav-component flex gap align-items-center" v-if="!isLoginPage">
        <strong>ClickStitch</strong>
        <div></div>
        <div class="flex-auto">
            <RouterLink to="/dashboard">
                <IconComponent icon="home" gap="right" />
                <span>Dashboard</span>
            </RouterLink>
        </div>
        <div class="flex-auto">
            <RouterLink to="/marketplace">
                <IconComponent icon="download" gap="right" />
                <span>Marketplace</span>
            </RouterLink>
        </div>
        <div class="flex-auto">
            <RouterLink to="/settings">
                <IconComponent icon="settings" gap="right" />
                <span>Settings</span>
            </RouterLink>
        </div>
    </nav>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRoute } from 'vue-router';

const route = useRoute();

const isLoginPage = computed<boolean>(() => route.path.startsWith('/login'));
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
}
</style>