<template>
    <ViewComponent class="new-pattern-view">
        <template #nav>
            <strong>New Pattern</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <h2>Create a New Pattern</h2>
                    <RouterLink class="link-component" to="/patterns">
                        <IconComponent class="flex-auto" icon="arrow-left" gap="right" />
                        <small>Back to Patterns</small>
                    </RouterLink>
                    <FormComponent>
                        <FormSectionComponent>
                            <h3>Pattern Details</h3>
                            <FormInputComponent label="Title">
                                <input type="text" placeholder="My Amazing Pattern">
                            </FormInputComponent>
                            <ImageUploadComponent heading="Banner Image/ Thumbnail" />
                            <FormInputComponent label="Shop Url">
                                <small><em>A link to the pattern where stitchers can buy it</em></small>
                                <input type="text" placeholder="https://etsy.com/shop/beautifulpatternsco/amazing_pattern">
                            </FormInputComponent>
                        </FormSectionComponent>
                        <FormSectionComponent>
                            <p>
                                Once you're happy with the details, click the button below.
                                <br>
                                These details will be changable later, so don't worry if you spot a mistake after submitting!
                            </p>
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