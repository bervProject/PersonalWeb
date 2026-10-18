# AGENTS.md

This file provides guidance to agents when working with code in this repository.

## Coding Rules (Non-Obvious)

- **Dual controller surface**: Every entity needs both an OData controller (`Controllers/OData/`, extends `DefaultController`) and a REST controller (`Controllers/`, extends `DefaultRestController`). Adding only one is incomplete.
- **`Program.cs` partial class**: The `public partial class Program { }` at the end of `PersonalWeb.Api/Program.cs` is required for `WebApplicationFactory<Program>` in integration tests — do not remove it.
- **No implicit usings in `PersonalWeb.Api`**: Unlike AppHost/MigrationService, `PersonalWeb.Api.csproj` does NOT enable `ImplicitUsings`. All `using` directives must be written explicitly.
- **Soft delete only**: Never call `_dbContext.Remove()`. `Delete()` in `DefaultModelRepositories` sets `IsDeleted = true`. All queries go through `GetAll()` which filters `!x.IsDeleted`.
- **Throw `EntityNotFoundException`, not HTTP results from repositories**: Repositories throw `EntityNotFoundException` (from `PersonalWeb.Api/Exceptions/`); controllers map this to HTTP responses.
- **Models are records**: Entity classes are `record` types, not `class`. Keep this consistent for new entities.
- **Frontend proxy env var**: The dev proxy in `setupProxy.js` uses `process.env.services__apiservice__apiservice__0` — this is injected by Aspire at runtime, not a `.env` file variable.
- **Frontend tests from `PersonalWeb.Admin/`**: Jest and `yarn test` must be run from the `PersonalWeb.Admin/` directory. Running from repo root will fail.
- **Integration tests need a live PostgreSQL**: The `PersonalWeb.Api.Integration.Test` project connects to a real Postgres. Set `ConnectionStrings__Default` env var or ensure `appsettings.Development.json` points to a running instance.
- **DB migrations via CLI before running integration tests**: Run `dotnet ef database update --project PersonalWeb.Api` before executing integration tests locally.
