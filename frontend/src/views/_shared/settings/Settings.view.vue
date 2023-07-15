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
            <section v-if="isCreator || isAdmin" class="flex gap">
                <CardComponent border="top" padded v-if="authDetails !== null">
                    <h2>You Are a Creator!</h2>
                    <p>You'll have the ability to edit your creator details and patterns here.</p>
                    <p>
                        <a :href="urlToSubdomain('creator')">
                            <ButtonComponent>
                                <IconComponent icon="external-link" gap="right" />
                                <span>Go to Creator Dashboard</span>
                            </ButtonComponent>
                        </a>
                    </p>
                </CardComponent>
                <CardComponent border="top" padded v-if="authDetails !== null">
                    <h2>You Are an Admin!</h2>
                    <p>You'll have the ability to view and edit users on the platform.</p>
                    <p>
                        <a :href="urlToSubdomain('admin')">
                            <ButtonComponent>
                                <IconComponent icon="external-link" gap="right" />
                                <span>Go to Admin Dashboard</span>
                            </ButtonComponent>
                        </a>
                    </p>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRouter } from 'vue-router';

import { useAuth } from '@/use/auth/Auth.use';
import { type Subdomain } from '@/setup/router/router-helper';

const auth = useAuth();
const router = useRouter();

const authDetails = auth.details;

const isCreator = computed<boolean>(() => authDetails.value?.permissions.find(x => x.type === 'Creator') !== undefined);
const isAdmin = computed<boolean>(() => authDetails.value?.permissions.find(x => x.type === 'Admin') !== undefined);

const urlToSubdomain = function (subdomain: Subdomain): string {
    const baseDomain = window.location.host;
    const protocol = window.location.protocol;

    return `${protocol}//${subdomain}.${baseDomain}`;
};

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