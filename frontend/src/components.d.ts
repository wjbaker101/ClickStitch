declare module '@vue/runtime-core' {
    export interface GlobalComponents {
        ButtonComponent: typeof import('@wjb/vue/components/ButtonComponent.vue').default;
        CardComponent: typeof import('@/components/Card.component.vue').default;
        LinkComponent: typeof import('@/components/Link.component.vue').default;
        ViewComponent: typeof import('@/components/View.component.vue').default;
    }
}

export {};