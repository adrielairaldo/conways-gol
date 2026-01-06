import type { AdvanceBoardRequest } from './contracts/AdvanceBoardRequest';
import type { AdvanceBoardResponse } from './contracts/AdvanceBoardResponse';
import { AdvanceBoardResponseSchema } from './schemas/advanceBoardSchemas';
import type { CreateBoardRequest } from './contracts/CreateBoardRequest';
import type { CreateBoardResponse } from './contracts/CreateBoardResponse';
import { CreateBoardResponseSchema } from './schemas/createBoardSchemas';
import type { GetBoardResponse } from './contracts/GetBoardResponse';
import { GetBoardResponseSchema } from './schemas/getBoardResponse';
import { httpClient } from './httpClient';

export const BoardsApi = {
  createBoard: async (request: CreateBoardRequest): Promise<CreateBoardResponse> => {
    const response = await httpClient.post<CreateBoardResponse>("/api/Boards", request);
    return CreateBoardResponseSchema.parse(response);
  },

  getBoard: async (boardId: string): Promise<GetBoardResponse> => {
    const response = await httpClient.get<GetBoardResponse>(`/api/Boards/${boardId}`);
    return GetBoardResponseSchema.parse(response);
  },

  advanceBoard: async (boardId: string, request: AdvanceBoardRequest): Promise<AdvanceBoardResponse> => {
    const response = await httpClient.post<AdvanceBoardResponse>(`/api/Boards/${boardId}/advance`, request);
    return AdvanceBoardResponseSchema.parse(response);
  }
};