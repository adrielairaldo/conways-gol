import type { CellState } from "../domain/CellState";

export function createEmptyGrid(rows: number, columns: number): CellState[][] {
    return Array.from({ length: rows }, () =>
        Array.from({ length: columns }, () => 0 as CellState)
    );
}