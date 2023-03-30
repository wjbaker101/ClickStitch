import { ICanvas } from '@/model/canvas.model';
import { IPalette } from '@/model/palette.model';

export interface IProject {
    title: string;
    palette: IPalette;
    canvas: ICanvas;
}