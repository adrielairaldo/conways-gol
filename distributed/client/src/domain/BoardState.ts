import { CellState } from "./CellState";

export interface BoardState {
  grid: CellState[][];
  generation: number;
}