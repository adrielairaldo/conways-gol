import React from "react";

type Props = {
    isRunning: boolean;
    generation: number;
    onStart: () => void;
    onStop: () => void;
    onReset: () => void;
};

export const GameControls: React.FC<Props> = ({
    isRunning,
    generation,
    onStart,
    onStop,
    onReset,
}) => {
    const handleToggleRun = isRunning ? onStop : onStart;
    const runButtonLabel = isRunning ? "Stop" : "Start";

    return (
        <div style={{ marginBottom: 16 }}>
            <button onClick={handleToggleRun}>
                {runButtonLabel}
            </button>

            <button onClick={onReset} style={{ marginLeft: 8 }}>
                Reset
            </button>

            <span style={{ marginLeft: 16 }}>
                Generation: {generation}
            </span>
        </div>
    );
};
