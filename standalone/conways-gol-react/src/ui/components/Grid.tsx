import React from 'react';
import { Cell } from './Cell';
import type { Grid } from '../../domain/grid';
import '../styles/grid.css';

type Props = {
  grid: Grid;
  onToggle: (rowIndex: number, columnIndex: number) => void;
};

export const GridComponent: React.FC<Props> = ({ grid, onToggle }) => {
  const totalColumns = grid[0]?.length ?? 0;

  return (
    <div
      className="grid"
      style={{
        gridTemplateColumns: `repeat(${totalColumns}, 20px)`,
      }}
    >
      {grid.map((row, rowIndex) =>
        row.map((cell, columnIndex) => (
          <Cell
            key={`${rowIndex}-${columnIndex}`}
            isAlive={cell === 1}
            onClick={() => onToggle(rowIndex, columnIndex)}
          />
        ))
      )}
    </div>
  );
};