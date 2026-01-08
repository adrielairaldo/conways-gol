# Conway's Game of Life **Service**

## Overview
Backend implementation of Conway's Game of Life built with **.NET 8**, following **Clean Architecture** principles and a **CQRS**-oriented design.

The service exposes a REST API to manage boards, advance generations, and simulate the game until a conclusion is reached, while ensuring persistence, recoverability, and production readiness.

## Architecture

The solution is structured according to **Clean Architecture**, where dependencies always point inward and the core domain remains isolated from infrastructure and framework concerns.

The project is divided into four main layers:

### 1. Domain
- **Responsibility**: Pure business logic and Conway’s Game of Life rules.
- **Key Characteristics**:
  - Framework-agnostic
  - No external dependencies
  - Fully unit-testable
- **Components**:
  - **Entities & Value Objects**: `Board`, `BoardState`, `Grid`, `CellState`, `BoardId`
  - **Domain Services**: `NextGenerationCalculator`, `SimulationService`
  - **Domain Interfaces (Repositories)**:
    - `IBoardRepository`
    - `ICacheService`
- **Dependencies**: None

#### Domain Model Overview (Conceptual)

flowchart LR
    Board -->|has| BoardState
    BoardState -->|contains| Grid
    Grid -->|matrix of| CellState

- A **Board** represents a long-lived aggregate.
- A **BoardState** is immutable and represents a snapshot in time.
- A **Grid** models the 2D cell matrix.
- Domain services operate on these structures without persistence concerns.

### 2. Application
- **Responsibility**: Orchestration of use cases and coordination between domain and infrastructure.
- **Key Characteristics**:
  - No direct knowledge of infrastructure implementations
  - Implements application workflows
- **Components**:
  - **Commands**: `CreateBoard`, `AdvanceBoard`, `SimulateUntilConclusion`
  - **Queries**: `GetBoard`
  - **Handlers** implementing CQRS abstractions
- **Dependencies**: Domain

### 3. Infrastructure
- **Responsibility**: Technical implementations of external concerns.
- **Key Characteristics**:
  - Contains no business logic
  - Implements domain/application interfaces

The infrastructure layer is split into two dedicated projects:

#### Infrastructure.MongoDb
- **Purpose**: Persistent storage of boards and their states.
- **Responsibilities**:
  - Implements `IBoardRepository`
  - Maps domain entities to MongoDB documents
  - Handles database connectivity and configuration
- **Rationale**:
  - The domain is inherently **document-oriented**
  - Each board is a self-contained aggregate
  - MongoDB enables efficient single-document reads and writes
- **Dependencies**: Domain

#### Infrastructure.Redis
- **Purpose**: High-performance caching layer.
- **Responsibilities**:
  - Implements `ICacheService`
  - Caches frequently accessed board states
  - Reduces load on the primary database
- **Usage**:
  - Read-through cache for queries (e.g. `GetBoard`)
  - Cache invalidation on state-changing commands (e.g. `AdvanceBoard`)
- **Dependencies**: Domain

### 4. Web API
- **Responsibility**: Hosting the REST API and exposing the application to clients.
- **Components**:
  - Controllers
  - Request/response DTOs
  - Swagger / OpenAPI documentation
  - Health checks
- **Dependencies**: Application

## Key Design Principles
- **Clean Architecture**: Clear separation of concerns with inward-facing dependencies.
- **CQRS**: Commands mutate state; queries read state.
- **Repository Pattern**: Abstracts persistence details from application logic.
- **Caching Strategy**: Explicit cache usage to improve performance and scalability.
- **Testability**: Domain and application layers are fully unit-tested in isolation.

## Persistence & Indexing Strategy

The application uses **MongoDB** as the primary persistence layer.

### Access Patterns
- Boards are always accessed by `BoardId`
- Each operation reads or updates a single board document
- No cross-board queries or aggregations are currently required

### Indexes
- MongoDB primary index on `_id` is used for all board lookups
- `BoardId` is mapped directly to `_id` to guarantee **O(1)** retrieval
- This ensures constant-time reads for all core operations

### Future Considerations
- A TTL index on `createdAt` can be added to automatically clean up abandoned boards
- Additional indexes may be introduced if new query patterns emerge (e.g. listing boards)

This indexing strategy aligns with current access patterns while remaining extensible for future requirements.

## Caching Strategy

- Redis is used as a distributed cache
- Cache keys are derived from domain identifiers (e.g. `BoardId`)
- Query handlers attempt cache reads before hitting persistence
- Command handlers invalidate cache entries after state changes

This approach improves performance while maintaining consistency.

## Testing Strategy
- **Domain**: Fully unit-tested, deterministic, no mocks
- **Application**: Handler-level tests with mocked repositories and cache

## Notes on Scalability & Extensibility
- Stateless Web API enables horizontal scaling
- Redis cache reduces database pressure
- Domain-driven design allows future rule extensions
- Infrastructure implementations can be replaced without impacting core logic