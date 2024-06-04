<template>
    <ViewComponent class="supported-pattern-formats-view">
        <template #nav>
            <strong>Supported Pattern Formats</strong>
        </template>
        <div class="content-width">
            <CardComponent border="top" padded class="mb-4">
                <h2>What are pattern schematics?</h2>
                <p>When creating a digital pattern, the individual stitches must be saved in a specific format so that it can be read by the website or program you created it on.</p>
                <p>ClickStitch will take that format and save it, so that stitchers are able to track their own progress on patterns you create!</p>
                <p>This page details which pattern formats are supported. Feel free to ask for additional formats to be supported.</p>
            </CardComponent>
            <CardComponent border="top" padded class="mb-4">
                <h2>FlossCross.com</h2>
                <ul>
                    <li><code>.fcjson</code></li>
                    <li><code>.oxs</code> (Same as MacStitch / WinStitch)</li>
                </ul>
                <p>Example method of exporting via the dashboard:</p>
                <img class="rounded-md shadow-md" src="@/assets/supported-pattern-formats/flosscross.png">
            </CardComponent>
            <CardComponent border="top" padded class="mb-4">
                <h2>MacStitch / WinStitch</h2>
                <ul>
                    <li><code>.oxs</code></li>
                </ul>
                <p>Example method of exporting in WinStitch:</p>
                <img class="rounded-md shadow-md" src="@/assets/supported-pattern-formats/winstitch.png">
            </CardComponent>
        </div>
    </ViewComponent>
</template>

<script setup lang="ts">
</script>