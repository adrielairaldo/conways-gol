import type { AdvanceBoardRequest } from '../api/contracts/AdvanceBoardRequest';
import { BoardsApi } from '../api/boardsApi';
import type { BoardState } from '../domain/BoardState';
import type { CreateBoardRequest } from '../api/contracts/CreateBoardRequest';
import { useCallback, useState } from 'react';

/**
 * Custom React hook for managing Conway's Game of Life board state and operations.
 * 
 * This hook handles all board-related logic including creation, advancement, persistence,
 * and session recovery. It communicates with the backend API and manages loading states.
 * 
 * @returns An object containing:
 *   - boardId: The unique identifier of the current board (null if no board exists)
 *   - boardState: The current state of the board including grid and generation (null if no board exists)
 *   - isLoading: Boolean indicating if an async operation is in progress
 *   - recoverPreviousSessionIfAny: Function to restore a board from a previous session
 *   - createNewBoard: Function to create a new board with an initial grid configuration
 *   - advance: Function to advance the board by a specified number of generations
 *   - resetBoard: Function to clear the current board and start fresh
 */
export function useBoard() {
    const [boardId, setBoardId] = useState<string | null>(null);
    const [boardState, setBoardState] = useState<BoardState | null>(null);
    const [isLoading, setIsLoading] = useState(false);

    const STORED_BOARD_ID_KEY = "conways-game-of-life.board-id";

    /**
     * Attempts to recover a board from a previous session using localStorage.
     * 
     * If a board ID is found in localStorage, this function fetches the board state
     * from the API and restores it. If the recovery fails (e.g., board was deleted
     * or backend was reset), it cleans up the invalid stored data.
     * 
     * This function should be called once when the app initializes.
     */
    const recoverPreviousSessionIfAny = useCallback(async () => {
        const storedBoardId = localStorage.getItem(STORED_BOARD_ID_KEY);

        if (!storedBoardId) {
            return;
        }

        setIsLoading(true);

        try {
            // Fetch the persisted BoardState:
            const getBoardResponse = await BoardsApi.getBoard(storedBoardId);

            // Map HTTP response to Domain model:
            const boardState: BoardState = {
                grid: getBoardResponse.grid,
                generation: getBoardResponse.generation
            };

            // Update state:
            setBoardId(storedBoardId);
            setBoardState(boardState);
        } catch {
            // If recovery fails (e.g. board deleted or backend reset),
            // we clean up the invalid stored session.
            localStorage.removeItem("boardId");
            setBoardId(null);
            setBoardState(null);
        } finally {
            setIsLoading(false);
        }
    }, []);

    /**
     * Creates a new board with the provided initial grid configuration.
     * 
     * This function sends the initial grid to the API, receives a board ID,
     * stores it in localStorage for session persistence, and fetches the
     * initial board state.
     * 
     * @param initialGrid - A 2D array representing the initial state of cells
     *                      (1 for alive, 0 for dead)
     */
    const createNewBoard = useCallback(async (initialGrid: number[][]) => {
        setIsLoading(true);
        try {
            // Create the Board:
            const createBoardRequest: CreateBoardRequest = { initialGrid: initialGrid };
            const createBoardResponse = await BoardsApi.createBoard(createBoardRequest);
            setBoardId(createBoardResponse.boardId);

            // Store the Board ID for session recovery:
            localStorage.setItem(STORED_BOARD_ID_KEY, createBoardResponse.boardId);

            // Fetch the newly created BoardState:
            const getBoardResponse = await BoardsApi.getBoard(createBoardResponse.boardId);

            // Map HTTP response to Domain model:
            const boardState: BoardState = {
                grid: getBoardResponse.grid,
                generation: getBoardResponse.generation
            };

            // Update state:
            setBoardState(boardState);
        } finally {
            setIsLoading(false);
        }
    }, []);

    /**
     * Advances the current board by the specified number of generations.
     * 
     * This function sends a request to the API to calculate the next state(s)
     * of the board according to Conway's Game of Life rules, then updates
     * the local state with the new grid and generation count.
     * 
     * @param steps - The number of generations to advance (typically 1)
     */
    const advance = useCallback(async (steps: number) => {
        if (!boardId) return;

        setIsLoading(true);
        try {
            // Advance the Board:
            const advanceBoardRequest: AdvanceBoardRequest = { steps: steps };
            const advanceBoardResponse = await BoardsApi.advanceBoard(boardId, advanceBoardRequest);

            // Map HTTP response to Domain model:
            const boardState: BoardState = {
                grid: advanceBoardResponse.grid,
                generation: advanceBoardResponse.generation
            };

            // Update state:
            setBoardState(boardState);
        } finally {
            setIsLoading(false);
        }
    },
        [boardId]
    );

    /**
     * Resets the board and clears all stored data.
     * 
     * This function removes the board ID from localStorage and clears
     * the board state, returning the app to its initial state where
     * users can create a new board.
     */
    const resetBoard = useCallback(() => {
        // Clear stored Board ID:
        localStorage.removeItem(STORED_BOARD_ID_KEY);

        // Reset state:
        setBoardId(null);
        setBoardState(null);
    }, []);

    return {
        boardId,
        boardState,
        isLoading,
        recoverPreviousSessionIfAny,
        createNewBoard,
        advance,
        resetBoard
    };
}
