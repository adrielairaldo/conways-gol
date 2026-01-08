# Conway's Game of Life Service

## Overview
Backend implementation of Conway's Game of Life built with .NET 8, following Clean Architecture and CQRS.

## Architecture
The project is divided into four main layers:

### 1. Domain
- **Responsibility**: Pure business logic and Game of Life rules.
- **Components**: `Board`, `Grid`, `CellState`, `SimulationService`.
- **Dependencies**: None.

### 2. Application
- **Responsibility**: Orchestration and use cases.
- **Components**: 
  - **Commands**: `CreateBoard`, `AdvanceBoard`.
  - **Queries**: `GetBoard`.
  - **Interfaces**: `IBoardRepository`.
- **Dependencies**: Domain.

### 3. Infrastructure
- **Responsibility**: Database persistence and external integrations.
- **Components**: MongoDB implementation, Document Mappers.
- **Dependencies**: Domain, Application.

### 4. Web API
- **Responsibility**: Hosting the REST API and documentation.
- **Components**: Controllers, Swagger, DTOs.
- **Dependencies**: Application.

## Key Design Principles
- **Clean Architecture**: Dependencies point only inward.
- **CQRS**: Clear separation between actions that change state and actions that read state.
- **Repository Pattern**: Decouples the application from MongoDB details.
- **Unit Testing**: Domain logic is fully isolated for easy testing.

## Persistence & Indexing Strategy

The application uses MongoDB as the persistence layer, as the domain model (Board and BoardState) is inherently document-oriented.

### Access Patterns
- Boards are always accessed by `BoardId`
- Each operation reads or updates a single board document
- No cross-board queries or aggregations are currently required

### Indexes
- MongoDB primary index on `_id` is used for all board lookups
- `BoardId` is mapped directly to `_id` to guarantee O(1) retrieval
- This ensures constant-time reads for all core operations

### Future Considerations
- A TTL index on `createdAt` can be added to automatically clean up abandoned boards
- Additional indexes may be introduced if new query patterns emerge (e.g. listing boards)

This indexing strategy aligns with the current access patterns while remaining extensible for future requirements.