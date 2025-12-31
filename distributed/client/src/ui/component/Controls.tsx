import '../styles/controls.css';

interface Props {
    isGameStarted: boolean;
    onCreateBoard?: () => void;
    onAdvance?: () => void;
    generation?: number;
    onReset?: () => void;
    disabled: boolean;
}

export const Controls: React.FC<Props> = ({ isGameStarted, onCreateBoard, onAdvance, generation, onReset, disabled }) => (
    <div className="controls">
        {!isGameStarted && (
            <button onClick={onCreateBoard} disabled={disabled}>
                Create board
            </button>
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