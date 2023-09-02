<template>
    <div class="project-stitches-modal-component">
        <h2>Stitches</h2>
        <div class="stitches">
            <ListItemComponent v-for="thread in threads">
                <div class="flex align-items-center">
                    <div>
                        <div class="thread-colour text-centered" :style="threadStyle(thread.thread)">
                            {{ thread.thread.index }}
                        </div>
                        <span class="thread-text">
                            <strong>{{ thread.thread.name }}</strong> - <small>{{ thread.thread.description }}</small>
                        </span>
                    </div>
                    <div class="flex-auto">
                        {{ thread.completedStitches.length }} <small>/ {{ thread.stitches.length }}</small>
                    </div>
                </div>
                <template #expanded>
                    <div class="thread-actions flex gap align-items-center">
                        <div>
                            <strong>{{ thread.completedStitches.length / thread.stitches.length * 100 }}%</strong> Completed
                        </div>
                    </div>
                </template>
            </ListItemComponent>
        </div>
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
            margin-top: 0.125rem;
        }

        &.is-expanded {
            .more-content {
                margin-top: 0;
            }
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

    .thread-actions {
        padding: 1rem;
    }
}
</style>