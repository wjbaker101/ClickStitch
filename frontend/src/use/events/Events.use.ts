import type { EventNames } from './types/EventsMap.type';

type EventFunction<TEventParameters> = (eventArgs: TEventParameters) => void;

const events = new Map<EventNames, Array<EventFunction<any>>>();

export const useEvents = function () {
    return {

        subscribe<TEventParameters>(key: EventNames, func: EventFunction<TEventParameters>): void {
            if (!events.has(key))
                events.set(key, []);

            events.get(key)?.push(func);
        },

        publish<TEventParameters>(key: EventNames, parameters: TEventParameters): void {
            events.get(key)?.forEach(x => x(parameters));
        },

        unsubscribe(key: EventNames, func: EventFunction<any>): void {
            const funcs = events.get(key);
            if (!funcs)
                return;

            events.set(key, funcs.filter(x => x !== func));
        },

    };
};