# AGENTS.md

This file provides guidance to agents when working with code in this repository.

## Stack

- **Backend**: .NET 10, ASP.NET Core Web API, EF Core 10 + Npgsql (PostgreSQL), OData v4 (Microsoft.AspNetCore.OData 9.x), Swagger
- **Frontend**: React 19 + react-admin 5 + `ra-data-odata-server` (OData provider), TypeScript, Jest + ts-jest, Yarn (v1.22)
- **Orchestration**: .NET Aspire 13 (`PersonalWeb.AppHost`) — Aspire SDK version 9.5.2 is in the AppHost csproj despite referencing Aspire.Hosting packages at 13.x
- **DB migrations**: Separate `PersonalWeb.MigrationService` worker project (not EF CLI in prod); run migrations manually via `dotnet ef database update --project PersonalWeb.Api`

## Commands

### .NET (run from repo root)
```
dotnet build
dotnet test                                          # all tests
dotnet test --filter "FullyQualifiedName~<TestName>" # single test
dotnet run --project PersonalWeb.AppHost             # start Aspire (preferred dev entry)
dotnet ef database update --project PersonalWeb.Api  # apply migrations
```

Integration tests (`PersonalWeb.Api.Integration.Test`) require a real PostgreSQL instance. The connection string is read from `ConnectionStrings__Default` env var (set in CI) or `appsettings.Development.json`.

### Frontend (must run from `PersonalWeb.Admin/`)
```
yarn install
yarn test           # Jest with coverage
yarn test --testPathPattern=<pattern>  # single test file
yarn build
yarn start
```

The AppHost auto-runs `yarn install` if `node_modules` is missing (via MSBuild `RestoreNpm` target). The frontend uses `react-scripts` (CRA), **not** Vite.

## Architecture

Two parallel API surface areas exist for every entity — **do not collapse them**:
- `Controllers/OData/` — OData controllers extending `DefaultController<TEntity, TRepository>` (inherits `ODataController`, uses `[EnableQuery]`, `Delta<T>` for PATCH, registered under route `/v1/`)
- `Controllers/` (non-OData) — REST controllers extending `DefaultRestController<TEntity, TRepository>` (inherits `ControllerBase`, route pattern `/api/[controller]/[action]`)

The frontend admin (`PersonalWeb.Admin`) talks to `/api` (non-OData REST via the OData data provider through a dev proxy), which is proxied to `/v1` of the API service. The proxy target is injected via Aspire's `services__apiservice__apiservice__0` env var in [`setupProxy.js`](PersonalWeb.Admin/src/setupProxy.js).

## Critical Patterns

### Models
- All entity models are `record` types extending `DefaultModel` (abstract record).
- `DefaultModel` implements both `IAuditable` and `IDraftable`.
- `IAuditable` mandates: `Id` (Guid), `CreateBy` (Required), `CreatedDate` (Required), `UpdatedBy`, `UpdatedDate`, `IsDeleted`, `DeletedBy`, `DeletedDate`.
- Soft-delete is enforced: `GetAll()` always filters `!x.IsDeleted`. Hard deletes do not exist — `Delete()` sets `IsDeleted = true`.

### Repositories
- All repositories extend `DefaultModelRepositories<TEntity, TContext>` and implement `IDefaultModelRepositories<TEntity>`.
- Concrete repository classes (e.g., `BlogRepositories`) inherit `DefaultModelRepositories` with `PersonalWebApiContext`; they contain no additional logic unless overridden.
- Throw `EntityNotFoundException` (not 404 directly) when an entity is not found — the exception is defined in `PersonalWeb.Api/Exceptions/`.

### Integration Tests
- Use `WebApplicationFactory<Program>` — requires the `public partial class Program { }` declaration at the bottom of [`Program.cs`](PersonalWeb.Api/Program.cs).
- Each test class implementing `IDisposable` cleans up via `ExecuteDelete()` on the DbSet in `Dispose()`.
- The `ODataListResponse<T>` wrapper type used in integration tests lives in `PersonalWeb.Api.Integration.Test/Models/`.

### Frontend Services
- Each entity has a dedicated file in `PersonalWeb.Admin/src/services/` exporting `List`, `Show`, `Edit`, `Create` components for react-admin.
- The OData data provider is initialized asynchronously in `App.tsx`; the app renders `<Loading>` until the provider resolves.

## Code Style

### C#
- Namespace per folder, matching project name prefix (`PersonalWeb.Api.Controllers`, etc.)
- `ImplicitUsings` and `Nullable` are enabled only in `PersonalWeb.AppHost` and `PersonalWeb.MigrationService` — **not** in `PersonalWeb.Api`; explicit `using` statements are required there.
- `[Required]` + `[MaxLength]` data annotations on model properties for validation (enforced by OData controller's `ModelState.IsValid` check).

### TypeScript/React
- Strict TypeScript (`noImplicitAny`, `strictNullChecks`, `noImplicitReturns`).
- `tsconfig.json` targets `es5` / `commonjs` (Jest compatibility), but the project `"type": "module"` is set in `package.json`.
- Jest uses `ts-jest` for `.ts/.tsx` and `babel-jest` for `.js/.jsx`; CSS imports are mocked via `identity-obj-proxy` (must be a dev dependency if added).
- axios module must be mapped to `axios/dist/node/axios.cjs` in Jest config (already set in [`jest.config.ts`](PersonalWeb.Admin/jest.config.ts)).
