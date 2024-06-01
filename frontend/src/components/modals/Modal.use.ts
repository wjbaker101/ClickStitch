import { useEventBus } from '@vueuse/core';

const showBus = useEventBus<IShowModalEvent>('modal_show');
const hideBus = useEventBus<{}>('modal_hide');

export const useModal = function () {
    return {

        subscribeOnShow(action: (event: IShowModalEvent) => void) {
            return showBus.on(event => {
                action(event);
            });
        },

        subscribeOnHide(action: () => void) {
            return hideBus.on(event => {
                action();
            });
        },

        show(event: IShowModalEvent): void {
            showBus.emit(event);
        },

        hide(): void {
            hideBus.emit();
        },

    };
};

export interface IShowModalEvent {
    readonly component: InstanceType<any>;
    readonly componentProps: any;
}