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
                <div v-if="userDetails.permissions.length === 0">
                    No permissions
                </div>
                <div v-else v-for="permission in userDetails.permissions">
                    {{ permission.name }}
                </div>
            </section>
        </template>
    </ListItemComponent>
</template>

<script setup lang="ts">
import ListItemComponent from '@/components/ListItem.component.vue';

import { IUserDetails } from '@/models/GetUsers.model';

defineProps<{
    userDetails: IUserDetails;
}>();
</script>

<style lang="scss">
.user-item-component {
    border-left: 3px solid transparent;

    &.is-admin {
        border-left: 3px solid #22c;
    }
}
</style>