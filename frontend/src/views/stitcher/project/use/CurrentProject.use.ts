import { computed, ref } from 'vue';

import { type IGetProject } from '@/models/GetProject.model';
import { type IStitch, type IPatternThread } from '@/models/Pattern.model';

const project = ref<IGetProject | null>(null);

const palette = computed<Map<number, IPatternThread>>(() => {
    if (project.value === null)
        return new Map<number, IPatternThread>();

    const _palette = new Map<number, IPatternThread>();
    for (const thread of project.value.threads) {
        _palette.set(thread.thread.index, thread.thread);
    }

    return _palette;
});

const stitches = computed<Array<IStitch>>(() => {
    if (project.value === null)
        return [];

    let _stitches: Array<IStitch> = [];

    _stitches = _stitches.concat(project.value.threads.flatMap(thread => thread.stitches.map<IStitch>(stitch => ({
        x: stitch[0],
        y: stitch[1],
        threadIndex: thread.thread.index,
        stitchedAt: null,
    }))));

    _stitches = _stitches.concat(project.value.threads.flatMap(thread => thread.completedStitches.map<IStitch>(stitch => ({
        x: stitch[0],
        y: stitch[1],
        threadIndex: thread.thread.index,
        stitchedAt: stitch[2],
    }))));

    return _stitches;
});

export const useCurrentProject = function () {
    return {
        palette,
        stitches,

        setProject(newProject: IGetProject): void {
            project.value = newProject;
        },
    };
};