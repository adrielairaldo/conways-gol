import { describe, it, expect, beforeEach, vi, afterEach } from 'vitest';
import { renderHook, waitFor } from '@testing-library/react';
import { useBoard } from '../../../hooks/useBoard';
import { BoardsApi } from '../../../api/boardsApi';


// Mock the entire API module
vi.mock('../api/boardsApi', () => ({
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
        // Setup environment variable for the hook
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

            // Use spyOn for cleaner, type-safe mocking
            const createSpy = vi.spyOn(BoardsApi, 'createBoard').mockResolvedValue({ boardId: mockBoardId });
            const getSpy = vi.spyOn(BoardsApi, 'getBoard').mockResolvedValue(mockBoardState);

            const { result } = renderHook(() => useBoard());

            // Act
            await result.current.createNewBoard(mockInitialGrid, mockOnError);

            // Assert
            await waitFor(() => {
                expect(result.current.boardId).toBe(mockBoardId);
                expect(result.current.boardState).toEqual(mockBoardState);
            });

            expect(createSpy).toHaveBeenCalledWith({ initialGrid: mockInitialGrid });
            expect(getSpy).toHaveBeenCalledWith(mockBoardId);
            expect(localStorage.getItem(DEFAULT_STORAGE_KEY)).toBe(mockBoardId);
            expect(mockOnError).not.toHaveBeenCalled();
        });

        it('should handle API errors and call onError callback', async () => {
            // Arrange
            const mockInitialGrid = [[0, 1], [1, 0]];
            const mockError = { userMessage: 'Cannot connect to server' };

            vi.spyOn(BoardsApi, 'createBoard').mockRejectedValue(mockError);

            const { result } = renderHook(() => useBoard());

            // Act
            await result.current.createNewBoard(mockInitialGrid, mockOnError);

            // Assert
            await waitFor(() => {
                expect(mockOnError).toHaveBeenCalledWith('Cannot connect to server');
                expect(result.current.isLoading).toBe(false);
            });

            expect(result.current.boardState).toBeNull();
        });

        it('should set loading state to true during API call', async () => {
            // Arrange
            const mockInitialGrid = [[0, 1]];
            let resolveCreate: (value: any) => void;
            const createPromise = new Promise((resolve) => { resolveCreate = resolve; });

            vi.mocked(BoardsApi.createBoard).mockReturnValue(createPromise as any);

            const { result } = renderHook(() => useBoard());

            // Act
            const actPromise = result.current.createNewBoard(mockInitialGrid, mockOnError);

            // Assert
            await waitFor(() => {
                expect(result.current.isLoading).toBe(true);
            });

            // Cleanup - resolve the promise to allow the hook to finish
            resolveCreate!({ boardId: 'test' });
            await actPromise;
        });
    });

    describe('advance', () => {
        it('should advance the board and update generation', async () => {
            // Arrange
            const mockBoardId = 'test-board-123';
            const mockInitialState = { grid: [[0, 1]], generation: 0 };
            const mockAdvancedState = { grid: [[1, 0]], generation: 1 };

            // Explicitly spy on the API methods to ensure they are recognized as mocks
            vi.spyOn(BoardsApi, 'createBoard').mockResolvedValue({ boardId: mockBoardId });
            vi.spyOn(BoardsApi, 'getBoard').mockResolvedValue(mockInitialState);
            const advanceSpy = vi.spyOn(BoardsApi, 'advanceBoard').mockResolvedValue(mockAdvancedState);

            const { result } = renderHook(() => useBoard());
            
            // Setup the initial state required for the test
            await result.current.createNewBoard([[0, 1]], mockOnError);
            await waitFor(() => expect(result.current.boardId).toBe(mockBoardId));

            // Act
            await result.current.advance(1, mockOnError);

            // Assert
            await waitFor(() => {
                expect(result.current.boardState?.generation).toBe(1);
                expect(result.current.boardState?.grid).toEqual([[1, 0]]);
                expect(result.current.isLoading).toBe(false);
            });

            // Use the spy reference for a clean assertion
            expect(advanceSpy).toHaveBeenCalledWith(mockBoardId, { steps: 1 });
        });

        it('should not advance if no board exists', async () => {
            // Arrange
            // Explicitly spy on the method to ensure 'expect' recognizes it as a mock
            const advanceSpy = vi.spyOn(BoardsApi, 'advanceBoard');
            const { result } = renderHook(() => useBoard());

            // Act
            await result.current.advance(1, mockOnError);

            // Assert
            // Use the spy variable directly for the assertion
            expect(advanceSpy).not.toHaveBeenCalled();
            expect(result.current.boardState).toBeNull();
            expect(result.current.boardId).toBeNull();
        });

        it('should handle API errors during advance', async () => {
            // Arrange
            const mockBoardId = 'test-board-123';
            const mockError = { userMessage: 'Server error' };

            // Setup: Success for creation, Failure for advance
            vi.spyOn(BoardsApi, 'createBoard').mockResolvedValue({ boardId: mockBoardId });
            vi.spyOn(BoardsApi, 'getBoard').mockResolvedValue({ grid: [[0, 1]], generation: 0 });
            vi.spyOn(BoardsApi, 'advanceBoard').mockRejectedValue(mockError);

            const { result } = renderHook(() => useBoard());
            
            // Create the board first
            await result.current.createNewBoard([[0, 1]], mockOnError);
            await waitFor(() => expect(result.current.boardId).toBe(mockBoardId));
            
            vi.clearAllMocks(); // Clear calls from the Arrange phase

            // Act
            await result.current.advance(1, mockOnError);

            // Assert
            await waitFor(() => {
                expect(mockOnError).toHaveBeenCalledWith('Server error');
                expect(result.current.isLoading).toBe(false);
            });
        });
    });

    describe('resetBoard', () => {
        it('should reset board state and clear localStorage', async () => {
            // Arrange
            const mockBoardId = 'test-board-123';
            vi.mocked(BoardsApi.createBoard).mockResolvedValue({ boardId: mockBoardId });
            vi.mocked(BoardsApi.getBoard).mockResolvedValue({ grid: [[1, 1]], generation: 0 });

            const { result } = renderHook(() => useBoard());
            await result.current.createNewBoard([[1, 1]], mockOnError);
            expect(localStorage.getItem(DEFAULT_STORAGE_KEY)).toBe(mockBoardId);

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
            const mockBoardState = { grid: [[1, 1]], generation: 5 };

            localStorage.setItem(DEFAULT_STORAGE_KEY, mockBoardId);
            vi.mocked(BoardsApi.getBoard).mockResolvedValue(mockBoardState);

            const { result } = renderHook(() => useBoard());

            // Act
            await result.current.recoverPreviousSessionIfAny(mockOnError);

            // Assert
            await waitFor(() => {
                expect(result.current.boardId).toBe(mockBoardId);
                expect(result.current.boardState).toEqual(mockBoardState);
            });

            expect(BoardsApi.getBoard).toHaveBeenCalledWith(mockBoardId);
        });

        it('should clean up localStorage if recovery fails', async () => {
            // Arrange
            const mockBoardId = 'invalid-board';
            localStorage.setItem(DEFAULT_STORAGE_KEY, mockBoardId);
            vi.mocked(BoardsApi.getBoard).mockRejectedValue({ userMessage: 'Not found' });

            const { result } = renderHook(() => useBoard());

            // Act
            await result.current.recoverPreviousSessionIfAny(mockOnError);

            // Assert
            await waitFor(() => {
                expect(localStorage.getItem(DEFAULT_STORAGE_KEY)).toBeNull();
                expect(result.current.boardId).toBeNull();
            });
        });
    });
});