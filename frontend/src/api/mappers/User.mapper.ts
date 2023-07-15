import dayjs from 'dayjs';

import { type IUser } from '@/models/User.model';
import { type IApiUser } from '../types/ApiUser.type';

export const userMapper = {

    map(user: IApiUser): IUser {
        return {
            reference: user.reference,
            createdAt: dayjs(user.createdAt),
            email: user.email,
        };
    },

};