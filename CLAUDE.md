# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a .NET 8 ASP.NET Core Web API that serves UFC data to a Blazor WebAssembly frontend. The API provides CRUD operations for UFC events, fights, fighters, and rounds with Entity Framework Core and SQL Server.

## Architecture

- **Framework**: ASP.NET Core Web API (.NET 8.0)
- **Database**: SQL Server with Entity Framework Core
- **Models**: CSV-based data models in `/CSVObjects/`
- **Controllers**: Located in `/DB/` directory
- **Database Context**: `AppDbContext` in `/DB/DbContext.cs`
- **Frontend Integration**: Serves data to Blazor WebAssembly app at `https://app.nedevans.au`

## Development Commands

```bash
# Build the project
dotnet build

# Run the API (runs on http://0.0.0.0:8080)
dotnet run

# Run with hot reload
dotnet watch run

# Apply Entity Framework migrations
dotnet ef database update

# Create new migration
dotnet ef migrations add <MigrationName>
```

## Key Implementation Details

### Database Connection
- Uses environment variables for connection string: `dbServer`, `dbName`, `dbPassword`
- Connection string built in `Program.cs:14`
- Includes retry logic with 5 attempts and 20-second delays

### CORS Configuration
- Configured for frontend origin: `https://app.nedevans.au`
- Located in `Program.cs:30-38`

### Data Models
All models are in `/CSVObjects/` with Entity Framework annotations:
- `EventCsv` - UFC events
- `FightCsv` - Individual fights with relationships to fighters/events
- `FighterCsv` - Fighter information
- `RoundCsv` - Round-by-round data

### Controllers Structure
Located in `/DB/` directory:
- `EventsController.cs` - Event CRUD operations
- `FightsController.cs` - Fight operations with pagination and filtering
- `FightersController.cs` - Fighter operations
- `RoundsController.cs` - Round operations

### Pagination Implementation
**COMPLETED**: The `/fights` endpoint now has full server-side pagination implemented:

#### Implementation Details:
- **Response Model**: `PaginatedFights` class in `/CSVObjects/PaginatedFights.cs`
- **Query Parameters**:
  - `pageNumber` (default: 1), `pageSize` (default: 10)
  - Filtering: `eventId`, `fighterId`, `dateFrom`, `dateTo`, `weightClass`, `round`, `result`, `titleFight`, `referee`
  - Sorting: `sortBy` (default: "eventdate"), `order` (default: "desc")
- **Response Format**: `{ "fights": [...], "totalCount": number }`
- **Location**: Updated `GetFights` method in `/DB/FightsController.cs:23`

#### API Usage:
```bash
curl "http://localhost:8080/fights?pageNumber=1&pageSize=10&weightClass=lightweight"
```

#### TODO - Future Pagination:
The following endpoints should be updated to use the same pagination pattern:
- `/events` - EventsController.cs
- `/fighters` - FightersController.cs
- `/rounds` - RoundsController.cs

Each will need:
1. `PaginatedEvents`, `PaginatedFighters`, `PaginatedRounds` response models
2. Updated controller methods with pagination parameters
3. `Skip()`, `Take()`, and `CountAsync()` implementation

## Database Schema Notes

- Composite primary key on `RoundCsv`: `FightId`, `FighterId`, `Round`
- Foreign key relationships configured with `NoAction` delete behavior
- Indexes on key fields for performance (`EventId`, `Fighter1Id`, `Fighter2Id`, `WinnerId`)

## Testing

- Swagger UI available at `/swagger` in development
- API hosted at production URL: `https://sqlapi.nedevans.au`

## Important Notes

- All responses must use the `*Csv` model format expected by the frontend
- Environment variables required for database connection
- CORS is specifically configured for the frontend domain
- The existing `API_CLAUDE.md` contains detailed pagination implementation examples and more comprehensive documentation