# CLAUDE.md - UFC API Project

This file provides guidance to Claude Code when working with the UFC API backend project.

## Project Overview

This is a REST API backend for the UFC data management system, serving data to the Blazor WebAssembly frontend. The API provides CRUD operations for UFC events, fights, fighters, and rounds.

**API Base URL**: `https://sqlapi.nedevans.au`

## Architecture

- **Backend**: ASP.NET Core Web API (.NET 8.0)
- **Database**: SQL Server/SQLite (check connection strings)
- **Data Models**: Entity Framework models or similar
- **Frontend Consumer**: Blazor WebAssembly application

## Current API Endpoints

### Events
- `GET /events` - Get all events with optional filters
- `GET /events/{id}` - Get specific event
- `POST /events` - Create new event
- `PUT /events/{id}` - Update event
- `DELETE /events/{id}` - Delete event

### Fights
- `GET /fights` - Get all fights with optional filters
- `GET /fights/{id}` - Get specific fight
- `POST /fights` - Create new fight
- `PUT /fights/{id}` - Update fight
- `DELETE /fights/{id}` - Delete fight

### Fighters
- `GET /fighters` - Get all fighters
- `GET /fighters/{id}` - Get specific fighter
- `POST /fighters` - Create new fighter
- `PUT /fighters/{id}` - Update fighter
- `DELETE /fighters/{id}` - Delete fighter

### Rounds
- `GET /rounds` - Get all rounds
- `GET /rounds/{id}` - Get specific round
- `POST /rounds` - Create new round
- `PUT /rounds/{id}` - Update round
- `DELETE /rounds/{id}` - Delete round

## Data Models

Expected CSV-based model structure (align with frontend):

```csharp
public class EventCsv
{
    public string EventId { get; set; }
    public string EventName { get; set; }
    public DateTime? EventDate { get; set; }
    public string EventCity { get; set; }
    public string EventState { get; set; }
    public string EventCountry { get; set; }
}

public class FightCsv
{
    public string FightId { get; set; }
    public string EventId { get; set; }
    public string Fighter1Id { get; set; }
    public string Fighter2Id { get; set; }
    public string WinnerId { get; set; }
    public string Result { get; set; }
    public string WeightClass { get; set; }
    public int? FinishRound { get; set; }
    public string FinishTime { get; set; }
    // Add other properties as needed
}

public class FighterCsv
{
    public string FighterId { get; set; }
    public string FighterFName { get; set; }
    public string FighterLName { get; set; }
    // Add other properties as needed
}

public class RoundCsv
{
    public string RoundId { get; set; }
    public string FightId { get; set; }
    public string FighterId { get; set; }
    public int Round { get; set; }
    // Add other properties as needed
}
```

## Pagination Implementation (PRIORITY)

The frontend now supports pagination for the fights endpoint. You need to:

### 1. Add Pagination Support to GET /fights

**Current Frontend Expectation**:
```csharp
public class PaginatedFights
{
    public List<FightCsv> Fights { get; set; } = new List<FightCsv>();
    public int TotalCount { get; set; }
}
```

**Query Parameters to Support**:
- `pageNumber` (int, default: 1)
- `pageSize` (int, default: 10)
- `eventId` (string, optional)
- `fighterId` (string, optional)
- `dateFrom` (DateTime, optional)
- `dateTo` (DateTime, optional)
- `weightClass` (string, optional)
- `round` (int, optional)
- `result` (string, optional)
- `titleFight` (bool, optional)
- `referee` (string, optional)
- `sortBy` (string, default: "eventdate")
- `order` (string, default: "desc")

**Expected Response Format**:
```json
{
  "fights": [
    {
      "fightId": "1",
      "eventId": "event1",
      "fighter1Id": "fighter1",
      // ... other properties
    }
  ],
  "totalCount": 1500
}
```

### 2. Implementation Pattern

```csharp
[HttpGet]
public async Task<ActionResult<PaginatedFights>> GetFights(
    int pageNumber = 1,
    int pageSize = 10,
    string? eventId = null,
    string? weightClass = null,
    // ... other filter parameters
    string sortBy = "eventdate",
    string order = "desc")
{
    var query = _context.Fights.AsQueryable();

    // Apply filters
    if (!string.IsNullOrEmpty(eventId))
        query = query.Where(f => f.EventId == eventId);

    if (!string.IsNullOrEmpty(weightClass))
        query = query.Where(f => f.WeightClass.Contains(weightClass));

    // Add other filters...

    // Get total count before pagination
    var totalCount = await query.CountAsync();

    // Apply sorting
    query = sortBy.ToLower() switch
    {
        "eventdate" => order == "desc" ? query.OrderByDescending(f => f.Event.EventDate) : query.OrderBy(f => f.Event.EventDate),
        "weightclass" => order == "desc" ? query.OrderByDescending(f => f.WeightClass) : query.OrderBy(f => f.WeightClass),
        _ => query.OrderByDescending(f => f.Event.EventDate)
    };

    // Apply pagination
    var fights = await query
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .Select(f => new FightCsv
        {
            FightId = f.FightId,
            EventId = f.EventId,
            // ... map other properties
        })
        .ToListAsync();

    return new PaginatedFights
    {
        Fights = fights,
        TotalCount = totalCount
    };
}
```

## Development Commands

```bash
# Build the API
dotnet build

# Run the API (typically on https://localhost:5001)
dotnet run

# Run with specific environment
dotnet run --environment Development

# Apply database migrations (if using EF)
dotnet ef database update
```

## CORS Configuration

Ensure CORS is configured for the frontend domain:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy =>
    {
        policy.WithOrigins("http://localhost:5227", "https://localhost:7227")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

## Database Schema

Document your database schema here or reference schema files.

## Testing

- Use Postman/Swagger for API testing
- Test pagination with various page sizes
- Verify filtering works correctly
- Test CORS from frontend domain

## Current Issues to Address

1. **PRIORITY**: Implement server-side pagination for `/fights` endpoint
2. Ensure all endpoints return data in CSV model format expected by frontend
3. Verify CORS configuration allows frontend requests
4. Test all CRUD operations work correctly

## Deployment Notes

- API is deployed at `https://sqlapi.nedevans.au`
- Frontend expects this exact URL
- Ensure SSL/HTTPS is properly configured
- Database connection strings should be in configuration

## Important Notes

- **Data Format**: All responses must match the `*Csv` model format expected by frontend
- **Error Handling**: Return appropriate HTTP status codes and error messages
- **Validation**: Implement proper input validation for all endpoints
- **Performance**: Implement proper indexing for pagination queries