import '../styles/controls.css';

interface Props {
    isGameStarted: boolean;
    onCreateBoard?: () => void;
    onAdvance?: () => void;
    generation?: number;
    disabled: boolean;
}

export const Controls: React.FC<Props> = ({ isGameStarted, onCreateBoard, onAdvance, generation,disabled }) => (
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
            </>
        )}
    </div>
);