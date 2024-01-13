import { type IPagination } from '@/models/Pagination.model';
import { type IApiPagination } from '@/api/api-models/ApiPagination.type';

export const paginationMapper = {

    map(pagination: IApiPagination): IPagination {
        return {
            pageNumber: pagination.pageNumber,
            pageSize: pagination.pageSize,
            pageCount: pagination.pageCount,
            totalCount: pagination.totalCount,
        };
    },

};