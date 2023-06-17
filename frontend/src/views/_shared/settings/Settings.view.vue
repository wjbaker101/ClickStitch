<template>
    <ViewComponent class="settings-view">
        <template #nav>
            <strong>Settings</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded v-if="authDetails !== null">
                    <h2>Authentication</h2>
                    <p><strong>Currently logged in as: </strong>{{ authDetails.email }}</p>
                    <p><strong>Session started at:</strong> {{ authDetails.loggedInAt }}</p>
                    <p class="text-centered">
                        <ButtonComponent @click="onLogOut">Log Out</ButtonComponent>
                    </p>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';

import { useAuth } from '@/use/auth/Auth.use';

const auth = useAuth();
const router = useRouter();

const authDetails = auth.details;

const onLogOut = function (): void {
    auth.clear();
    router.push({ path: '/login' });
};
</script>

<style lang="scss">
.settings-view {

    .creator-name {
        width: 350px;
        max-width: 100%;
    }
}
</style>