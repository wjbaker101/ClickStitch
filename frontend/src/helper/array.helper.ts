export const sum = function <T>(numbers: Array<T>, func: (t: T) => number): number {
    return numbers.reduce((total, x) => total + func(x), 0);
};