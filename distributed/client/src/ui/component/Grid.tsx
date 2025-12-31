import { Cell } from './Cell';
import { CellState } from '../../domain/CellState';
import '../styles/grid.css';

interface GridProps {
    grid: CellState[][];
    onCellToggle?: (targetRow: number, targetColumn: number) => void;
}

export const Grid: React.FC<GridProps> = ({ grid, onCellToggle }) => {
    return (
        <div
            className="grid"
            style={{
                gridTemplateColumns: `repeat(${grid[0].length}, 20px)`,
            }}
        >
            {grid.map((row, rowIndex) =>
                row.map((cell, columnIndex) => (
                    <Cell key={`${rowIndex}-${columnIndex}`}
                        isAlive={cell === CellState.Alive}
                        onClick={onCellToggle ? () => onCellToggle(rowIndex, columnIndex) : undefined}
                    />))
            )}
        </div>
    );
}