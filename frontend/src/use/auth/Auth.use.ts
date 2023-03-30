import { ref } from 'vue';

import { useCache } from '@/use/cache/Cache.use';

const cache = useCache();

const cacheKey = 'auth';
const maxTime = 1000 * 60 * 60 * 24 * 6;

export interface IAuth {
    readonly username: string;
    readonly loginToken: string;
}

const auth = ref<IAuth | null>(cache.get(cacheKey));

export const useAuth = function () {
    return {

        details: auth,

        set(newAuth: IAuth): void {
            auth.value = newAuth;
            cache.set(cacheKey, auth.value, Date.now() + maxTime);
        },

        clear() {
            auth.value = null;
            cache.delete(cacheKey);
        },

    };
};