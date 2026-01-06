import z from "zod";

/**
 * Schema for validating the response when advancing a board.
 * Ensures the grid is a 2D array of numbers and generation is a number.
 */
export const AdvanceBoardResponseSchema = z.object({
    grid: z.array(z.array(z.number())),
    generation: z.number()
});

/**
 * Schema for validating the request when advancing a board.
 * Ensures steps is a positive integer.
 */
export const AdvanceBoardRequestSchema = z.object({
    steps: z.number().int().positive("Steps must be a positive integer")
});

export type AdvanceBoardResponse = z.infer<typeof AdvanceBoardResponseSchema>;
export type AdvanceBoardRequest = z.infer<typeof AdvanceBoardRequestSchema>;