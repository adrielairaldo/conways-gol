import type { AdvanceBoardRequest } from './contracts/AdvanceBoardRequest';
import type { AdvanceBoardResponse } from './contracts/AdvanceBoardResponse';
import type { CreateBoardRequest } from './contracts/CreateBoardRequest';
import type { CreateBoardResponse } from './contracts/CreateBoardResponse';
import type { GetBoardResponse } from './contracts/GetBoardResponse';
import { httpClient } from './httpClient';

export const BoardsApi = {
  createBoard: (request: CreateBoardRequest) => httpClient.post<CreateBoardResponse>("/api/Boards", request),
  getBoard: (boardId: string) => httpClient.get<GetBoardResponse>(`/api/Boards/${boardId}`),
  advanceBoard: (boardId: string, request: AdvanceBoardRequest) => httpClient.post<AdvanceBoardResponse>(`/api/Boards/${boardId}/advance`, request),
};