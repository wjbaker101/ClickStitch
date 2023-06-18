<template>
    <ViewComponent class="new-pattern-view">
        <template #nav>
            <strong>New Pattern</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <h2>Create a New Pattern</h2>
                    <RouterLink class="flex align-items-center" to="/patterns">
                        <IconComponent class="flex-auto" icon="arrow-left" gap="right" />
                        <small>Back to patterns</small>
                    </RouterLink>
                    <FormComponent>
                        <FormSectionComponent>
                            <h3>Pattern Details</h3>
                            <FormInputComponent label="Title">
                                <input type="text">
                            </FormInputComponent>
                            <ImageUploadComponent heading="Banner Image/ Thumbnail" />
                            <p>
                                <small>This image will also be resized and used as the thumbnail.</small>
                            </p>
                        </FormSectionComponent>
                        <FormSectionComponent>
                            <ButtonComponent>
                                <IconComponent icon="plus" gap="right" />
                                <span>Create</span>
                            </ButtonComponent>
                        </FormSectionComponent>
                    </FormComponent>
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
import ImageUploadComponent from '@/components/ImageUpload.component.vue';

</script>

<style lang="scss">
.new-pattern-view {}
</style>