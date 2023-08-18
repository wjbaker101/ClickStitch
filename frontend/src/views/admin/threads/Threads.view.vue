<template>
    <ViewComponent class="threads-view">
        <template #nav>
            <strong>Threads</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <h2>Threads</h2>
                    <LoadingComponent v-if="threads === null" />
                    <div v-else>
                        <ListItemComponent
                            :key="userDetails.user.reference"
                            v-for="userDetails in threads.users"
                        >
                        </ListItemComponent>
                    </div>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import ListItemComponent from '@/components/ListItem.component.vue';

import { api } from '@/api/api';

import { type IGetUsers } from '@/models/GetUsers.model';

const threads = ref<IGetUsers | null>(null);

onMounted(async () => {
    const usersResult = await api.admin.getUsers(1, 50);
    if (usersResult instanceof Error)
        return;

    threads.value = usersResult;
});
</script>

<style lang="scss">
.threads-view {
}
</style>