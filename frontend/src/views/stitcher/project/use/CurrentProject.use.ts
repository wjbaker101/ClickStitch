import { computed, ref, type Ref } from 'vue';

import { type IGetProject } from '@/models/GetProject.model';
import { type IStitch, type IPatternThread } from '@/models/Pattern.model';
import { Position } from '@/class/Position.class';

const project = ref<IGetProject | null>(null);

const stitchPositionLookup = ref<Map<string, IStitch>>(new Map<string, IStitch>());

const pausePosition = ref<Position | null>(null);

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
        project : project as Ref<IGetProject>,
        stitchPositionLookup,
        pausePosition,

        palette,
        stitches,

        setProject(newProject: IGetProject): void {
            project.value = newProject;

            const _stitchPositionLookup = new Map<string, IStitch>();
            for (const thread of project.value.threads) {

                for (const stitch of thread.completedStitches) {
                    _stitchPositionLookup.set(`${stitch[0]}:${stitch[1]}`, {
                        x: stitch[0],
                        y: stitch[1],
                        threadIndex: thread.thread.index,
                        stitchedAt: stitch[2],
                    });
                }

                for (const stitch of thread.stitches) {
                    _stitchPositionLookup.set(`${stitch[0]}:${stitch[1]}`, {
                        x: stitch[0],
                        y: stitch[1],
                        threadIndex: thread.thread.index,
                        stitchedAt: null,
                    });
                }
            }

            stitchPositionLookup.value = _stitchPositionLookup;

            if (project.value.project.pausePositionX !== null && project.value.project.pausePositionY !== null)
                pausePosition.value = Position.at(project.value.project.pausePositionX, project.value.project.pausePositionY);
            else
                pausePosition.value = null;
        },
    };
};