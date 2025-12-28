import type { Grid } from "../domain/grid";

export type GameState = {
    grid: Grid;
    isRunning: boolean;
    generation: number;
};

export type GameAction =
    | { type: "START" }
    | { type: "STOP" }
    | { type: "TICK" }
    | { type: "RESET"; grid: Grid }
    | { type: "TOGGLE_CELL"; targetRow: number; targetColumn: number };
