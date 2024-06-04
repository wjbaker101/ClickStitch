<template>
    <ListItemComponent
        class="border-l-2 border-solid border-transparent"
        :class="{
            'is-creator': userDetails.permissions.find(x => x.type === 'Creator') !== undefined,
            'is-admin': userDetails.permissions.find(x => x.type === 'Admin') !== undefined,
        }"
        hover
    >
        <p>
            <IconComponent icon="user" gap="right" />
            <strong>{{ userDetails.user.email }}</strong>
        </p>
        <p>
            <strong>Created At: </strong>
            <span>{{ userDetails.user.createdAt.format('DD/MM/YYYY HH:mm:ss') }} ({{ userDetails.user.createdAt.fromNow() }})</span>
        </p>
        <p>
            <strong>Last Logged In: </strong>
            <span v-if="userDetails.user.lastLoginAt !== null">{{ userDetails.user.lastLoginAt.format('DD/MM/YYYY HH:mm:ss') }} ({{ userDetails.user.lastLoginAt.fromNow() }})</span>
            <span v-else>Never :(</span>
        </p>
        <template #expanded>
            <h3 class="m-0 mb-4">Permissions:</h3>
            <div v-for="permission in displayPermissions">
                <CheckBoxComponent :label="permission.name" v-model="permission.isEnabled" @change="onPermissionChange(permission, $event)" />
            </div>
        </template>
    </ListItemComponent>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import CheckBoxComponent from '@/components/inputs/CheckBoxComponent.vue';
import ListItemComponent from '@/components/ListItemComponent.vue';

import { api } from '@/api/api';

import { type IUserDetails } from '@/models/GetUsers.model';
import { type IPermission } from '@/models/Permission.model';
import { permissionMapper } from '@/api/mappers/Permission.mapper';

const props = defineProps<{
    userDetails: IUserDetails;
    permissions: Array<IPermission>;
}>();

const displayPermissions = ref(props.permissions.map(x => ({
    name: x.name,
    type: x.type,
    isEnabled: props.userDetails.permissions.find(y => y.type === x.type) !== undefined,
})));

const onPermissionChange = async function (permission: IPermission, event: { isChecked: boolean }): Promise<void> {
    if (event.isChecked) {
        await api.admin.assignPermissionToUser(props.userDetails.user.reference, {
            permissionType: permissionMapper.mapTypeToApi(permission.type),
        });
    }
    else {
        await api.admin.removePermissionFromUser(props.userDetails.user.reference, permissionMapper.mapTypeToApi(permission.type));
    }
};
</script>

<style lang="scss" scoped>
* {

    &.is-creator {
        border-left-color: #2c3;
    }

    &.is-admin {
        border-left-color: #22c;
    }
}
</style>