import { describe, it, expect, beforeEach, vi, afterEach } from 'vitest';
import { renderHook, waitFor } from '@testing-library/react';
import { useBoard } from '../../hooks/useBoard';
import { BoardsApi } from '../../api/boardsApi';

// Mock the entire API module
vi.mock('../../api/boardsApi', () => ({
    BoardsApi: {
        createBoard: vi.fn(),
        getBoard: vi.fn(),
        advanceBoard: vi.fn(),
    }
}));

describe('useBoard', () => {
    const mockOnError = vi.fn();
    const DEFAULT_STORAGE_KEY = 'conways-game-of-life.board-id';

    beforeEach(() => {
        vi.clearAllMocks();
        localStorage.clear();
        // Ensure the environment variable used by the hook matches our test expectations
        vi.stubEnv('VITE_STORED_BOARD_ID_KEY', DEFAULT_STORAGE_KEY);
    });

    afterEach(() => {
        vi.unstubAllEnvs();
    });

    describe('Initial State', () => {
        it('should initialize with null board state and not loading', () => {
            // Arrange & Act
            const { result } = renderHook(() => useBoard());

            // Assert
            expect(result.current.boardState).toBeNull();
            expect(result.current.boardId).toBeNull();
            expect(result.current.isLoading).toBe(false);
        });
    });

    describe('createNewBoard', () => {
        it('should create a new board successfully and store board ID in localStorage', async () => {
            // Arrange
            const mockInitialGrid = [[0, 1], [1, 0]];
            const mockBoardId = 'test-board-123';
            const mockBoardState = { grid: mockInitialGrid, generation: 0 };

            vi.mocked(BoardsApi.createBoard).mockResolvedValue({ boardId: mockBoardId });
            vi.mocked(BoardsApi.getBoard).mockResolvedValue(mockBoardState);

            const { result } = renderHook(() => useBoard());

            // Act
            await result.current.createNewBoard(mockInitialGrid, mockOnError);

            // Assert
            await waitFor(() => {
                expect(result.current.boardId).toBe(mockBoardId);
                expect(result.current.boardState).toEqual(mockBoardState);
                expect(result.current.isLoading).toBe(false);
            });

            expect(BoardsApi.createBoard).toHaveBeenCalledWith({ initialGrid: mockInitialGrid });
            expect(BoardsApi.getBoard).toHaveBeenCalledWith(mockBoardId);
            expect(localStorage.getItem(DEFAULT_STORAGE_KEY)).toBe(mockBoardId);
            expect(mockOnError).not.toHaveBeenCalled();
        });

        it('should handle API errors and call onError callback', async () => {
            // Arrange
            const mockInitialGrid = [[0, 1], [1, 0]];
            const mockError = { userMessage: 'Cannot connect to server' };

            vi.mocked(BoardsApi.createBoard).mockRejectedValue(mockError);

            const { result } = renderHook(() => useBoard());

            // Act
            await result.current.createNewBoard(mockInitialGrid, mockOnError);

            // Assert
            await waitFor(() => {
                expect(result.current.isLoading).toBe(false);
            });

            expect(mockOnError).toHaveBeenCalledWith('Cannot connect to server');
            expect(result.current.boardState).toBeNull();
            expect(result.current.boardId).toBeNull();
        });

        it('should set loading state to true during API call', async () => {
            // Arrange
            const mockInitialGrid = [[0, 1]];
            let resolveCreate: (value: any) => void;
            const createPromise = new Promise((resolve) => { resolveCreate = resolve; });

            vi.mocked(BoardsApi.createBoard).mockReturnValue(createPromise as any);

            const { result } = renderHook(() => useBoard());

            // Act
            const createPromise2 = result.current.createNewBoard(mockInitialGrid, mockOnError);

            // Assert
            await waitFor(() => {
                expect(result.current.isLoading).toBe(true);
            });

            // Cleanup
            resolveCreate!({ boardId: 'test' });
            await createPromise2;
        });
    });

    describe('advance', () => {
        it('should advance the board and update generation', async () => {
            // Arrange
            const mockBoardId = 'test-board-123';
            const mockInitialState = { grid: [[0, 1], [1, 0]], generation: 0 };
            const mockAdvancedState = { grid: [[1, 0], [0, 1]], generation: 1 };

            vi.mocked(BoardsApi.createBoard).mockResolvedValue({ boardId: mockBoardId });
            vi.mocked(BoardsApi.getBoard).mockResolvedValue(mockInitialState);
            vi.mocked(BoardsApi.advanceBoard).mockResolvedValue(mockAdvancedState);

            const { result } = renderHook(() => useBoard());

            await result.current.createNewBoard([[0, 1], [1, 0]], mockOnError);
            await waitFor(() => expect(result.current.boardId).toBe(mockBoardId));

            // Act
            await result.current.advance(1, mockOnError);

            // Assert
            await waitFor(() => {
                expect(result.current.boardState?.generation).toBe(1);
                expect(result.current.boardState?.grid).toEqual([[1, 0], [0, 1]]);
                expect(result.current.isLoading).toBe(false);
            });

            expect(BoardsApi.advanceBoard).toHaveBeenCalledWith(mockBoardId, { steps: 1 });
        });

        it('should not advance if no board exists', async () => {
            // Arrange
            const { result } = renderHook(() => useBoard());

            // Act
            await result.current.advance(1, mockOnError);

            // Assert
            expect(BoardsApi.advanceBoard).not.toHaveBeenCalled();
            expect(result.current.boardState).toBeNull();
        });

        it('should handle API errors during advance', async () => {
            // Arrange
            const mockBoardId = 'test-board-123';
            const mockError = { userMessage: 'Server error' };

            // Professional approach: Use spyOn to ensure type-safe mock methods
            vi.spyOn(BoardsApi, 'createBoard').mockResolvedValue({ boardId: mockBoardId });
            vi.spyOn(BoardsApi, 'getBoard').mockResolvedValue({ grid: [[0, 1]], generation: 0 });
            vi.spyOn(BoardsApi, 'advanceBoard').mockRejectedValue(mockError);

            const { result } = renderHook(() => useBoard());

            // Setup: Create the board and WAIT for the state to reflect the new ID
            await result.current.createNewBoard([[0, 1]], mockOnError);
            await waitFor(() => {
                expect(result.current.boardId).toBe(mockBoardId);
            });

            // Clear mocks after setup so we only track calls from the 'Act' phase
            vi.clearAllMocks(); 

            // Act
            await result.current.advance(1, mockOnError);

            // Assert
            // We wrap the error assertion inside waitFor. 
            // This ensures we wait for the catch block and state updates to finish.
            await waitFor(() => {
                expect(mockOnError).toHaveBeenCalledWith('Server error');
                expect(result.current.isLoading).toBe(false);
            });
            
            // Additional check to ensure we actually hit the API
            expect(BoardsApi.advanceBoard).toHaveBeenCalledTimes(1);
        });
    });

    describe('resetBoard', () => {
        it('should reset board state and clear localStorage', async () => {
            // Arrange
            const mockBoardId = 'test-board-123';
            vi.mocked(BoardsApi.createBoard).mockResolvedValue({ boardId: mockBoardId });
            vi.mocked(BoardsApi.getBoard).mockResolvedValue({ grid: [[1, 1]], generation: 5 });

            const { result } = renderHook(() => useBoard());
            await result.current.createNewBoard([[1, 1]], mockOnError);

            // Act
            result.current.resetBoard();

            // Assert
            expect(result.current.boardId).toBeNull();
            expect(result.current.boardState).toBeNull();
            expect(localStorage.getItem(DEFAULT_STORAGE_KEY)).toBeNull();
        });
    });

    describe('recoverPreviousSessionIfAny', () => {
        it('should recover board from localStorage if board ID exists', async () => {
            // Arrange
            const mockBoardId = 'stored-board-456';
            const mockBoardState = { grid: [[1, 1], [0, 0]], generation: 5 };

            localStorage.setItem(DEFAULT_STORAGE_KEY, mockBoardId);
            vi.mocked(BoardsApi.getBoard).mockResolvedValue(mockBoardState);

            const { result } = renderHook(() => useBoard());

            // Act
            await result.current.recoverPreviousSessionIfAny(mockOnError);

            // Assert
            await waitFor(() => {
                expect(result.current.boardId).toBe(mockBoardId);
                expect(result.current.boardState).toEqual(mockBoardState);
                expect(result.current.isLoading).toBe(false);
            });

            expect(BoardsApi.getBoard).toHaveBeenCalledWith(mockBoardId);
            expect(mockOnError).not.toHaveBeenCalled();
        });

        it('should do nothing if no board ID exists in localStorage', async () => {
            // Arrange
            const { result } = renderHook(() => useBoard());

            // Act
            await result.current.recoverPreviousSessionIfAny(mockOnError);

            // Assert
            expect(BoardsApi.getBoard).not.toHaveBeenCalled();
            expect(result.current.boardId).toBeNull();
        });

        it('should clean up localStorage if recovery fails', async () => {
            // Arrange
            const mockBoardId = 'invalid-board';
            const mockError = { userMessage: 'Board not found' };

            localStorage.setItem(DEFAULT_STORAGE_KEY, mockBoardId);
            vi.mocked(BoardsApi.getBoard).mockRejectedValue(mockError);

            const { result } = renderHook(() => useBoard());

            // Act
            await result.current.recoverPreviousSessionIfAny(mockOnError);

            // Assert
            await waitFor(() => {
                expect(result.current.boardId).toBeNull();
                expect(localStorage.getItem(DEFAULT_STORAGE_KEY)).toBeNull();
                expect(result.current.isLoading).toBe(false);
            });
        });
    });
});