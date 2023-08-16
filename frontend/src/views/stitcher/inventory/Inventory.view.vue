<template>
    <ViewComponent class="inventory-view">
        <template #nav>
            <strong>Inventory</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <h2>Manage Your Threads</h2>
                    <FormComponent>
                        <FormSectionComponent class="flex align-items-center">
                            <FormInputComponent label="Search">
                                <input type="search" placeholder="DMC 814">
                            </FormInputComponent>
                            <div class="flex-auto">
                                <ButtonComponent @click="onAddThread">
                                    <IconComponent icon="plus" gap="right" />
                                    <span>Add Thread</span>
                                </ButtonComponent>
                            </div>
                        </FormSectionComponent>
                    </FormComponent>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import AddThreadModalComponent from '@/views/stitcher/inventory/modals/AddThreadModal.component.vue';

import { useModal } from '@wjb/vue/use/modal.use';

const modal = useModal();

const onAddThread = function (): void {
    modal.show({
        component: AddThreadModalComponent,
        componentProps: {},
    });
};
</script>

<style lang="scss">
</style>