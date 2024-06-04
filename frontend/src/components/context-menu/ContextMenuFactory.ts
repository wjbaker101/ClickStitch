import type { IContextMenuItem, IContextMenuSeparator } from './types/ContextMenuSchema.type';

export const factory = {

    separator: (): IContextMenuSeparator => ({
        type: 'separator',
    }),

    item: (text: string, action: () => void): IContextMenuItem => ({
        type: 'item',
        text,
        action,
    }),

};