import type { AdvanceBoardRequest } from '../api/contracts/AdvanceBoardRequest';
import { BoardsApi } from '../api/boardsApi';
import type { BoardState } from '../domain/BoardState';
import type { CreateBoardRequest } from '../api/contracts/CreateBoardRequest';
import { useCallback, useState } from 'react';

export function useBoard() {
    const [boardId, setBoardId] = useState<string | null>(null);
    const [boardState, setBoardState] = useState<BoardState | null>(null);
    const [isLoading, setIsLoading] = useState(false);

    const STORED_BOARD_ID_KEY = "conways-game-of-life.board-id";

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
