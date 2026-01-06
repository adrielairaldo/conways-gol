import z from "zod";

/**
 * Schema for validating the response when retrieving a board.
 * Ensures the grid is a 2D array of numbers and generation is a number.
 */
export const GetBoardResponseSchema = z.object({
    grid: z.array(z.array(z.number())),
    generation: z.number()
});

export type GetBoardResponse = z.infer<typeof GetBoardResponseSchema>;