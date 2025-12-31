import React from 'react';
//import '../styles/cell.css';

interface Props {
    isAlive: boolean;
    onClick?: () => void;
}

export const Cell: React.FC<Props> = React.memo(({ isAlive, onClick }) => {
    return (
        <div
            // className="cell"
            // style={{
            //     backgroundColor: isAlive ? "#222" : "#fff",
            //     cursor: onClick ? "pointer" : "default"
            // }}
            className={`
                w-5 h-5 border border-gray-700/30 transition-all duration-200
                ${isAlive ? 'bg-cyan-400 shadow-[0_0_8px_rgba(34,211,238,0.6)]' : 'bg-gray-800/50'}
                ${onClick ? 'cursor-pointer hover:border-cyan-500/50' : 'cursor-default'}
            `}
            onClick={onClick}
        />
    );
});