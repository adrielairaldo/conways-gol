import type { BoardState } from '../domain/BoardState';
import type { CreateBoardRequest } from './contracts/CreateBoardRequest';
import type { CreateBoardResponse } from './contracts/CreateBoardResponse';
import { httpClient } from './httpClient';

export const BoardsApi = {
  createBoard: (request: CreateBoardRequest) => httpClient.post<CreateBoardResponse>("/api/Boards", request),
  getBoard: (boardId: string) => httpClient.get<BoardState>(`/api/Boards/${boardId}`),
};