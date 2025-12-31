import { CellState } from './domain/CellState';
import { Controls } from './ui/component/Controls';
import { createEmptyGrid } from './utils/gridFactory';
import { Grid } from './ui/component/Grid';
import { useBoard } from './hooks/useBoard';
import { useEffect, useState } from 'react';

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

  const isGameStarted = boardState !== null;

  useEffect(() => {
    recoverPreviousSessionIfAny();
  }, [recoverPreviousSessionIfAny]);

  return (
    <div style={{ textAlign: "center", padding: '1rem' }}>
      <h1>Conway's Game of Life</h1>

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

          <Grid grid={draftGrid} onCellToggle={toggleDraftCell} />
        </>
      )}

      {isGameStarted && (
        <>
          <Controls
            isGameStarted={true}
            onAdvance={() => advance(1)}
            generation={boardState.generation}
            onReset={resetBoard}
            disabled={isLoading}
          />

          <Grid grid={boardState.grid} />
        </>
      )}
    </div>
  );
};