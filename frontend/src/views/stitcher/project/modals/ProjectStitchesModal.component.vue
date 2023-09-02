<template>
    <div class="project-stitches-modal-component">
        <h2>Stitches</h2>
        <ul class="stitches">
            <ListItemComponent v-for="thread in threads">
                <div class="thread-colour text-centered" :style="threadStyle(thread.thread)">
                    {{ thread.thread.index }}
                </div>
                <span class="thread-text">
                    <code>{{ thread.thread.name }}</code> - <small>{{ thread.thread.description }}</small>
                </span>
                <!-- <template #expanded>
                    asd
                </template> -->
            </ListItemComponent>
        </ul>
    </div>
</template>

<script setup lang="ts">
import type { StyleValue } from 'vue';

import ListItemComponent from '@/components/ListItem.component.vue';

import { isDark } from '@/helper/helper';

import type { IGetProject } from '@/models/GetProject.model';
import type { IPatternThread } from '@/models/Pattern.model';

const props = defineProps<{
    project: IGetProject;
}>();

const threads = props.project.threads;

const threadStyle = function (thread: IPatternThread): StyleValue {
    return {
        backgroundColor: thread.colour,
        color: isDark(thread.colour) ? '#fff' : '#000',
    };
};
</script>

<style lang="scss">
.project-stitches-modal-component {

    .list-item-component {
        padding: 0;
        padding-right: 0.5rem;

        & + .list-item-component {
            margin-top: 1px;
        }

        &.is-expanded {
            .more-content {
                margin-top: 0;
                padding: 0.5rem;
            }
        }
    }

    .stitches {
        padding-left: 0;
        list-style: none;

        & > li {
            margin-top: 1px;
        }
    }

    .thread-colour {
        width: 2rem;
        line-height: 2rem;
        margin-right: 0.5rem;
        display: inline-block;
        aspect-ratio: 1;
        vertical-align: middle;
        border-radius: var(--wjb-border-radius);
    }

    .thread-text {
        vertical-align: middle;
    }
}
</style>