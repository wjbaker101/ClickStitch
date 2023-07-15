<template>
    <ViewComponent class="supported-pattern-formats-view">
        <template #nav>
            <strong>Supported Pattern Formats</strong>
        </template>
        <div class="content-width">
            <section>
                <CardComponent border="top" padded>
                    <h2>What are pattern schematics?</h2>
                    <p>When creating a digital pattern, the individual stitches must be saved in a specific format so that it can be read by the website or program you created it on.</p>
                    <p>ClickStitch will take that format and save it, so that stitchers are able to track their own progress on patterns you create!</p>
                    <p>This page details which pattern formats are supported. Feel free to ask for additional formats to be supported.</p>
                </CardComponent>
            </section>
            <section>
                <CardComponent border="top" padded>
                    <h2>FlossCross.com</h2>
                    <ul>
                        <li><code>.fcjson</code></li>
                        <li><code>.oxs</code></li>
                    </ul>
                    <img class="example-image" src="@/assets/supported-pattern-formats/flosscross.png">
                </CardComponent>
            </section>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
</script>

<style lang="scss">
.supported-pattern-formats-view {

    .example-image {
        border-radius: var(--wjb-border-radius);
        box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1), 0 6px 16px -12px rgba(0, 0, 0, 1);
    }
}
</style>