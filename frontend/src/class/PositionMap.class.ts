export class PositionMap<TValue> {

    private map: Map<number, Map<number, TValue>> = new Map<number, Map<number, TValue>>();

    set(x: number, y: number, value: TValue): void {
        if (!this.map.has(x))
            this.map.set(x, new Map<number, TValue>());

        this.map.get(x)?.set(y, value);
    }

    get (x: number, y: number): TValue | null {
        if (!this.map.has(x))
            return null;

        if (!this.map.get(x)?.has(y))
            return null;

        return this.map.get(x)?.get(y) as TValue;
    }

}