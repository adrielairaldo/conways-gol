import userEvent from '@testing-library/user-event';
//import { Cell } from './Cell';
import { describe, expect, it, vi } from 'vitest';
import { render } from '@testing-library/react';
import { Cell } from '../../../ui/component/Cell';

describe('Cell Component', () => {
    describe('Rendering', () => {
        it('should render an alive cell with cyan background', () => {
            // Arrange & Act
            const { container } = render(<Cell isAlive={true} />);
            const cell = container.firstChild as HTMLElement;

            // Assert
            expect(cell).toBeInTheDocument();
            expect(cell).toHaveClass('bg-cyan-400');
        });

        it('should render a dead cell with dark gray background', () => {
            // Arrange & Act
            const { container } = render(<Cell isAlive={false} />);
            const cell = container.firstChild as HTMLElement;

            // Assert
            expect(cell).toBeInTheDocument();
            expect(cell).toHaveClass('bg-gray-800/50');
        });
    });

    describe('Click Interactions', () => {
        it('should call onClick handler when cell is clicked', async () => {
            // Arrange
            const user = userEvent.setup();
            const handleClick = vi.fn();
            const { container } = render(<Cell isAlive={false} onClick={handleClick} />);
            const cell = container.firstChild as HTMLElement;

            // Act
            await user.click(cell);

            // Assert
            expect(handleClick).toHaveBeenCalledTimes(1);
        });

        it('should not throw error when clicked without onClick handler', async () => {
            // Arrange
            const user = userEvent.setup();
            const { container } = render(<Cell isAlive={false} />);
            const cell = container.firstChild as HTMLElement;

            // Act & Assert
            await expect(user.click(cell)).resolves.not.toThrow();
        });

        it('should call onClick multiple times for multiple clicks', async () => {
            // Arrange
            const user = userEvent.setup();
            const handleClick = vi.fn();
            const { container } = render(<Cell isAlive={false} onClick={handleClick} />);
            const cell = container.firstChild as HTMLElement;

            // Act
            await user.click(cell);
            await user.click(cell);
            await user.click(cell);

            // Assert
            expect(handleClick).toHaveBeenCalledTimes(3);
        });
    });

    describe('Cursor Styling', () => {
        it('should show pointer cursor when onClick handler is provided', () => {
            // Arrange & Act
            const { container } = render(<Cell isAlive={false} onClick={vi.fn()} />);
            const cell = container.firstChild as HTMLElement;

            // Assert
            expect(cell).toHaveClass('cursor-pointer');
        });

        it('should show default cursor when onClick handler is not provided', () => {
            // Arrange & Act
            const { container } = render(<Cell isAlive={false} />);
            const cell = container.firstChild as HTMLElement;

            // Assert
            expect(cell).toHaveClass('cursor-default');
        });
    });

    describe('Component Memoization', () => {
        it('should not re-render when props remain the same', () => {
            // Arrange
            const handleClick = vi.fn();
            const { rerender, container } = render(<Cell isAlive={true} onClick={handleClick} />);
            const firstRender = container.firstChild;

            // Act
            rerender(<Cell isAlive={true} onClick={handleClick} />);
            const secondRender = container.firstChild;

            // Assert
            expect(firstRender).toBe(secondRender); // Same DOM node = no re-render
        });
    });
});