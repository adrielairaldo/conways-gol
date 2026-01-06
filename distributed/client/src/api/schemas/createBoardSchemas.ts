import z from "zod";

/**
 * Schema for validating the response when creating a new board.
 * Ensures the boardId is a non-empty string.
 */
export const CreateBoardResponseSchema = z.object({
    boardId: z.string().min(1, "Board ID cannot be empty")
});

/**
 * Schema for validating the request when creating a new board.
 * Ensures the initial grid is a 2D array of numbers (0 or 1).
 */
export const CreateBoardRequestSchema = z.object({
    initialGrid: z.array(z.array(z.number().int().min(0).max(1)))
});

export type CreateBoardResponse = z.infer<typeof CreateBoardResponseSchema>;
export type CreateBoardRequest = z.infer<typeof CreateBoardRequestSchema>;