<template>
    <ViewComponent class="inventory-view">
        <template #nav>
            <strong>Inventory</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <h2>Manage Your Threads</h2>
                    <section>
                        <FormComponent>
                            <FormSectionComponent class="flex align-items-center">
                                <FormInputComponent label="Search">
                                    <input type="search" placeholder="DMC 814" v-model="searchTerm">
                                </FormInputComponent>
                            </FormSectionComponent>
                        </FormComponent>
                    </section>
                    <section>
                        <p v-if="matches.length === 0" class="text-centered">
                            Enter a thread code above and select how many you have.
                        </p>
                        <ThreadItemComponent v-for="thread in matches" :thread="thread" />
                    </section>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import { computed, ref, watch } from 'vue';

import ThreadItemComponent from '@/views/stitcher/inventory/components/ThreadItem.component.vue';

import { knownThreads, type IKnownThread } from '@/data/known-threads';

const searchTerm = ref<string>('');
const matches = ref<Array<IKnownThread>>([]);

const searchTermSanitised = computed<string>(() => searchTerm.value.trim().toLowerCase());

watch(searchTerm, () => {
    if (searchTermSanitised.value.length < 1)
        return;

    matches.value = knownThreads
        .filter(x => x.code.toLowerCase().indexOf(searchTermSanitised.value) > -1)
        .sort((a, b) => a.code.localeCompare(b.code));
});
</script>

<style lang="scss">
.inventory-view {

    .thread-image {
        width: 4rem;
        aspect-ratio: 1;
        border-radius: 50%;
        vertical-align: middle;
        box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);
    }
}
</style>