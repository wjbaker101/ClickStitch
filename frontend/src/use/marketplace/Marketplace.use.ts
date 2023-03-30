import { ref } from 'vue';

import { api } from '@/api/api';

import { IPattern } from '@/models/Pattern.model';
import { IBasket } from '@/models/Basket.model';

const basket = ref<IBasket | null>(null);

const patternReferences = ref<Set<string>>(new Set());

const getBasket = async function (): Promise<void> {
    const result = await api.basket.get();
    if (result instanceof Error)
        return;

    basket.value = result;

    patternReferences.value = new Set(basket.value.items.map(x => x.pattern.reference));
};

(async () => {
    await getBasket();
})();

export const useMarketplace = function () {
    return {

        get() {
            return basket;
        },

        patternReferences,

        async addItem(pattern: IPattern): Promise<void> {
            const result = await api.basket.addItem(pattern.reference);
            if (result instanceof Error)
                return;

                await getBasket();
        },

        async removeItem(pattern: IPattern): Promise<void> {
            if (basket.value === null)
                return;

            await api.basket.removeItem(pattern.reference);

            await getBasket();
        },

        async complete(): Promise<void> {
            await api.basket.complete();
        },

    };
};