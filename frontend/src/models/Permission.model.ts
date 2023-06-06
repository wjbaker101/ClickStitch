export interface IPermission {
    readonly type: PermissionType;
    readonly name: string;
}

export type PermissionType = 'Unknown' | 'Admin' | 'Creator';