<template>
    <ItemComponent
        class="user-item-component hover"
        :class="{
            'is-admin': userDetails.permissions.find(x => x.type === 'Admin') !== undefined,
            'is-open': isOpen,
        }"
    >
        <div class="flex gap align-items-center">
            <div>
                <strong>{{ userDetails.user.email }}</strong>
                <br>
                <span>{{ userDetails.user.createdAt.format('DD/MM/YYYY HH:mm:ss') }} ({{ userDetails.user.createdAt.fromNow() }})</span>
            </div>
            <div class="flex-auto" @click="onToggleOpen">
                <IconComponent icon="arrow-triangle-down" gap="right" />
                <span style="vertical-align: middle;">More</span>
            </div>
        </div>
        <div class="more-content">
            <section>
                <h3>Permissions:</h3>
                <div v-if="userDetails.permissions.length === 0">
                    No permissions
                </div>
                <div v-else v-for="permission in userDetails.permissions">
                    {{ permission.name }}
                </div>
            </section>
        </div>
    </ItemComponent>
</template>

<script setup lang="ts">
import { ref } from 'vue';

import ItemComponent from './Item.component.vue';

import { IUserDetails } from '@/models/GetUsers.model';

defineProps<{
    userDetails: IUserDetails;
}>();

const isOpen = ref<boolean>(false);

const onToggleOpen = function (): void {
    isOpen.value = !isOpen.value;
};
</script>

<style lang="scss">
.user-item-component {
    border-left: 3px solid transparent;

    &.is-admin {
        border-left: 3px solid #22c;
    }

    &.is-open {
        .more-content {
            display: block;
        }
    }

    .more-content {
        display: none;
        margin-top: 1rem;

        section {
            margin-left: 0.5rem;
            padding-left: 1rem;
            border-left: 1px solid #ccc;

            & + section {
                margin-top: 0.5rem;
            }
        }
    }
}
</style>