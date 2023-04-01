type AidaCount = number;
type NumberOfThreads = number;
type NumberOfStitches = number;

const skeinTable: Record<AidaCount, Record<NumberOfThreads, NumberOfStitches>> = {
    14: {
        2: 1785,
    },
    16: {
        2: 2040,
    },
};

export const calculateSkeins = function (aidaCount: number, stitchCount: number, threadCount: number = 2): number {
    const stitchesPerSkein = skeinTable[aidaCount][threadCount] as number;

    return Math.ceil(stitchCount / stitchesPerSkein);
};

interface IFabricSize {
    in: ISize;
    cm: ISize;
}

interface ISize {
    readonly width: number;
    readonly height: number;
}

export const calculateFabricSize = function (width: number, height: number, aidaCount: number): IFabricSize {
    const padding = 2;

    const widthInches = width / aidaCount + padding * 2;
    const heightInches = height / aidaCount + padding * 2;

    return {
        in: {
            width: Math.ceil(widthInches),
            height: Math.ceil(heightInches),
        },
        cm: {
            width: Math.ceil(widthInches * 2.54),
            height: Math.ceil(heightInches * 2.54),
        },
    };
};