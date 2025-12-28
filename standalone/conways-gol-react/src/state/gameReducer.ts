import { nextGeneration } from "../domain/gameRules";
import { toggleCell } from "../domain/grid";
import type { GameAction, GameState } from "./gameTypes";

export function gameReducer(state: GameState, action: GameAction): GameState {
    switch (action.type) {
        case "START":
            return { ...state, isRunning: true };

        case "STOP":
            return { ...state, isRunning: false };

        case "TICK":
            return {
                ...state,
                grid: nextGeneration(state.grid),
                generation: state.generation + 1,
            };

        case "RESET":
            return {
                grid: action.grid,
                isRunning: false,
                generation: 0,
            };

        case "TOGGLE_CELL":
            return {
                ...state,
                grid: toggleCell(state.grid, action.targetRow, action.targetColumn),
            };

        default:
            return state;
    }
}
