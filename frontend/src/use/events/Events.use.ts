type EventFunction<TEventParameters> = (eventArgs: TEventParameters) => void;

const events = new Map<string, Array<EventFunction<any>>>();

export const useEvents = function () {
    return {

        subscribe<TEventParameters>(key: string, func: EventFunction<TEventParameters>): void {
            if (!events.has(key))
                events.set(key, []);

            events.get(key)?.push(func);
        },

        publish<TEventParameters>(key: string, parameters: TEventParameters): void {
            events.get(key)?.forEach(x => x(parameters));
        },

        unsubscribe(key: string, func: EventFunction<any>): void {
            const funcs = events.get(key);
            if (!funcs)
                return;

            events.set(key, funcs.filter(x => x !== func));
        },

    };
};