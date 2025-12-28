import type { Grid } from "./grid";

const neighborDirections = [
    [-1, -1], [-1, 0], [-1, 1],
    [ 0, -1],          [ 0, 1],
    [ 1, -1], [ 1, 0], [ 1, 1],
];

function countAliveNeighbors(grid: Grid, targetRow: number, targetColumn: number): number {
    const totalRows = grid.length;
    const totalColumns = grid[0].length;

    return neighborDirections.reduce((aliveCount, [rowOffset, columnOffset]) => {
        const neighborRow = targetRow + rowOffset;
        const neighborColumn = targetColumn + columnOffset;

        const isInsideGrid =
            neighborRow >= 0 &&
            neighborRow < totalRows &&
            neighborColumn >= 0 &&
            neighborColumn < totalColumns;

        return isInsideGrid
            ? aliveCount + grid[neighborRow][neighborColumn]
            : aliveCount;
    }, 0);
}

export function nextGeneration(currentGrid: Grid): Grid {
    return currentGrid.map((row, rowIndex) =>
        row.map((cellValue, columnIndex) => {
            const aliveNeighborsCount = countAliveNeighbors(
                currentGrid,
                rowIndex,
                columnIndex
            );

            const isAlive = cellValue === 1;
            const survives = isAlive && (aliveNeighborsCount === 2 || aliveNeighborsCount === 3);
            const becomesAlive = !isAlive && aliveNeighborsCount === 3;

            return survives || becomesAlive ? 1 : 0;
        })
    );
}