# LoyaltySystem

Welcome to the **LoyaltySystem** project — a modular, production-ready loyalty management service built using modern .NET practices.

## Project Overview

**LoyaltySystem** is a cleanly architected solution designed to manage loyalty points, transactions, and user data, featuring:

- **Clean Architecture** principles (Separation into API, Application, Infrastructure, Domain, and Core shared layers).
- **CQRS (Command Query Responsibility Segregation)** pattern implementation.
- **JWT Authentication** for secure API access.
- **Entity Framework Core** for database operations with a **BaseRepository** abstraction.
- **FluentValidation** for request validation.
- **AutoMapper** for object mapping.
- **Redis caching** integration.
- **Database Migrations** with EF Core.
- **Docker** and **Docker Compose** setup.
- **Unit Tests** with controllers and services coverage.
- **Swagger** for API documentation.
- **Serilog** for structured logging (async logging to console and file).

## Tech Stack

- .NET 8
- ASP.NET Core Web API
- MediatR
- FluentValidation
- Entity Framework Core
- AutoMapper
- Redis
- Docker & Docker Compose
- PostgreSQL
- xUnit (for unit tests)
- Moq
- Serilog (for logging)

## Features

- Create users.
- Earn loyalty points.
- Query user total points.
- Secure endpoints with JWT authentication.
- CQRS handlers for commands and queries.
- Fluent API validation.
- Centralized error handling with Middleware.
- Automated database migrations.
- Unit testing support.
- Full containerization with Docker.
- Async logging with Serilog to console and log files in the `Logs` folder.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/)
- [Redis](https://redis.io/) (containerized with Docker Compose)

### Setup Instructions (Manual)

1. Clone the repository:
   ```bash
   git clone https://github.com/AlbertKarapetyan/LoyaltySystem.git
   ```

2. Navigate into the project directory:
   ```bash
   cd LoyaltySystem
   ```

3. Configure `appsettings.json` with your database and JWT settings.

4. Apply EF Core migrations manually:

5. Run the application:
   ```bash
   dotnet run --src/LoyaltySystem.API
   ```

6. Access Swagger UI at `https://localhost:{port}/swagger` to explore the API endpoints.

### Logging with Serilog
Serilog is configured to log messages asynchronously and output them both to the console and to log files. Log files are saved in the Logs folder with a rolling date pattern.

### Setup Instructions (with Docker Compose)

1. Build and run the containers:
   ```bash
   docker-compose up --build
   ```

2. Docker Compose will automatically:
   - Build Docker images for API and migrations runner.
   - Start PostgreSQL and Redis containers.
   - Run database migrations automatically.
   - Execute unit tests.
   - Launch the LoyaltySystem API.

3. Access Swagger UI at `http://localhost:5000/swagger`.

### Testing

Navigate to the test project and run manually if needed:
```bash
cd test/LS.Test
 dotnet test
```

(When using Docker Compose, tests are automatically executed before container launch.)

## Docker Files Overview

- `Dockerfile` in API project: builds and runs the LoyaltySystem.API.
- `docker-compose.yml`: defines all services — API, PostgreSQL, Redis, and migrations.

## Folder Structure

```
LoyaltySystem.sln
src/
  LoyaltySystem.API/             // API layer (Controllers, Middleware)
  LoyaltySystem.Application/     // Application layer (CQRS - Commands, Queries, Handlers, DTOs, Validators)
  LoyaltySystem.Domain/          // Domain layer (Entities, ValueObjects)
  LoyaltySystem.Infrastructure/  // Infrastructure layer (DbContext, Migrations, Repository Implementations, External Services)
  LoyaltySystem/                 // Main
tests/
  LS.Test/                       // Unit Tests
Dockerfile                       // Dockerfile for building API project
docker-compose.yml               // Docker Compose file orchestrating API, PostgreSQL, Redis, and migrations
```

## License

Distributed under the MIT License.

---

Built with passion and best practices ✨.

