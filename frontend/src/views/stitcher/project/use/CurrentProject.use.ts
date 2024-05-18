import { computed, readonly, ref, type Ref, shallowReadonly } from 'vue';

import { type IGetProject } from '@/models/GetProject.model';
import { type IStitch, type IPatternThread } from '@/models/Pattern.model';
import { Position } from '@/class/Position.class';
import { PositionMap } from '@/class/PositionMap.class';

const project = ref<IGetProject | null>(null);

const palette = ref(new Map<number, IPatternThread>());
const stitches = ref<Array<IStitch>>([]);
const backStitches = ref<Array<IBackStitch>>([]);
const stitchPositionLookup = ref(new PositionMap<IStitch>());
const pausePosition = ref<Position | null>(null);

const activeStitch = ref<IActiveStitch | null>(null);
const jumpedBackStitch = ref<IJumpedBackStitch | null>(null);

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

        palette: readonly(palette),
        stitches: shallowReadonly(stitches),
        backStitches: shallowReadonly(backStitches),
        stitchPositionLookup: shallowReadonly(stitchPositionLookup),
        pausePosition,

        activeStitch: readonly(activeStitch),
        jumpedBackStitch: readonly(jumpedBackStitch),

        percentageCompleted,

        setProject(newProject: IGetProject): void {
            project.value = newProject;

            const _palette = new Map<number, IPatternThread>();
            for (const thread of project.value.threads) {
                _palette.set(thread.thread.index, thread.thread);
            }
            palette.value = _palette;

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

            const inCompletedBackStitches = project.value.threads.flatMap<IBackStitch>(thread => thread.backStitches.map(x => ({
                threadIndex: thread.thread.index,
                colour: thread.thread.colour,
                startX: x[0],
                startY: x[1],
                endX: x[2],
                endY: x[3],
                isCompleted: false,
            })));

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

            const _stitchPositionLookup = new PositionMap<IStitch>();
            for (const stitch of stitches.value) {
                _stitchPositionLookup.set(stitch.x, stitch.y, stitch);
            }
            stitchPositionLookup.value = _stitchPositionLookup;

            if (project.value.project.pausePositionX !== null && project.value.project.pausePositionY !== null)
                pausePosition.value = Position.at(project.value.project.pausePositionX, project.value.project.pausePositionY);
            else
                pausePosition.value = null;
        },

        setActiveStitch(stitch: IActiveStitch | null): void {
            activeStitch.value = stitch;
        },

        setJumpedBackStitch(backStitch: IJumpedBackStitch | null): void {
            jumpedBackStitch.value = backStitch;
        },
    };
};

export interface IBackStitch {
    readonly threadIndex: number;
    readonly colour: string;
    readonly startX: number;
    readonly startY: number;
    readonly endX: number;
    readonly endY: number;
    isCompleted: boolean;
}

export interface IActiveStitch {
    readonly x: number;
    readonly y: number;
    readonly endX?: number;
    readonly endY?: number;
    readonly threadIndex: number;
}

export interface IJumpedBackStitch {
    readonly startX: number;
    readonly startY: number;
    readonly endX: number;
    readonly endY: number;
}