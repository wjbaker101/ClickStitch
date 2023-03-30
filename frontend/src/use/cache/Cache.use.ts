interface ICacheItem<T> {
    readonly data: T;
    readonly expiresAt: number;
}

export const useCache = function () {
    return {

        get<T>(key: string): T |  null {
            const item = localStorage.getItem(key);
            if (item === null)
                return null;

            const json = JSON.parse(item) as ICacheItem<T>;
            if (json.expiresAt !== 0 && json.expiresAt < Date.now())
                return null;

            return json.data;
        },

        set<T>(key: string, data: T, expiresAt: number = 0): void {
            const item: ICacheItem<T> = {
                data,
                expiresAt,
            };

            localStorage.setItem(key, JSON.stringify(item));
        },

        delete(key: string): void {
            localStorage.removeItem(key);
        },
    };
};