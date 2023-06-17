<template>
    <ViewComponent class="creator-patterns-view">
        <template #nav>
            <strong>Creator Patterns</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <h2>Your Patterns</h2>
                    <p>Manage your patterns here.</p>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
</script>

<style lang="scss">
.creator-patterns-view {}
</style>