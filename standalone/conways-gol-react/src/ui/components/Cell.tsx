import React from 'react';
import '../styles/cell.css';

type Props = {
  isAlive: boolean;
  onClick: () => void;
};

export const Cell: React.FC<Props> = React.memo(({ isAlive, onClick }) => {
  return (
    <div
      className="cell"
      onClick={onClick}
      style={{
        backgroundColor: isAlive ? "#222" : "#fff"
      }}
    />
  );
});