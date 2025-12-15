# StonksAppWithLogs

A small sample stock order application with logging and a WebAPI front-end. The solution demonstrates a layered architecture using .NET 10 and C# 14, with separate projects for core domain logic, infrastructure (EF Core), a Web API, and unit tests.

Projects
- `StonksAppWithLogs.Core` — Domain entities, DTOs, service contracts and service implementations.
- `StonksAppWithLogs.Infrastructure` — EF Core DbContext, repository implementations and migrations.
- `StonksAppWithLogs.WebAPI` — ASP.NET Core Web API that exposes endpoints for placing and listing orders and querying external stock profile data.
- `Tests` — Unit tests for controllers and services (uses xUnit, Moq, AutoFixture).

Prerequisites
- .NET 10 SDK
- (Optional) SQL Server or another database supported by EF Core for running migrations and the app's persistence layer.

Quick start
1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd StonksAppWithLogs
   ```

2. Configure settings:
   - Set your database connection string in `StonksAppWithLogs.WebAPI/appsettings.json` or use `dotnet user-secrets` for local development.
   - If using the Finnhub integration, provide the API key via configuration (appsettings or user secrets). The exact configuration key is defined in `StonksAppWithLogs.Core.Domain.Options.FinnhubOptions`.

3. Apply EF Core migrations (optional if you don't need persistence):
   ```bash
   dotnet ef database update --project StonksAppWithLogs.Infrastructure --startup-project StonksAppWithLogs.WebAPI
   ```

4. Build and run the Web API:
   ```bash
   dotnet build
   dotnet run --project StonksAppWithLogs.WebAPI
   ```

5. Run tests:
   ```bash
   dotnet test
   ```

Core behaviors
- Place buy/sell orders via POST endpoints exposed by the Web API. The API validates requests and maps DTOs to domain entities.
- Repository implementations persist orders using EF Core; repository methods return domain entities which are mapped back to DTO responses.
- Services contain business logic and mapping between DTOs and entities.

Testing
- Unit tests use xUnit, Moq and AutoFixture. Controller tests mock the stocks service and external Finnhub service. Service tests mock repository contracts to validate service behavior.

Contributing
- Open an issue or submit a PR. Follow standard .NET conventions and add tests for new behavior.

License
- MIT (or update with an explicit license file if required).

For detailed architecture and per-project responsibilities see `DESCRIPTION.md`.