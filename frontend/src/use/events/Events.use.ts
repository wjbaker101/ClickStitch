import { onMounted, onUnmounted } from 'vue';

import type { EventNames, IEventsMap } from './types/EventsMap.type';

type EventFunction<TEvent extends IEventsMap[EventNames]> = (eventArgs: TEvent) => void;

const events = new Map<EventNames, Array<EventFunction<any>>>();

export const useEvents = function () {
    return {

        subscribe<TEvent extends IEventsMap[EventNames]>(key: EventNames, func: EventFunction<TEvent>): void {
            if (!events.has(key))
                events.set(key, []);

            events.get(key)?.push(func);
        },

        publish<TEvent extends IEventsMap[EventNames]>(key: EventNames, parameters: TEvent): void {
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

export const useEvent = function <TEvent extends IEventsMap[EventNames]>(key: EventNames, func: EventFunction<TEvent>) {

    onMounted(() => {
        useEvents().subscribe(key, func);
    });

    onUnmounted(() => {
        useEvents().unsubscribe(key, func);
    });

    return {};
};