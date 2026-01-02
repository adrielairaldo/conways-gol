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