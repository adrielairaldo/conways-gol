import '../styles/controls.css';

interface Props {
    isGameStarted: boolean;
    disabled: boolean;

    rowCount?: number;
    columnCount?: number;
    onGridSizeChange?: (rows: number, columns: number) => void;

    onCreateBoard?: () => void;
    onAdvance?: () => void;
    generation?: number;
    onReset?: () => void;
}

export const Controls: React.FC<Props> = ({ 
    isGameStarted, disabled, rowCount, columnCount, onGridSizeChange, onCreateBoard, onAdvance, generation, onReset 
}) => (
    <div className="controls">
        {!isGameStarted && (
            <>
                <label>
                    Rows:
                    <input type="number" min={1} value={rowCount} disabled={disabled}
                        onChange={(e) => onGridSizeChange?.(Number(e.target.value), columnCount!)}
                    />
                </label>

                <label>
                    Columns:
                    <input type="number" min={1} value={columnCount} disabled={disabled} 
                        onChange={(e) => onGridSizeChange?.(rowCount!, Number(e.target.value))}
                    />
                </label>

                <button onClick={onCreateBoard} disabled={disabled}>
                    Create board
                </button>
            </>
        )}

        {isGameStarted && (
            <>
                <button onClick={onAdvance} disabled={disabled}>
                    Next generation
                </button>

                <span>
                    Generation: {generation}
                </span>

                <button onClick={onReset} disabled={disabled}>
                    Reset
                </button>
            </>
        )}
    </div>
);