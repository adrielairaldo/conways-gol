import type { BoardState } from "./BoardState";

export interface Board {
  id: string;
  state: BoardState;
}