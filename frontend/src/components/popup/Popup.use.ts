import { useEventBus } from '@vueuse/core';

const bus = useEventBus<IPopupEvent>('popup');

export const usePopup = function () {
    return {

        subscribe(action: (event: IPopupEvent) => void) {
            return bus.on(event => {
                action(event);
            });
        },

        message(message: string): void {
            bus.emit({
                type: 'message',
                message,
            });
        },

        error(error: string): void {
            bus.emit({
                type: 'error',
                message: error,
            });
        },

        success(message: string): void {
            bus.emit({
                type: 'success',
                message,
            });
        },

    };
};

export interface IPopupEvent {
    readonly type: 'message' | 'error' | 'success';
    readonly message: string;
}