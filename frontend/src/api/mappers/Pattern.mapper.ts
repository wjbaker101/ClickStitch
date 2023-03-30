import dayjs from 'dayjs';

import { IPattern } from '@/models/Pattern.model';
import { IApiPattern } from '../types/ApiPattern.type';

export const patternMapper = {

    map(pattern: IApiPattern): IPattern {
        return {
            reference: pattern.reference,
            createdAt: dayjs(pattern.createdAt),
            title: pattern.title,
            width: pattern.width,
            height: pattern.height,
            price: pattern.price,
        };
    },

};