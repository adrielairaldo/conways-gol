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

    return {
        boardId,
        boardState,
        isLoading,
        createNewBoard,
        advance
    };
}
