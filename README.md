# Conway's Game of Life

## Running the Application

### Prerequisites
- Docker and Docker Compose installed on your system

### Setup and Launch

1. Navigate to the infrastructure directory:
```powershell
   cd distributed/infra/
```

2. Create the required Docker volumes:
```powershell
   docker volume create mongodbdata
   docker volume create mongodbconfig
```

3. Start the application using Docker Compose:
```powershell
   docker-compose up
```

This command will launch three containers on an isolated network:
- **Client**: React frontend application
- **Service**: Backend API
- **MongoDB**: Database for game state persistence

### Access Points

Once the containers are running, you can access:

| Component | URL | Description |
|-----------|-----|-------------|
| Game Interface | http://localhost:3000/ | Main application UI |
| API Documentation | http://localhost:5000/swagger/index.html | Swagger API documentation |
| Health Check (Live) | http://localhost:5000/health/live | Liveness probe endpoint |
| Health Check (Ready) | http://localhost:5000/health/ready | Readiness probe endpoint |

## Project Structure

The repository contains two implementations:

### 1. Standalone Version
**Location**: `standalone/`

A single-page React application with all game logic implemented on the client side. This version was developed first to understand the domain and validate the game mechanics before building the distributed architecture.

**Purpose**: Domain exploration and proof of concept

### 2. Distributed Version
**Location**: `distributed/`

A production-ready implementation following a client-server architecture with three main components:
```
distributed/
├── client/          # Frontend application (thin client)
├── service/         # Backend API
└── infra/           # Docker infrastructure configuration
```

#### Components:

- **Client** (`distributed/client/`): 
  - React-based frontend with TypeScript
  - Handles UI presentation and user interactions
  - Communicates with the backend API for game logic
  - No business logic on the client side

- **Service** (`distributed/service/`): 
  - Backend API built with ASP.NET Core 8
  - Implements Conway's Game of Life rules and logic
  - Manages game state persistence in MongoDB
  - Exposes API endpoints

- **Infrastructure** (`distributed/infra/`): 
  - Docker Compose configuration
  - Container orchestration
  - Network configuration
  - Volume management for data persistence

## Architecture Documentation

Detailed documentation for each component:

- **Client Architecture**: [`distributed/client/README.md`](distributed/client/README.md)
  - Frontend layer structure
  - Component hierarchy
  - API integration approach
  - State management strategy

- **Service Architecture**: [`distributed/service/README.md`](distributed/service/README.md)
  - Backend architecture
  - API endpoints
  - Game logic implementation
  - Database schema

## Development Approach

The project was developed in two phases:

1. **Standalone Implementation**: Built a client-only version to understand the game domain, validate the rules of Conway's Game of Life, and prototype the user interface.

2. **Distributed Implementation**: Refactored into a client-server architecture to separate concerns, enable scalability, and demonstrate professional software design patterns with proper layer separation.

## Stopping the Application

To stop all running containers:
```powershell
docker-compose down
```

To stop and remove volumes (this will delete all game data):
```powershell
docker-compose down -v
```

# Author

**César Adriel Airaldo**

- LinkedIn: [linkedin.com/in/adrielairaldo](https://www.linkedin.com/in/adrielairaldo/)
- Email: adrielairaldo@hotmail.com | adrielairaldo@gmail.com

---

*This project was developed as a demonstration of full-stack development skills using React, TypeScript, and modern architectural patterns.*
