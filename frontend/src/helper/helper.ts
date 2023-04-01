export const copy = function <T>(object: T): T {
    return Object.assign({}, object);
};

export const currency = function (price: number): string {
    return new Intl.NumberFormat('en-GB', {
        style: 'currency',
        currency: 'gbp',
      }).format(price);
};

export const setTitle = function (title: string): void {
    document.title = `ClickStitch | ${title}`;
};

export const isDark = function (colour: string): boolean {
    const red = Number('0x' + colour[1] + colour[2]);
    const green = Number('0x' + colour[3] + colour[4]);
    const blue = Number('0x' + colour[5] + colour[6]);

    const lightness = 0.2126 * red + 0.7152 * green + 0.0722 * blue; // ITU-R BT.709

    return lightness < 90;
};