import { CellState } from './domain/CellState';
import { Controls } from './ui/component/Controls';
import { createEmptyGrid } from './utils/gridFactory';
import { Grid } from './ui/component/Grid';
import { useBoard } from './hooks/useBoard';
import { useEffect, useState } from 'react';
import './ui/styles/tailwind.css';

const DEFAULT_ROW_COUNT = import.meta.env.VITE_DEFAULT_ROW_COUNT;
const DEFAULT_COLUMN_COUNT = import.meta.env.VITE_DEFAULT_COLUMN_COUNT;

export const App: React.FC = () => {
  const { boardState, isLoading, recoverPreviousSessionIfAny, createNewBoard, advance, resetBoard } = useBoard();

  const [rowCount, setRowCount] = useState(DEFAULT_ROW_COUNT);
  const [columnCount, setColumnCount] = useState(DEFAULT_COLUMN_COUNT);

  const [draftGrid, setDraftGrid] = useState<CellState[][]>(() => createEmptyGrid(DEFAULT_ROW_COUNT, DEFAULT_COLUMN_COUNT));

  const handleGridSizeChange = (rows: number, columns: number) => {
    setRowCount(rows);
    setColumnCount(columns);
    setDraftGrid(createEmptyGrid(rows, columns));
  };

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

  const handleReset = () => {
    resetBoard(); // From useBoard hook
    setDraftGrid(createEmptyGrid(rowCount, columnCount)); // Reset draft grid as well (this component state)
  };


  const isGameStarted = boardState !== null;

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