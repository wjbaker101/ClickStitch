import dayjs from 'dayjs';

import { type ICreator } from '@/models/Creator.model';
import { type IApiCreator } from '../types/ApiCreator.type';

export const creatorMapper = {

    map(creator: IApiCreator): ICreator {
        return {
            reference: creator.reference,
            createdAt: dayjs(creator.createdAt),
            name: creator.name,
            storeUrl: creator.storeUrl,
        };
    },

};