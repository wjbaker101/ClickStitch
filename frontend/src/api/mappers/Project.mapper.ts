import dayjs from 'dayjs';

import { patternMapper } from './Pattern.mapper';

import { IProject } from '@/models/Project.model';
import { IApiProject } from '../types/ApiProject.type';

export const projectMapper = {

    map(project: IApiProject): IProject {
        return {
            pattern: patternMapper.map(project.pattern),
            purchasedAt: dayjs(project.purchasedAt),
        };
    },

};