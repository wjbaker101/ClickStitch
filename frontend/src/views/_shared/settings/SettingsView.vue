<template>
    <ViewComponent>
        <template #nav>
            <strong>Settings</strong>
        </template>
        <CardComponent border="top" padded v-if="authDetails !== null" class="mb-4">
            <h2 class="mb-4 text-2xl font-bold">User Details</h2>
            <p><strong>Currently logged in as: </strong>{{ authDetails.email }}</p>
            <LoadingComponent v-if="isLoading" itemName="user details" />
            <template v-else-if="self !== null">
                <p><strong>Created at:</strong> {{ self.user.createdAt }} ({{ self.user.createdAt.fromNow() }})</p>
                <p><strong>Last logged in:</strong> {{ self.user.lastLoginAt }} ({{ self.user.lastLoginAt?.fromNow() }})</p>
            </template>
            <p class="text-center">
                <BtnComponent @click="onLogOut">Log Out</BtnComponent>
            </p>
        </CardComponent>
        <div v-if="isCreator || isAdmin" class="flex gap-4">
            <CardComponent border="top" padded v-if="authDetails !== null" class="grow basis-1">
                <h2 class="mb-4 text-2xl font-bold">You Are a Creator!</h2>
                <p>You'll have the ability to edit your creator details and patterns here.</p>
                <p>
                    <a :href="urlToSubdomain('creator')">
                        <BtnComponent>
                            <ExternalLinkIcon class="mr-2" />
                            <span class="align-middle">Go to Creator Dashboard</span>
                        </BtnComponent>
                    </a>
                </p>
            </CardComponent>
            <CardComponent border="top" padded v-if="authDetails !== null" class="grow basis-1">
                <h2 class="mb-4 text-2xl font-bold">You Are an Admin!</h2>
                <p>You'll have the ability to view and edit users on the platform.</p>
                <p>
                    <a :href="urlToSubdomain('admin')">
                        <BtnComponent>
                            <ExternalLinkIcon class="mr-2" />
                            <span class="align-middle">Go to Admin Dashboard</span>
                        </BtnComponent>
                    </a>
                </p>
            </CardComponent>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';

import { ExternalLinkIcon } from 'lucide-vue-next';
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