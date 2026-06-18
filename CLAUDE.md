# RestaurantOS

> "Discord dla gastronomii" — real-time communication platform for restaurant teams.

## Project Overview

RestaurantOS is a bachelor's thesis project (WSB Merito Poznań, defense: November 2026) built by a 3-person full-stack team. It solves communication fragmentation in hospitality by providing centralized channels, knowledge base, scheduling, and real-time availability tracking.

## Tech Stack

| Layer | Technology |
|-------|------------|
| Backend | ASP.NET Core 10, C# 13 |
| ORM | Entity Framework Core 10 |
| Database | PostgreSQL 16 |
| Real-time | SignalR |
| Auth | ASP.NET Identity + JWT (Access + Refresh tokens) |
| Testing | xUnit, Moq |
| DevOps | Docker, GitHub Actions CI/CD |
| Frontend | React (optional, later phase) |

## Architecture

Clean Architecture with 4 projects:

```
RestaurantOS/
├── src/
│   ├── RestaurantOS.API/           # Controllers, SignalR Hubs, middleware
│   ├── RestaurantOS.Application/   # Use cases, DTOs, interfaces, validators
│   ├── RestaurantOS.Domain/        # Entities, value objects, domain events
│   └── RestaurantOS.Infrastructure/ # EF Core, repositories, external services
├── tests/
│   ├── RestaurantOS.UnitTests/
│   └── RestaurantOS.IntegrationTests/
├── docker-compose.yml
└── README.md
```

## Domain Models

### Core Entities

- **Restaurant** — tenant, all data scoped to restaurant
- **User** — Identity user extended with restaurant membership
- **Role** — Owner | Manager | Staff (role-based authorization)
- **Channel** — communication channel within restaurant (kitchen, bar, service, announcements)
- **Message** — real-time message in channel
- **AvailabilityItem** — item on availability board (type: Unavailable | Recommended)

### Key Relationships

```
Restaurant 1──* User (via RestaurantMembership)
Restaurant 1──* Channel
Channel    1──* Message
Channel    *──* User (via ChannelMembership)
Restaurant 1──* AvailabilityItem
```

## Four Pillars (Features)

### 1. Real-time Communication (MVP)
- Topic-based channels per restaurant
- SignalR WebSocket messaging
- Message persistence and history
- @mentions and notifications

### 2. Knowledge Base (MLP)
- Recipes, procedures, checklists
- Document versioning
- Search functionality

### 3. Staff Scheduling (MLP)
- Shift planning and publication
- Availability submission by staff
- Shift swap requests

### 4. Availability Board (MVP)
- Real-time per-restaurant board
- Two sections: ❌ Unavailable (86'd) | ✓ Recommended
- SignalR broadcast on changes
- Manager+ can edit, Staff read-only

## Coding Conventions

### C# Style
- Use `var` when type is obvious
- Primary constructors for simple classes
- Records for DTOs
- Expression-bodied members where readable
- Async/await with `Async` suffix

### Naming
- Entities: PascalCase, singular (`Channel`, not `Channels`)
- Interfaces: `I` prefix (`IChannelRepository`)
- DTOs: `EntityNameDto`, `CreateEntityRequest`, `UpdateEntityRequest`
- Endpoints: RESTful (`GET /api/restaurants/{id}/channels`)

### EF Core
- Fluent API configuration (not attributes)
- Configurations in separate `EntityConfiguration` classes
- Soft delete via `IsDeleted` flag where appropriate
- Always include `CreatedAt`, `UpdatedAt` timestamps

### Testing
- Arrange-Act-Assert pattern
- Descriptive test names: `MethodName_Scenario_ExpectedResult`
- Use builders/factories for test data
- Mock external dependencies only

## API Design

### Authentication Endpoints
```
POST   /api/auth/register
POST   /api/auth/login
POST   /api/auth/refresh
POST   /api/auth/logout
GET    /api/auth/me
```

### Restaurant Endpoints
```
POST   /api/restaurants
GET    /api/restaurants/{id}
PUT    /api/restaurants/{id}
DELETE /api/restaurants/{id}
```

### Channel Endpoints
```
GET    /api/restaurants/{restaurantId}/channels
POST   /api/restaurants/{restaurantId}/channels
GET    /api/channels/{id}
GET    /api/channels/{id}/messages?page=1&pageSize=50
```

### Availability Endpoints
```
GET    /api/restaurants/{restaurantId}/availability
POST   /api/restaurants/{restaurantId}/availability
DELETE /api/availability/{id}
```

### SignalR Hubs
```
/hubs/messages    — real-time messaging
/hubs/availability — availability board updates
```

## Current Phase

**MVP (deadline: May 18, 2026)**
- [x] Project setup
- [ ] Auth system (Identity + JWT)
- [ ] Restaurant & Channel CRUD
- [ ] Real-time messaging (SignalR)
- [ ] Availability board

## Commands

```bash
# Run locally
docker-compose up -d
dotnet run --project src/RestaurantOS.API

# Run tests
dotnet test

# Add migration
dotnet ef migrations add MigrationName -p src/RestaurantOS.Infrastructure -s src/RestaurantOS.API

# Update database
dotnet ef database update -p src/RestaurantOS.Infrastructure -s src/RestaurantOS.API
```

## Important Notes

- All data is restaurant-scoped (multi-tenancy)
- JWT tokens: Access (15 min), Refresh (7 days)
- SignalR requires JWT auth via query string for WebSocket
- Use Polish for user-facing strings, English for code
- Thesis defense: November 2026
