import React from 'react';

interface Props {
    isAlive: boolean;
    onClick?: () => void;
}

/**
 * Represents a single cell in Conway's Game of Life grid.
 * 
 * This component displays a cell that can be either alive (cyan with glow effect)
 * or dead (dark gray). If an onClick handler is provided, the cell becomes
 * clickable and shows hover effects.
 * 
 * The component is memoized to prevent unnecessary re-renders and improve
 * performance when dealing with large grids.
 */
export const Cell: React.FC<Props> = React.memo(({ isAlive, onClick }) => {
    return (
        <div className={`
                w-5 h-5 border border-gray-700/30 transition-all duration-200
                ${isAlive ? 'bg-cyan-400 shadow-[0_0_8px_rgba(34,211,238,0.6)]' : 'bg-gray-800/50'}
                ${onClick ? 'cursor-pointer hover:border-cyan-500/50' : 'cursor-default'}
            `}
            onClick={onClick}
        />
    );
});