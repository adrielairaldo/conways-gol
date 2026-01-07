import { CellState } from './domain/CellState';
import { Controls } from './ui/component/Controls';
import { createEmptyGrid } from './utils/gridFactory';
import { Grid } from './ui/component/Grid';
import { Toast } from './ui/component/Toast';
import { useBoard } from './hooks/useBoard';
import { useCallback, useEffect, useState } from 'react';
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

  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  /**
   * Updates the grid dimensions and creates a new empty draft grid.
   * 
   * This function is called when users change the row or column count
   * in the input fields before creating a board.
   * 
   * Memoized to prevent unnecessary re-renders of child components.
   * 
   * @param rows - The new number of rows for the grid
   * @param columns - The new number of columns for the grid
   */
  const handleGridSizeChange = useCallback((rows: number, columns: number) => {
    setRowCount(rows);
    setColumnCount(columns);
    setDraftGrid(createEmptyGrid(rows, columns));
  }, []);

  /**
   * Toggles a cell between alive and dead states in the draft grid.
   * 
   * This function is called when users click on cells before the game starts,
   * allowing them to design their initial pattern.
   * 
   * Memoized to prevent unnecessary re-renders of child components.
   * 
   * @param targetRow - The row index of the cell to toggle
   * @param targetColumn - The column index of the cell to toggle
   */
  const toggleDraftCell = useCallback((targetRow: number, targetColumn: number) => {
    setDraftGrid(currentGrid =>
      currentGrid.map((row, rowIndex) =>
        row.map((cell, columnIndex) =>
          rowIndex === targetRow && columnIndex === targetColumn
            ? cell === CellState.Alive ? CellState.Dead : CellState.Alive
            : cell
        )
      )
    );
  }, []);

  /**
   * Displays an error message to the user via a toast notification.
   * 
   * Memoized to prevent unnecessary re-renders of child components.
   * 
   * @param message - The error message to display
   */
  const showError = useCallback((message: string) => {
    setErrorMessage(message);
  }, []);

  /**
   * Closes the error toast notification.
   * 
   * Memoized to prevent unnecessary re-renders of child components.
   */
  const closeError = useCallback(() => {
    setErrorMessage(null);
  }, []);

  /**
   * Resets the board and clears the draft grid.
   * 
   * This function is called when users click the Reset button. It clears
   * the active board and creates a fresh empty draft grid for designing
   * a new pattern.
   * 
   * Memoized to prevent unnecessary re-renders of child components.
   */
  const handleReset = useCallback(() => {
    resetBoard(); // From useBoard hook
    setDraftGrid(createEmptyGrid(rowCount, columnCount)); // Reset draft grid as well (this component state)
  }, [resetBoard, rowCount, columnCount]);

  /**
   * Creates a new board with the current draft grid.
   * 
   * Memoized to prevent unnecessary re-renders of child components.
   */
  const handleCreateBoard = useCallback(() => {
    createNewBoard(draftGrid, showError);
  }, [createNewBoard, draftGrid, showError]);

  /**
   * Advances the game by one generation.
   * 
   * Memoized to prevent unnecessary re-renders of child components.
   */
  const handleAdvance = useCallback(() => {
    advance(1, showError);
  }, [advance, showError]);

  const isGameStarted = boardState !== null;

  /**
   * Attempts to recover a previous session when the app loads.
   * This runs once when the component mounts.
   */
  useEffect(() => {
    recoverPreviousSessionIfAny(showError);
  }, [recoverPreviousSessionIfAny, showError]);

  return (
    <div className="min-h-screen bg-gradient-to-b from-gray-950 to-gray-900 text-white py-8 px-4">
      {/* Toast Notification */}
      {errorMessage && (
        <Toast message={errorMessage} onClose={closeError} />
      )}

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
              onCreateBoard={handleCreateBoard}
              disabled={isLoading}
            />

            <Grid disabled={isLoading} grid={draftGrid} onCellToggle={toggleDraftCell} />
          </>
        )}

        {isGameStarted && (
          <>
            <Controls
              isGameStarted={true}
              onAdvance={handleAdvance}
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