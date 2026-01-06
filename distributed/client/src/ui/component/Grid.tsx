import { Cell } from './Cell';
import { CellState } from '../../domain/CellState';
//import '../styles/grid.css';

interface GridProps {
    disabled: boolean;
    grid: CellState[][];
    onCellToggle?: (targetRow: number, targetColumn: number) => void;
}

/**
 * Grid component that displays the Conway's Game of Life board.
 * 
 * This component renders a 2D grid of Cell components. When disabled (e.g., during
 * API calls), it displays a loading overlay with a spinner and prevents all user
 * interaction with the cells.
 * 
 * The grid size is dynamic and automatically adjusts based on the number of
 * columns in the provided grid data.
 */
export const Grid: React.FC<GridProps> = ({ disabled, grid, onCellToggle }) => {
    return (
        <div className="flex justify-center items-start">
            <div className="relative">
                <div className="inline-grid gap-0 p-4 bg-gray-900 rounded-xl border border-gray-800 shadow-2xl"
                    style={{
                        gridTemplateColumns: `repeat(${grid[0].length}, 20px)`,
                    }}
                >
                    {grid.map((row, rowIndex) =>
                        row.map((cell, columnIndex) => (
                            <Cell
                                key={`${rowIndex}-${columnIndex}`}
                                isAlive={cell === CellState.Alive}
                                onClick={onCellToggle && !disabled ? () => onCellToggle(rowIndex, columnIndex) : undefined}
                            />))
                    )}
                </div>

                {/* Loading Overlay */}
                {disabled && (
                    <div className="absolute inset-0 flex items-center justify-center bg-gray-950/60 
                        rounded-xl backdrop-blur-[1px]">
                        <div className="flex flex-col items-center gap-3">
                            {/* Spinner */}
                            <div className="relative w-12 h-12">
                                <div className="absolute inset-0 border-4 border-cyan-500/20 rounded-full"></div>
                                <div className="absolute inset-0 border-4 border-transparent border-t-cyan-400 
                                    rounded-full animate-spin"></div>
                            </div>
                            {/* Loading Text */}
                            <span className="text-cyan-400 font-semibold text-sm tracking-wide">
                                Processing...
                            </span>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}