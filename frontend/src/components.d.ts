declare module '@vue/runtime-core' {
    export interface GlobalComponents {
        ButtonComponent: typeof import('@wjb/vue/components/ButtonComponent.vue').default;
        CardComponent: typeof import('@/components/CardComponent.vue').default;
        LinkComponent: typeof import('@/components/LinkComponent.vue').default;
        ViewComponent: typeof import('@/components/ViewComponent.vue').default;
    }
}

export {};