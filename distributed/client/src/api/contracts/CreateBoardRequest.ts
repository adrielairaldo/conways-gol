import type { CellState } from "../../domain/CellState";

export interface CreateBoardRequest {
    initialGrid: CellState[][];
}