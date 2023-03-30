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