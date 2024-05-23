export const calculateRequiredSkeins = function (stitches: number, aidaCount: number): number {
    // https://www.mismatch.co.uk/cross.htm#floss_amt
    const strands = 2;
    const estimatedStitches = 17 * (15 / (6 / aidaCount)) * (6 / strands);

    return Math.ceil(stitches / estimatedStitches);
};