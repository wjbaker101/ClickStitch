<template>
    <ViewComponent class="admin-view">
        <template #nav>
            <strong>Admin</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <h2>Users</h2>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
</script>

<style lang="scss">
.admin-view {}
</style>