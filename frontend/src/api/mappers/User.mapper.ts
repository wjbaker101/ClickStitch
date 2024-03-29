import dayjs from 'dayjs';

import { type IUser } from '@/models/User.model';
import { type IApiUser } from '@/api/api-models/ApiUser.type';

export const userMapper = {

    map(user: IApiUser): IUser {
        return {
            reference: user.reference,
            createdAt: dayjs(user.createdAt),
            email: user.email,
            lastLoginAt: user.lastLoginAt === null ? null : dayjs(user.lastLoginAt),
        };
    },

};