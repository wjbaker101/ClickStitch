import { ref } from 'vue';
import { Dayjs } from 'dayjs';

import { useCache } from '@/use/cache/Cache.use';

import { type IPermission } from '@/models/Permission.model';

const cache = useCache();

const cacheKey = 'auth';
const maxTime = 1000 * 60 * 60 * 24 * 6;

export interface IAuth {
    readonly email: string;
    readonly loginToken: string;
    readonly permissions: Array<IPermission>;
    readonly loggedInAt: Dayjs;
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