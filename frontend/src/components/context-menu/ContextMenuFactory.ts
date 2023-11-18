import type { IconName } from '@wjb/vue/component/IconComponent.vue';
import type { IContextMenuItem, IContextMenuSeparator } from './types/ContextMenuSchema.type';

export const factory = {

    separator: (): IContextMenuSeparator => ({
        type: 'separator',
    }),

    item: (text: string, action: () => void, icon?: IconName): IContextMenuItem => ({
        type: 'item',
        text,
        icon,
        action,
    }),

};