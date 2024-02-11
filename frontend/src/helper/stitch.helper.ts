export interface IFabricSize {
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

export const calculateRequiredSkeins = function (stitches: number, aidaCount: number): number {
    // https://www.mismatch.co.uk/cross.htm#floss_amt
    const strands = 2;
    const estimatedStitches = 17 * (15 / (6 / aidaCount)) * (6 / strands);

    return Math.ceil(stitches / estimatedStitches);
};