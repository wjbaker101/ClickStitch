import { apiClient } from '@/api/client';

import type { IUpdateInventoryThreadRequest, IUpdateInventoryThreadResponse } from '@/api/parts/inventory/types/UpdateInventoryThread.type';
import type { ISearchInventoryThreadsResponse } from '@/api/parts/inventory/types/SearchInventoryThreads.type';

export const inventoryApi = {

    async searchThreads(searchTerm: string, brand: string | null): Promise<ISearchInventoryThreadsResponse | Error> {
        const queryParams = new Map<string, string>();
        if (searchTerm.length > 0)
            queryParams.set('search_term', searchTerm);
        if (brand !== null)
            queryParams.set('brand', brand);

        const response = await apiClient.get<ISearchInventoryThreadsResponse>({
            url: '/inventory/threads/search',
            queryParams,
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;

        return response;
    },

    async updateThread(threadReference: string, request: IUpdateInventoryThreadRequest): Promise<void | Error> {
        const response = await apiClient.put<IUpdateInventoryThreadResponse>({
            url: `/inventory/threads/${threadReference}`,
            body: request,
            auth: {
                required: true,
                use: true,
            },
        });

        if (response instanceof Error)
            return response;
    },

};