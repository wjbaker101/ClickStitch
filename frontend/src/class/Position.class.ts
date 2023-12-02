export class Position {

    public static ZERO = Position.at(0, 0);

    constructor(public readonly x: number, public readonly y: number) {
    }

    public static at(x: number, y: number): Position {
        return new Position(x, y);
    }

    public static copy(position: Position): Position {
        return Position.at(position.x, position.y);
    }

    public translate(x: number, y: number): Position {
        return Position.at(this.x + x, this.y + y);
    }

    public subtract(pos: Position): Position {
        return this.translate(-pos.x, -pos.y);
    }

    public scale(x: number, y: number): Position {
        return Position.at(this.x * x, this.y * y);
    }

    public min(x: number, y: number): Position {
        return Position.at(Math.max(x, this.x), Math.max(y, this.y));
    }

    public max(x: number, y: number): Position {
        return Position.at(Math.min(x, this.x), Math.min(y, this.y));
    }

    public floor(): Position {
        return Position.at(Math.floor(this.x), Math.floor(this.y));
    }

    public equals(pos: Position | null): boolean {
        if (pos === null)
            return false;

        return this.x === pos.x && this.y === pos.y;
    }
}