declare module '@vue/runtime-core' {
    export interface GlobalComponents {
        CardComponent: typeof import('@/components/Card.component.vue').default;
        LinkComponent: typeof import('@/components/Link.component.vue').default;
        ViewComponent: typeof import('@/components/View.component.vue').default;
    }
}

export {};