import type { AdvanceBoardRequest } from './contracts/AdvanceBoardRequest';
import type { BoardState } from '../domain/BoardState';
import type { CreateBoardRequest } from './contracts/CreateBoardRequest';
import type { CreateBoardResponse } from './contracts/CreateBoardResponse';
import { httpClient } from './httpClient';

export const BoardsApi = {
  createBoard: (request: CreateBoardRequest) => httpClient.post<CreateBoardResponse>("/api/Boards", request),
  getBoard: (boardId: string) => httpClient.get<BoardState>(`/api/Boards/${boardId}`),
  advanceBoard: (boardId: string, request: AdvanceBoardRequest) => httpClient.post<BoardState>(`/api/Boards/${boardId}/advance`, request),
};