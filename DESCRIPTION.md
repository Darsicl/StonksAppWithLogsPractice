Solution Description

Overview
--------
This solution implements a minimal stock order management system called `StonksAppWithLogs`. It demonstrates common architectural patterns used in .NET applications:

- Layered architecture: separation of concerns into `Core`, `Infrastructure`, `WebAPI`, and `Tests` projects.
- Dependency injection: services and repositories are injected into controllers and other services.
- DTOs and mapping: DTOs (`BuyOrderRequest`, `BuyOrderResponse`, etc.) are used for API boundaries and are mapped to domain entities.
- Persistence with EF Core: `StockMarketDbContext` manages `BuyOrders` and `SellOrders` and repository implementations use `DbContext`.
- External API integration: a `FinnhubService`/repository is included to fetch company profile information from the Finnhub API.

Project responsibilities
------------------------
- `StonksAppWithLogs.Core`
  - Domain entities (e.g., `BuyOrder`, `SellOrder`).
  - DTOs for requests and responses.
  - Service contracts (`IStocksService`, `IFinnhubService`).
  - Service implementations that implement business logic (e.g., `StocksService`).

- `StonksAppWithLogs.Infrastructure`
  - EF Core `DbContext` and repository implementations (`StocksRepository`, `FinnhubRepository`).
  - Database migrations.

- `StonksAppWithLogs.WebAPI`
  - ASP.NET Core controllers exposing endpoints for placing orders and querying stock information.
  - Application startup and DI configuration.

- `Tests`
  - Unit tests using xUnit, Moq, and AutoFixture to validate controller and service behavior.

Key workflows
-------------
- Creating an order (buy/sell)
  1. Controller receives `BuyOrderRequest` or `SellOrderRequest`.
  2. Controller validates model state and forwards the request to `IStocksService`.
  3. `StocksService` maps request to domain entity, assigns an ID, and calls `IStocksRepository` to persist.
  4. The created domain entity is mapped back to a `BuyOrderResponse`/`SellOrderResponse` and returned.

- Listing orders
  1. Controller calls `IStocksService.GetBuyOrders()` or `GetSellOrders()`.
  2. Service calls repository methods to retrieve entities and maps them to DTO responses.

Testing
-------
- Controller tests mock service contracts and validate HTTP responses and behavior.
- Service tests mock repositories and validate mapping and interactions (including null argument checks and that GUIDs are assigned).

Extensibility
-------------
- Add more business rules to `StocksService` (e.g., validating available funds, matching orders) and add corresponding tests.
- Replace `IStocksRepository` implementation for alternative storage (e.g., in-memory, NoSQL) without touching business logic.

Running locally
---------------
See `README.md` for quick start and running tests.

Contact
-------
This is a sample/demo project; adjust configuration and secrets before using in production.