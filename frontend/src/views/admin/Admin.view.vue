<template>
    <ViewComponent class="admin-view">
        <template #nav>
            <strong>Admin</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <h2>Users</h2>
                    <LoadingComponent v-if="getUsers === null" />
                    <div v-else>
                        <UserItemComponent
                            :key="userDetails.user.reference"
                            v-for="userDetails in getUsers.users"
                            :userDetails="userDetails"
                        />
                    </div>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import UserItemComponent from './components/UserItem.component.vue';

import { api } from '@/api/api';

import { IGetUsers } from '@/models/GetUsers.model';

const getUsers = ref<IGetUsers | null>(null);

onMounted(async () => {
    const result = await api.admin.getUsers(1, 50);
    if (result instanceof Error)
        return;

    getUsers.value = result;
});
</script>

<style lang="scss">
.admin-view {

    .user-item {
        border-left: 3px solid transparent;

        &.is-admin {
            border-left: 3px solid #22c;
        }
    }
}
</style>