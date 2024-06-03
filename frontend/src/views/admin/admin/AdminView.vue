<template>
    <ViewComponent class="admin-view">
        <template #nav>
            <strong>Admin</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <h2>Users</h2>
                    <PaginatedContentComponent loadingItemName="users" :logic="loadUsers" :pageSize="10">
                        <UserItemComponent
                            v-if="getUsers !== null && permissions !== null"
                            :key="userDetails.user.reference"
                            v-for="userDetails in getUsers.users"
                            :userDetails="userDetails"
                            :permissions="permissions"
                        />
                    </PaginatedContentComponent>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import PaginatedContentComponent from '@/components/paginated-content/PaginatedContentComponent.vue';
import UserItemComponent from './components/UserItemComponent.vue';

import { api } from '@/api/api';
import { paginationMapper } from '@/api/mappers/Pagination.mapper';

import type { IGetUsers } from '@/models/GetUsers.model';
import type { IPermission } from '@/models/Permission.model';
import type { IPagination } from '@/models/Pagination.model';

const getUsers = ref<IGetUsers | null>(null);
const permissions = ref<Array<IPermission> | null>(null);

const loadUsers = async function (pageNumber: number, pageSize: number): Promise<IPagination | Error> {
    const usersResult = await api.admin.getUsers(pageNumber, pageSize);
    if (usersResult instanceof Error)
        return usersResult;

    getUsers.value = usersResult;

    const permissionsResult = await api.admin.getPermissions();
    if (permissionsResult instanceof Error)
        return permissionsResult;

    permissions.value = permissionsResult;

    return paginationMapper.map(getUsers.value.pagination);
};
</script>