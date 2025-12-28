import React, { useEffect, useReducer } from 'react';
import { createEmptyGrid } from './domain/grid';
import { GameControls } from './ui/components/GameControls';
import { gameReducer } from './state/gameReducer';
import { GridComponent } from './ui/components/Grid';

const ROW_COUNT = 30;
const COLUMN_COUNT = 30;
const TICK_INTERVAL_MS = 300;

export const App: React.FC = () => {
  const [state, dispatch] = useReducer(gameReducer, {
    grid: createEmptyGrid(ROW_COUNT, COLUMN_COUNT),
    isRunning: false,
    generation: 0,
  });

  useEffect(() => {
    if (!state.isRunning) return;

    const intervalId = setInterval(() => {
      dispatch({ type: "TICK" });
    }, TICK_INTERVAL_MS);

    return () => clearInterval(intervalId);
  }, [state.isRunning]);

  const handleStart = () => dispatch({ type: "START" });
  const handleStop = () => dispatch({ type: "STOP" });
  const handleReset = () =>
    dispatch({
      type: "RESET",
      grid: createEmptyGrid(ROW_COUNT, COLUMN_COUNT),
    });

  const handleToggleCell = (rowIndex: number, columnIndex: number) =>
    dispatch({
      type: "TOGGLE_CELL",
      targetRow: rowIndex,
      targetColumn: columnIndex,
    });

  return (
    <div style={{ textAlign: "center", padding: 16 }}>
      <h1>Conway's Game of Life</h1>

      <GameControls
        isRunning={state.isRunning}
        generation={state.generation}
        onStart={handleStart}
        onStop={handleStop}
        onReset={handleReset}
      />

      <GridComponent grid={state.grid} onToggle={handleToggleCell} />
    </div>
  );
};
