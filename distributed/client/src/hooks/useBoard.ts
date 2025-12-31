import { useCallback, useState } from "react";
import type { BoardState } from "../domain/BoardState";
import { BoardsApi } from "../api/boardsApi";
import type { CreateBoardRequest } from "../api/contracts/CreateBoardRequest";
import type { AdvanceBoardRequest } from "../api/contracts/AdvanceBoardRequest";

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
            const board = await BoardsApi.getBoard(storedBoardId);

            setBoardId(storedBoardId);
            setBoardState(board);
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

            const createBoardRequest: CreateBoardRequest = { initialGrid: initialGrid };
            const createBoardResult = await BoardsApi.createBoard(createBoardRequest);
            setBoardId(createBoardResult.boardId);

            localStorage.setItem(STORED_BOARD_ID_KEY, createBoardResult.boardId);

            const board = await BoardsApi.getBoard(createBoardResult.boardId);
            setBoardState(board);
        } finally {
            setIsLoading(false);
        }
    }, []);

    const advance = useCallback(async (steps: number) => {
        if (!boardId) return;

        setIsLoading(true);
        try {
            const advanceBoardRequest: AdvanceBoardRequest = { steps: steps };
            const nextState = await BoardsApi.advanceBoard(boardId, advanceBoardRequest);
            setBoardState(nextState);
        } finally {
            setIsLoading(false);
        }
    },
        [boardId]
    );

    const resetBoard = useCallback(() => {
        localStorage.removeItem(STORED_BOARD_ID_KEY);

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
