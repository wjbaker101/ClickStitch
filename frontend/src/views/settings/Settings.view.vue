<template>
    <ViewComponent class="settings-view">
        <div class="content-width">
            <h1>Settings</h1>
            <ButtonComponent @click="onLogOut">Log Out</ButtonComponent>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { useAuth } from '@/use/auth/Auth.use';
import { useRouter } from 'vue-router';

const auth = useAuth();
const router = useRouter();

const onLogOut = function (): void {
    auth.clear();
    router.push({ path: '/login' });
};
</script>

<style lang="scss">
.settings-view {
}
</style>