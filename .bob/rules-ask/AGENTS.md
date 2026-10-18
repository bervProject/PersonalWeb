# AGENTS.md

This file provides guidance to agents when working with code in this repository.

## Documentation Context (Non-Obvious)

- **Two API layers**: The API project exposes both OData (`/v1/`) and a plain REST layer (`/api/[controller]/[action]`). They are independent controller hierarchies sharing the same repositories. Documentation or questions about one don't necessarily apply to the other.
- **Frontend is CRA, not Vite**: Despite the project being modern (React 19), `PersonalWeb.Admin` uses Create React App (`react-scripts`), not Vite. Vite-specific docs don't apply.
- **`ra-data-odata-server`**: The frontend uses an OData-specific data provider for react-admin (`ra-data-odata-server`), not the standard REST or JSON API providers. Query/filter semantics follow OData conventions.
- **Aspire SDK version mismatch in csproj**: `PersonalWeb.AppHost.csproj` declares `Aspire.AppHost.Sdk` at version `9.5.2` but the hosting packages reference `13.x`. This is intentional — the SDK and hosting packages have independent versioning.
- **`MigrationService` is the production migration runner**: EF migrations are not auto-applied at API startup. The `PersonalWeb.MigrationService` worker is the designed mechanism and is wired in Aspire with `WithExplicitStart()` and `WaitForCompletion`.
- **OData route base is `/v1/`**: All OData endpoints are under `/v1/EntitySetName` (e.g., `/v1/Blogs`). The REST endpoints are under `/api/Blogs/Get`, `/api/Blogs/Post`, etc.
