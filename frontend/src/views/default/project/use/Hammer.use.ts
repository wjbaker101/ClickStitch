import { Ref } from 'vue';
import Hammer from 'hammerjs';

export const useHammer = function (element: Ref<HTMLElement>) {

    const hammer = new Hammer.Manager(element.value);
    hammer.add(new Hammer.Tap({
        event: 'double-tap',
        taps: 2,
    }));
    hammer.add(new Hammer.Pan({
        event: 'pan',
        threshold: 2,
    }));

    return {
        on(event: 'tap' | 'double-tap' | 'pan', action: (input: HammerInput) => void): void {
            hammer.on(event, (input) => {
                action(input);
            });
        },
    };
};