export interface ICreatePatternRequest {
    readonly title: string;
    readonly price: number;
    readonly aidaCount: number;
    readonly externalShopUrl: string | null;
}

export interface ICreatePatternResponse {
}