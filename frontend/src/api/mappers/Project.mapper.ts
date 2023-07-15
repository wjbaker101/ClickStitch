import dayjs from 'dayjs';

import { patternMapper } from '@/api/mappers/Pattern.mapper';

import { type IProject } from '@/models/Project.model';
import { type IApiProject } from '@/api/types/ApiProject.type';

export const projectMapper = {

    map(project: IApiProject): IProject {
        return {
            pattern: patternMapper.map(project.pattern),
            purchasedAt: dayjs(project.purchasedAt),
        };
    },

};