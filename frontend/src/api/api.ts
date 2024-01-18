import { adminApi } from '@/api/parts/admin/admin.api';
import { authApi } from '@/api/parts/auth/auth.api';
import { creatorsApi } from '@/api/parts/creators/creators.api';
import { patternsApi } from '@/api/parts/patterns/patterns.api';
import { projectsApi } from '@/api/parts/projects/projects.api';
import { usersApi } from '@/api/parts/users/users.api';
import { inventoryApi } from './parts/inventory/inventory.api';

export const api = {

    admin: adminApi,
    auth: authApi,
    creators: creatorsApi,
    patterns: patternsApi,
    projects: projectsApi,
    users: usersApi,
    inventory: inventoryApi,

};