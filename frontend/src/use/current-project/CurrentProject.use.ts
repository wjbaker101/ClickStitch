import { ref } from 'vue';

import data from '@/data/templar-knight.json';

import { IProject } from '@/model/project.model';
import { IThread } from '@/model/thread.model';
import { IStitch } from '@/model/stitch.model';

const threads = new Map<number, IThread>();

for (const thread of data.palette.threads) {
    threads.set(thread.index, {
        index: thread.index,
        name: thread.name,
        description: thread.description,
        colour: `#${thread.colour.toLowerCase()}`,
    });
}

const filler: IStitch = {
    x: 0,
    y: 0,
    threadIndex: 0,
    isDone: false,
};

const stitches = Array<number>(data.canvas.width * data.canvas.height).fill(0).map(_ => Object.assign({}, filler));

for (const stitch of data.canvas.stitches) {
    stitches[stitch.x + data.canvas.width * stitch.y] = {
        x: stitch.y,
        y: stitch.x,
        threadIndex: stitch.index,
        isDone: false,
    };
}

const project = ref<IProject>({
    title: 'New Project',
    palette: {
        threads,
    },
    canvas: {
        width: data.canvas.width,
        height: data.canvas.height,
        stitches,
    },
});

export const useCurrentProject = function () {
    return {
        project,
    };
};