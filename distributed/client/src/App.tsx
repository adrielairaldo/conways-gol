import { CellState } from './domain/CellState';
import { Controls } from './ui/component/Controls';
import { createEmptyGrid } from './utils/gridFactory';
import { Grid } from './ui/component/Grid';
import { useBoard } from './hooks/useBoard';
import { useState } from 'react';

const ROW_COUNT = 25;
const COLUMN_COUNT = 25;

export const App: React.FC = () => {
  const { boardState, isLoading, createNewBoard, advance } = useBoard();

  const [draftGrid, setDraftGrid] = useState<CellState[][]>(() => createEmptyGrid(ROW_COUNT, COLUMN_COUNT));

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

  return (
    <div style={{ textAlign: "center", padding: '1rem' }}>
      <h1>Conway's Game of Life</h1>

      {!isGameStarted && (
        <>
          <Grid grid={draftGrid} onCellToggle={toggleDraftCell} />

          <Controls
            isGameStarted={false}
            onCreateBoard={() => createNewBoard(draftGrid)}
            disabled={isLoading}
          />
        </>
      )}

      {isGameStarted && (
        <>
          <Grid grid={boardState.grid} />

          <Controls
            isGameStarted={true}
            onAdvance={() => advance(1)}
            generation={boardState.generation}
            disabled={isLoading}
          />
        </>
      )}
    </div>
  );
};