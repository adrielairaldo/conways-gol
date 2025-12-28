export type Cell = 0 | 1;
export type Grid = ReadonlyArray<ReadonlyArray<Cell>>;

export function createEmptyGrid(rows: number, cols: number): Grid {
    return Array.from({ length: rows }, () =>
        Array.from({ length: cols }, () => 0 as Cell)
    );
}

export function toggleCell(currentGrid: Grid, targetRow: number, targetColumn: number): Grid {
    return currentGrid.map((row, rowIndex) => {
        return row.map((originalCell, columnIndex) => {
            const toggledCell = originalCell === 1 ? 0 : 1;

            return rowIndex === targetRow && columnIndex === targetColumn
                ? toggledCell
                : originalCell;
        });
    });
}
