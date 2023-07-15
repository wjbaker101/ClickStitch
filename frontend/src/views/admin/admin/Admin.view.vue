<template>
    <ViewComponent class="admin-view">
        <template #nav>
            <strong>Admin</strong>
        </template>
        <div class="content-width" v-if="permissions">
            <section>
                <CardComponent border="top" padded>
                    <h2>Users</h2>
                    <LoadingComponent v-if="getUsers === null" />
                    <div v-else>
                        <UserItemComponent
                            :key="userDetails.user.reference"
                            v-for="userDetails in getUsers.users"
                            :userDetails="userDetails"
                            :permissions="permissions"
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

import { type IGetUsers } from '@/models/GetUsers.model';
import { type IPermission } from '@/models/Permission.model';

const getUsers = ref<IGetUsers | null>(null);
const permissions = ref<Array<IPermission> | null>(null);

onMounted(async () => {
    const usersResult = await api.admin.getUsers(1, 50);
    if (usersResult instanceof Error)
        return;

    getUsers.value = usersResult;

    const permissionsResult = await api.admin.getPermissions();
    if (permissionsResult instanceof Error)
        return;

    permissions.value = permissionsResult;
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