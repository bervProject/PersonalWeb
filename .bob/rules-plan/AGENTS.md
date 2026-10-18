# AGENTS.md

This file provides guidance to agents when working with code in this repository.

## Architecture Constraints (Non-Obvious)

- **Dual controller pattern is intentional**: The OData surface (`Controllers/OData/`) uses `[EnableQuery]`, `Delta<T>` for PATCH, and OData-specific semantics. The REST surface (`Controllers/`) uses standard `[HttpGet/Post/Put/Delete]` attributes and is simpler. Adding a new entity requires both controllers.
- **Repository layer is mandatory**: Controllers never access `DbContext` directly. All data access goes through `IDefaultModelRepositories<TEntity>` / `DefaultModelRepositories<TEntity, TContext>`. Raw EF queries in controllers would break the pattern.
- **`DefaultModel` is the single base record**: All entities inherit the abstract `DefaultModel` record, which brings in both `IAuditable` and `IDraftable`. There is no separate audit log table — audit fields are columns on each entity table.
- **Soft delete is system-wide**: `GetAll()` always filters `IsDeleted = false`. Any new query that bypasses `GetAll()` will expose soft-deleted records unintentionally.
- **MigrationService controls schema evolution**: EF migrations are authored in `PersonalWeb.Api/Migrations/` but applied by `PersonalWeb.MigrationService` at startup. The API itself does not run migrations. In Aspire, the API waits for migration completion (`WaitForCompletion(migration)`).
- **Integration tests are stateful**: Each `IClassFixture<WebApplicationFactory<Program>>` test class shares a single factory (and DB connection). `Dispose()` must call `ExecuteDelete()` to clean up; tests that don't implement `IDisposable` leave data that can affect later tests.
- **Aspire wires the frontend URL via env**: The React admin app's API URL is not hardcoded — it comes from `services__apiservice__apiservice__0` injected by Aspire. In standalone mode (without Aspire), the proxy won't resolve and the frontend will fail to reach the API.
