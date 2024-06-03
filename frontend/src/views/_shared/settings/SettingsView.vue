<template>
    <ViewComponent class="settings-view">
        <template #nav>
            <strong>Settings</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded v-if="authDetails !== null">
                    <h2>User Details</h2>
                    <p><strong>Currently logged in as: </strong>{{ authDetails.email }}</p>
                    <LoadingComponent v-if="isLoading" itemName="user details" />
                    <template v-else-if="self !== null">
                        <p><strong>Created at:</strong> {{ self.user.createdAt }} ({{ self.user.createdAt.fromNow() }})</p>
                        <p><strong>Last logged in:</strong> {{ self.user.lastLoginAt }} ({{ self.user.lastLoginAt?.fromNow() }})</p>
                    </template>
                    <p class="text-centered">
                        <BtnComponent @click="onLogOut">Log Out</BtnComponent>
                    </p>
                </CardComponent>
            </section>
            <section v-if="isCreator || isAdmin" class="flex gap">
                <CardComponent border="top" padded v-if="authDetails !== null">
                    <h2>You Are a Creator!</h2>
                    <p>You'll have the ability to edit your creator details and patterns here.</p>
                    <p>
                        <a :href="urlToSubdomain('creator')">
                            <BtnComponent>
                                <IconComponent icon="external-link" gap="right" />
                                <span>Go to Creator Dashboard</span>
                            </BtnComponent>
                        </a>
                    </p>
                </CardComponent>
                <CardComponent border="top" padded v-if="authDetails !== null">
                    <h2>You Are an Admin!</h2>
                    <p>You'll have the ability to view and edit users on the platform.</p>
                    <p>
                        <a :href="urlToSubdomain('admin')">
                            <BtnComponent>
                                <IconComponent icon="external-link" gap="right" />
                                <span>Go to Admin Dashboard</span>
                            </BtnComponent>
                        </a>
                    </p>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';

import BtnComponent from '@/components/BtnComponent.vue';
import LoadingComponent from '@/components/loading/LoadingComponent.vue';

import { useAuth } from '@/use/auth/Auth.use';
import { api } from '@/api/api';

import { type Subdomain } from '@/setup/router/router-helper';
import { type IGetSelf } from '@/models/GetSelf.model';

const auth = useAuth();

const authDetails = auth.details;
const self = ref<IGetSelf | null>(null);
const isLoading = ref<boolean>(false);

const isCreator = computed<boolean>(() => authDetails.value?.permissions.find(x => x.type === 'Creator') !== undefined);
const isAdmin = computed<boolean>(() => authDetails.value?.permissions.find(x => x.type === 'Admin') !== undefined);

const urlToSubdomain = function (subdomain: Subdomain): string {
    const baseDomain = window.location.host;
    const protocol = window.location.protocol;

    return `${protocol}//${subdomain}.${baseDomain}`;
};

const onLogOut = function (): void {
    auth.clear();
};

onMounted(async () => {
    isLoading.value = true;

    const result = await api.users.getSelf();

    isLoading.value = false;

    if (result instanceof Error)
        return;

    self.value = result;
});
</script>