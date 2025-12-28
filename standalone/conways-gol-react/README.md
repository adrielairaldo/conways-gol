# Purpose of this project
Deepen my understanding of Conway's Game of Life through a minimal implementation using only React.
The business rules will be implemented based on the description provided in https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life

# Get it started
Only run in the root directory:

    npm run dev

The port is static at 3001, so it should be available at http://localhost:3001/

# Core Principles
 - Separation of concerns: Game rules ≠ rendering ≠ state orchestration. *I'm a BE eng., I'll try to bring some of my knowledge to FE.*
 - Pure functions where possible; focus on deterministic evolution of the grid.
 - Predictable state updates -> No hidden side effects.
 - Performance-aware but pragmatic: Optimize only *when it matters*, not prematurely
 - Scalable design. Easy to add new features (speed control, patterns, infinite grid)

# High-Level Architecture
## Logical Layers
Division of architecture into three layers (kind of mimicking typical BE architecture):

    /domain        → Game rules & algorithms (pure, framework-agnostic)
    /state         → Game lifecycle & orchestration
    /ui            → React components (rendering only)

## Domain Layer
 - Pure Logic, no React.
 - Immutable Data Model (*Cell*, *Grid*):  Easy to reason about, prevents errors, aligns with React's philosophy.
 - Game Rules (*countAliveNeighbors*, *nextGeneration*): SRP (Single Responsibility Principle), pure functions, only Business Rules.

## State Management Layer
 - My choice: only React and useReducer for state management.
 - State Machine like: Explicit state transitions, easy to reason about.
 - It allows me to do a little mirroring with the pattern commands and handlers I'm used to.

## React Layer (UI)
 - **App:** owns state & lifecycle.
 - **Grid:** layout only.
 - **Cell:** render + click interaction

### Rendering Strategy
 - Use *React.memo* on *Cell*; avoid re-rendering unchanged cells.
 - Pass only minimal props.

###  Timing & Game Loop
 - *useEffect + setInterval* in *App* component (responsible for state & lifecycle).
 - Why? Declarative lifecycle, no hidden mutable timers.

###  Styling Philosophy
 - I'm not great at that. I'm looking for something simple that will allow me to implement the idea.
 - I use CSS grid with minimal colors, and no animation unless trivial.
 - For components that merit it, I create a style sheet per component (it's not something I love doing, but in this case it isolates the logic from the rendering a little more).
 - Maybe I could play around with a framework like Tailwind.