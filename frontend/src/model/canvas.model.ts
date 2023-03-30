import { IStitch } from '@/model/stitch.model';

export interface ICanvas {
    width: number;
    height: number;
    stitches: Array<IStitch>;
}