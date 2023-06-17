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
            <section>
                <CardComponent border="top" padded>
                    <ListItemComponent>
                        <h3>asd</h3>
                        <template #expanded>
                            asd123
                        </template>
                    </ListItemComponent>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

import ListItemComponent from '@/components/ListItem.component.vue';

import { api } from '@/api/api';
import { IGetCreatorPatterns } from '@/models/GetCreatorPatterns.model';

const getPatterns = ref<IGetCreatorPatterns | null>(null);

onMounted(async () => {
    const selfResult = await api.creators.getSelf();
    if (selfResult instanceof Error)
        return;

    if (selfResult === null) {
        return;
    }

    const getPatternsResult = await api.creators.getPatterns(selfResult.reference, 5, 1);
    if (getPatternsResult instanceof Error)
        return;

    getPatterns.value = getPatternsResult;
});
</script>

<style lang="scss">
.creator-patterns-view {}
</style>