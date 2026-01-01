//import '../styles/controls.css';

interface Props {
    isGameStarted: boolean;
    disabled: boolean;

    // Props used when game is NOT started:
    rowCount?: number;
    columnCount?: number;
    onGridSizeChange?: (rows: number, columns: number) => void;

    // Props used when game IS started:
    onCreateBoard?: () => void;
    onAdvance?: () => void;
    generation?: number;
    onReset?: () => void;
}

/**
 * Controls component for Conway's Game of Life.
 * 
 * This component displays different controls based on whether the game has started:
 * - Before game starts: Shows row/column inputs and a "Create Board" button
 * - After game starts: Shows "Next Generation" button, generation counter, and "Reset" button
 * 
 * All controls can be disabled simultaneously (e.g., during API calls) using the disabled prop.
 */
export const Controls: React.FC<Props> = ({
    isGameStarted, disabled, rowCount, columnCount, onGridSizeChange, onCreateBoard, onAdvance, generation, onReset
}) => (
    <div
        //className="controls"
        className="flex flex-wrap items-center justify-center gap-4 mb-8"
    >
        {!isGameStarted && (
            <>
                <label>
                    Rows:
                    <input type="number" min={1} value={rowCount} disabled={disabled}
                        onChange={(e) => onGridSizeChange?.(Number(e.target.value), columnCount!)}
                        className="w-20 px-3 py-2 bg-gray-800 border border-gray-700 rounded-lg text-white 
                            focus:outline-none focus:ring-2 focus:ring-cyan-500 focus:border-transparent
                            disabled:opacity-50 disabled:cursor-not-allowed"
                    />
                </label>

                <label>
                    Columns:
                    <input type="number" min={1} value={columnCount} disabled={disabled}
                        onChange={(e) => onGridSizeChange?.(rowCount!, Number(e.target.value))}
                        className="w-20 px-3 py-2 bg-gray-800 border border-gray-700 rounded-lg text-white 
                            focus:outline-none focus:ring-2 focus:ring-cyan-500 focus:border-transparent
                            disabled:opacity-50 disabled:cursor-not-allowed"
                    />
                </label>

                <button onClick={onCreateBoard} disabled={disabled}
                    className="px-6 py-2 bg-cyan-500 hover:bg-cyan-400 text-gray-900 font-semibold 
                        rounded-lg transition-colors duration-200 disabled:opacity-50 
                        disabled:cursor-not-allowed disabled:hover:bg-cyan-500
                        shadow-lg shadow-cyan-500/20"
                >
                    Create board
                </button>
            </>
        )}

        {isGameStarted && (
            <>
                <button onClick={onAdvance} disabled={disabled}
                    className="px-6 py-2 bg-cyan-500 hover:bg-cyan-400 text-gray-900 font-semibold 
                        rounded-lg transition-colors duration-200 disabled:opacity-50 
                        disabled:cursor-not-allowed disabled:hover:bg-cyan-500
                        shadow-lg shadow-cyan-500/20"
                >
                    Next generation
                </button>

                <div className="px-4 py-2 bg-gray-800 border border-gray-700 rounded-lg">
                    <span className="text-gray-300">
                        Generation: <span className="font-bold text-cyan-400">{generation}</span>
                    </span>
                </div>

                <button onClick={onReset} disabled={disabled}
                    className="px-6 py-2 bg-gray-700 hover:bg-gray-600 text-gray-200 font-semibold 
                        rounded-lg transition-colors duration-200 disabled:opacity-50 
                        disabled:cursor-not-allowed disabled:hover:bg-gray-700"
                >
                    Reset
                </button>
            </>
        )}
    </div>
);