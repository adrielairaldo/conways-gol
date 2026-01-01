import { CellState } from './domain/CellState';
import { Controls } from './ui/component/Controls';
import { createEmptyGrid } from './utils/gridFactory';
import { Grid } from './ui/component/Grid';
import { useBoard } from './hooks/useBoard';
import { useEffect, useState } from 'react';
import './ui/styles/tailwind.css';

const DEFAULT_ROW_COUNT = import.meta.env.VITE_DEFAULT_ROW_COUNT;
const DEFAULT_COLUMN_COUNT = import.meta.env.VITE_DEFAULT_COLUMN_COUNT;

/**
 * Main application component for Conway's Game of Life.
 * 
 * This component manages the overall application state and coordinates between
 * the draft grid (where users design their initial pattern) and the active game
 * board (where the simulation runs).
 */
export const App: React.FC = () => {
  const { boardState, isLoading, recoverPreviousSessionIfAny, createNewBoard, advance, resetBoard } = useBoard();

  const [rowCount, setRowCount] = useState(DEFAULT_ROW_COUNT);
  const [columnCount, setColumnCount] = useState(DEFAULT_COLUMN_COUNT);

  const [draftGrid, setDraftGrid] = useState<CellState[][]>(() => createEmptyGrid(DEFAULT_ROW_COUNT, DEFAULT_COLUMN_COUNT));

  /**
   * Updates the grid dimensions and creates a new empty draft grid.
   * 
   * This function is called when users change the row or column count
   * in the input fields before creating a board.
   * 
   * @param rows - The new number of rows for the grid
   * @param columns - The new number of columns for the grid
   */
  const handleGridSizeChange = (rows: number, columns: number) => {
    setRowCount(rows);
    setColumnCount(columns);
    setDraftGrid(createEmptyGrid(rows, columns));
  };

  /**
   * Toggles a cell between alive and dead states in the draft grid.
   * 
   * This function is called when users click on cells before the game starts,
   * allowing them to design their initial pattern.
   * 
   * @param targetRow - The row index of the cell to toggle
   * @param targetColumn - The column index of the cell to toggle
   */
  const toggleDraftCell = (targetRow: number, targetColumn: number) => {
    setDraftGrid(currentGrid =>
      currentGrid.map((row, rowIndex) =>
        row.map((cell, columnIndex) =>
          rowIndex === targetRow && columnIndex === targetColumn
            ? cell === CellState.Alive ? CellState.Dead : CellState.Alive
            : cell
        )
      )
    );
  };

  /**
   * Resets the board and clears the draft grid.
   * 
   * This function is called when users click the Reset button. It clears
   * the active board and creates a fresh empty draft grid for designing
   * a new pattern.
   */
  const handleReset = () => {
    resetBoard(); // From useBoard hook
    setDraftGrid(createEmptyGrid(rowCount, columnCount)); // Reset draft grid as well (this component state)
  };

  const isGameStarted = boardState !== null;

  /**
   * Attempts to recover a previous session when the app loads.
   * This runs once when the component mounts.
   */
  useEffect(() => {
    recoverPreviousSessionIfAny();
  }, [recoverPreviousSessionIfAny]);

  return (
    <div
      className="min-h-screen bg-gradient-to-b from-gray-950 to-gray-900 text-white py-8 px-4"
      //style={{ textAlign: "center", padding: '1rem' }}
    >
      <div className="max-w-7xl mx-auto">
        <header className="text-center mb-12">
          <h1 className="text-5xl font-bold mb-3 bg-gradient-to-r from-cyan-400 to-blue-500 bg-clip-text text-transparent">
            Conway's Game of Life
          </h1>
          <p className="text-gray-400 text-lg">
            {isGameStarted
              ? "Click 'Next Generation' to evolve your cells"
              : "Click cells to toggle them, then create your board"}
          </p>
        </header>

        {!isGameStarted && (
          <>
            <Controls
              isGameStarted={false}
              rowCount={rowCount}
              columnCount={columnCount}
              onGridSizeChange={handleGridSizeChange}
              onCreateBoard={() => createNewBoard(draftGrid)}
              disabled={isLoading}
            />

            <Grid disabled={isLoading} grid={draftGrid} onCellToggle={toggleDraftCell} />
          </>
        )}

        {isGameStarted && (
          <>
            <Controls
              isGameStarted={true}
              onAdvance={() => advance(1)}
              generation={boardState.generation}
              onReset={handleReset}
              disabled={isLoading}
            />

            <Grid disabled={isLoading} grid={boardState.grid} />
          </>
        )}
      </div>
    </div>
  );
};