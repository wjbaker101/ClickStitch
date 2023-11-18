import type { IconName } from '@wjb/vue/component/IconComponent.vue';

export interface IContextMenuSchema {
    readonly header: string;
    readonly items: Array<IContextMenuItem | IContextMenuSeparator>;
}

export interface IContextMenuItem {
    readonly type: 'item';
    readonly icon?: IconName;
    readonly text: string;
    readonly action: () => void;
}

export interface IContextMenuSeparator {
    readonly type: 'separator';
}