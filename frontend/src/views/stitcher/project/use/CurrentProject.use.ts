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

const stitches = ref<Array<IStitch>>([]);

const backStitches = ref<Array<IBackStitch>>([]);

const percentageCompleted = computed<number>(() => {
    const incomplete = stitches.value.filter(x => x.stitchedAt === null).length +
                       backStitches.value.filter(x => !x.isCompleted).length;

    const complete = stitches.value.filter(x => x.stitchedAt !== null).length +
                     backStitches.value.filter(x => x.isCompleted).length;

    if (complete === 0)
        return 0;

    return complete / (incomplete + complete) * 100;
});

export const useCurrentProject = function () {
    return {
        project : project as Ref<IGetProject>,
        stitchPositionLookup,
        pausePosition,

        palette,
        stitches,
        backStitches,

        percentageCompleted,

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

            const inCompletedStitches = project.value.threads.flatMap(thread => thread.stitches.map<IStitch>(stitch => ({
                x: stitch[0],
                y: stitch[1],
                threadIndex: thread.thread.index,
                stitchedAt: null,
            })));

            const completedStitches = project.value.threads.flatMap(thread => thread.completedStitches.map<IStitch>(stitch => ({
                x: stitch[0],
                y: stitch[1],
                threadIndex: thread.thread.index,
                stitchedAt: stitch[2],
            })));

            stitches.value = inCompletedStitches.concat(completedStitches);

            const inCompletedBackStitches = project.value?.threads.flatMap<IBackStitch>(thread => thread.backStitches.map(x => ({
                threadIndex: thread.thread.index,
                colour: thread.thread.colour,
                startX: x[0],
                startY: x[1],
                endX: x[2],
                endY: x[3],
                isCompleted: false,
            })))

            const completedBackStitches = project.value.threads.flatMap<IBackStitch>(thread => thread.completedBackStitches.map(x => ({
                threadIndex: thread.thread.index,
                colour: thread.thread.colour,
                startX: x[0],
                startY: x[1],
                endX: x[2],
                endY: x[3],
                isCompleted: true,
            })));

            backStitches.value = inCompletedBackStitches.concat(completedBackStitches);
        },
    };
};

interface IBackStitch {
    readonly threadIndex: number;
    readonly colour: string;
    readonly startX: number;
    readonly startY: number;
    readonly endX: number;
    readonly endY: number;
    isCompleted: boolean;
}