const keysDown = new Set<string>();

export const useInput = function () {
    return {
        keysDown,
    };
};