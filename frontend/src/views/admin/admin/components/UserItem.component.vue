<template>
    <ListItemComponent
        class="user-item-component"
        :class="{
            'is-admin': userDetails.permissions.find(x => x.type === 'Admin') !== undefined,
        }"
        hover
    >
        <strong>{{ userDetails.user.email }}</strong>
        <br>
        <span>{{ userDetails.user.createdAt.format('DD/MM/YYYY HH:mm:ss') }} ({{ userDetails.user.createdAt.fromNow() }})</span>
        <template #expanded>
            <section>
                <h3>Permissions:</h3>
                <div v-for="permission in displayPermissions">
                    <input type="checkbox" v-model="permission.isEnabled" @change="onPermissionChange(permission, $event)"> {{ permission.name }}
                </div>
            </section>
        </template>
    </ListItemComponent>
</template>

<script setup lang="ts">
import { computed } from 'vue';

import ListItemComponent from '@/components/ListItem.component.vue';

import { api } from '@/api/api';

import { type IUserDetails } from '@/models/GetUsers.model';
import { type IPermission, type PermissionType } from '@/models/Permission.model';
import { permissionMapper } from '@/api/mappers/Permission.mapper';

const props = defineProps<{
    userDetails: IUserDetails;
    permissions: Array<IPermission>;
}>();

interface IDisplayPermission {
    readonly name: string;
    readonly type: PermissionType;
    isEnabled: boolean;
}

const displayPermissions = computed<Array<IDisplayPermission>>(() => props.permissions.map(x => ({
    name: x.name,
    type: x.type,
    isEnabled: props.userDetails.permissions.find(y => y.type === x.type) !== undefined,
})));

const onPermissionChange = async function (permission: IPermission, event: Event): Promise<void> {
    const element = event.target as HTMLInputElement;
    const isEnabled = element.checked;

    if (isEnabled) {
        await api.admin.assignPermissionToUser(props.userDetails.user.reference, {
            permissionType: permissionMapper.mapTypeToApi(permission.type),
        });
    }
    else {
        await api.admin.removePermissionFromUser(props.userDetails.user.reference, permissionMapper.mapTypeToApi(permission.type));
    }
};
</script>

<style lang="scss">
.user-item-component {
    border-left: 3px solid transparent;

    &.is-admin {
        border-left: 3px solid #22c;
    }
}
</style>