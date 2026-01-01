import type { CellState } from "../domain/CellState";

/**
 * Creates a new empty grid for Conway's Game of Life.
 * 
 * This function generates a 2D array where all cells are initialized as dead (0).
 * The grid can be used as the initial state for the draft board where users
 * design their starting pattern before the game begins.
 * 
 * @param rows - The number of rows in the grid
 * @param columns - The number of columns in the grid
 * @returns A 2D array of CellState values, where all cells are set to dead (0)
 * 
 * @example
 * // Creates a 10x10 grid with all cells dead
 * const emptyGrid = createEmptyGrid(10, 10);
 */
export function createEmptyGrid(rows: number, columns: number): CellState[][] {
    return Array.from({ length: rows }, () =>
        Array.from({ length: columns }, () => 0 as CellState)
    );
}