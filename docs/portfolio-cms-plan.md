# Portfolio CMS — Implementation Plan

## Overview

Evolve the existing `PersonalWeb` backend and CMS admin into the full feature set described in **SRS v3**. The codebase already has the skeleton (DefaultModel, OData + REST dual-controller pattern, repository layer, EF Core + PostgreSQL, React-Admin frontend). Work is structured as independent sub-tasks delivered in dependency order.

**Design Decisions (confirmed):**
1. **Auth integration tests** — existing integration tests that hit mutation endpoints will be updated to authenticate first (obtain a JWT from `/api/auth/login`) rather than bypassing auth.
2. **Blog vs Publication** — `Blog` is kept as-is (link aggregator / RSS-style). `Publication` is the new entity storing summary + external URL — acting as an RSS host for articles published on dev.to or elsewhere. `ContentHtml` is dropped in favour of a `Summary` field; `ExternalUrl` remains.
3. **Experience field renames** — since there is no existing database to preserve, the `Experience` model will be renamed to match the SRS (`CompanyName` / `RoleTitle`) with a fresh consolidated migration. The old field names (`EntityName`, `Title`, etc.) are removed.
4. **React-Admin auth** — ST-7 will also wire up `authProvider` in the Admin frontend so the CMS login screen calls `/api/auth/login`.

**In scope (SRS-derived):**
- Two new backend entities: **Engagement** (speaking/mentorship) and **Publication** (RSS-style article host)
- **Rename/restructure** existing **Experience** model to SRS-aligned field names (`CompanyName`, `RoleTitle`, `IsCurrent`, `TechnologiesUsed`)
- Schema updates to existing **Project** model (add `Slug`, `TechStack`, `LiveUrl`, `IsPublished`, `OrderIndex`)
- New **Profile** entity (global bio/config: `FullName`, `Titles`, `Bio`, `ResumeUrl`)
- Authentication: **User** entity with password-hash login, JWT access token + HttpOnly refresh token, admin role middleware
- Frontend CMS admin: new resource views for Engagement and Publication; update Experience and Project views for new fields; add Profile config page; wire `authProvider` login
- REST endpoints for all entities (dual OData + REST controller pattern)
- Swagger/OpenAPI annotations on all new endpoints

**Out of scope (infrastructure, not in this plan):**
- AWS deployment, Terraform IaC
- Public-facing portfolio frontend (Next.js/React public site)
- Automated dev.to pull/sync (read-only aggregation from dev.to API — separate future integration)

---

## Sub-Tasks

---

### ST-1 — Profile Entity (Backend)

**Status:** `[x] done`

**Intent:** Add the `Profile` entity to store the owner's global bio and title configuration (used by the Hero section of the public portfolio). This is a standalone new entity with no dependencies on other changes.

**Expected Outcomes:**
- `Profile` record type in `PersonalWeb.Api/Models/`
- `ProfileRepositories` concrete repository
- `ProfilesController` (OData) and `ProfilesRestController` (REST) registered
- `Profile` DbSet added to `PersonalWebApiContext`
- EF migration creating the `Profiles` table
- DI registered in `Program.cs`
- OData EDM entity set registered

**Fields (from SRS §5):**
| Property | Type | Constraints |
|---|---|---|
| `FullName` | string | max 512, Required |
| `Titles` | string | max 2048, Required — JSON array serialized as string (e.g. `["Software Engineer","AWS Community Builder","IBM Champion"]`) |
| `Bio` | string | no max (TEXT) |
| `ResumeUrl` | string | max 2048, nullable |

**Todo List:**
1. Create `PersonalWeb.Api/Models/Profile.cs` extending `DefaultModel`
2. Create `PersonalWeb.Api/Repositories/ProfileRepositories.cs`
3. Add `DbSet<Profile> Profiles` to `PersonalWebApiContext`
4. Add EF migration (`dotnet ef migrations add AddProfile --project PersonalWeb.Api`)
5. Create `PersonalWeb.Api/Controllers/OData/ProfilesController.cs` extending `DefaultController<Profile, ProfileRepositories>`
6. Create `PersonalWeb.Api/Controllers/ProfilesRestController.cs` extending `DefaultRestController<Profile, ProfileRepositories>`
7. Register `ProfileRepositories` as scoped in `Program.cs`
8. Register `Profiles` entity set in the OData EDM builder in `Program.cs`

**Relevant Context:**
- Pattern reference: [`PersonalWeb.Api/Models/Blog.cs`](PersonalWeb.Api/Models/Blog.cs), [`PersonalWeb.Api/Controllers/OData/BlogsController.cs`](PersonalWeb.Api/Controllers/OData/BlogsController.cs), [`PersonalWeb.Api/Controllers/BlogsRestController.cs`](PersonalWeb.Api/Controllers/BlogsRestController.cs)
- Repository pattern: [`PersonalWeb.Api/Repositories/BlogRepositories.cs`](PersonalWeb.Api/Repositories/BlogRepositories.cs)
- DI + OData registration: [`PersonalWeb.Api/Program.cs`](PersonalWeb.Api/Program.cs)

---

### ST-2 — Engagement Entity (Backend)

**Status:** `[x] done`

**Intent:** Add the `Engagement` entity to replace the current overloaded `Experience` model for speaking and mentorship events. This covers the "Event & Speaking Management" and "Mentorship Tracking" requirements from SRS §3.1.

**Expected Outcomes:**
- `Engagement` record type in `PersonalWeb.Api/Models/`
- `EngagementRepositories` concrete repository
- `EngagementsController` (OData) and `EngagementsRestController` (REST)
- `Engagement` DbSet added to `PersonalWebApiContext`
- EF migration creating the `Engagements` table
- DI registered in `Program.cs`

**Fields (from SRS §5):**
| Property | Type | Constraints |
|---|---|---|
| `EventName` | string | max 1024, Required |
| `EngagementType` | string | max 256, Required — e.g. "Speaker", "Mentor", "Instructor" |
| `TopicOrRole` | string | max 1024 |
| `EventDate` | DateTimeOffset | Required |
| `Location` | string | max 2048 |
| `ExternalUrl` | string | max 2048, nullable |

**Todo List:**
1. Create `PersonalWeb.Api/Models/Engagement.cs` extending `DefaultModel`
2. Create `PersonalWeb.Api/Repositories/EngagementRepositories.cs`
3. Add `DbSet<Engagement> Engagements` to `PersonalWebApiContext`
4. Add EF migration (`dotnet ef migrations add AddEngagement --project PersonalWeb.Api`)
5. Create `PersonalWeb.Api/Controllers/OData/EngagementsController.cs`
6. Create `PersonalWeb.Api/Controllers/EngagementsRestController.cs`
7. Register `EngagementRepositories` in `Program.cs` and EDM builder

**Relevant Context:**
- Same pattern as ST-1; use `Experience` as field reference for date/location patterns
- [`PersonalWeb.Api/Models/Experience.cs`](PersonalWeb.Api/Models/Experience.cs)

---

### ST-3 — Publication Entity (Backend)

**Status:** `[x] done`

**Intent:** Add the `Publication` entity as an RSS-style article host. It stores a title, slug, short summary, source tag, and an external link to where the full article lives (dev.to, Medium, etc.). This is separate from the existing `Blog` link aggregator, which is kept unchanged. There is no full rich-text body — the canonical content lives on the external platform.

**Expected Outcomes:**
- `Publication` record type in `PersonalWeb.Api/Models/`
- `PublicationRepositories` concrete repository
- `PublicationsController` (OData) and `PublicationsRestController` (REST)
- `Publication` DbSet added to `PersonalWebApiContext`
- EF migration creating the `Publications` table
- DI registered in `Program.cs`

**Fields (adjusted from SRS §5 — RSS-host pattern):**
| Property | Type | Constraints |
|---|---|---|
| `Title` | string | max 1024, Required |
| `Slug` | string | max 512, Required — unique |
| `Summary` | string | no max (TEXT) — short excerpt shown before linking out |
| `Source` | string | max 64 — e.g. "dev.to", "medium", "native" |
| `ExternalUrl` | string | max 2048, nullable — canonical article URL |
| `CoverImageUrl` | string | max 2048, nullable |
| `PublishedDate` | DateTimeOffset? | nullable |

**Todo List:**
1. Create `PersonalWeb.Api/Models/Publication.cs` extending `DefaultModel`
2. Create `PersonalWeb.Api/Repositories/PublicationRepositories.cs`
3. Add `DbSet<Publication> Publications` to `PersonalWebApiContext`
4. Add EF migration (`dotnet ef migrations add AddPublication --project PersonalWeb.Api`)
5. Create `PersonalWeb.Api/Controllers/OData/PublicationsController.cs`
6. Create `PersonalWeb.Api/Controllers/PublicationsRestController.cs`
7. Register `PublicationRepositories` in `Program.cs` and EDM builder

**Relevant Context:**
- Same pattern as ST-1/ST-2
- `Blog` model is kept unchanged (simple link + icon aggregator); `Publication` is separate
- [`PersonalWeb.Api/Models/Blog.cs`](PersonalWeb.Api/Models/Blog.cs)

---

### ST-4 — Rebuild Experience Model to SRS Schema (Backend)

**Status:** `[x] done`

**Intent:** Since there is no live database to preserve, replace the existing `Experience` model entirely with the SRS-aligned schema. Old field names (`Title`, `EntityName`, `EntityLink`, `RepositoryLink`, `ExperienceType`, `Description`) are removed or renamed to match SRS §5. The existing migration is deleted and a single fresh migration is authored for the full schema.

**Expected Outcomes:**
- `Experience` record has SRS-aligned field names
- The single existing EF migration (`20211009072917_NewBaseMigration`) is deleted and replaced with a new consolidated initial migration that covers all entities in their final form
- `ExperiencesController` and `ExperienceRepositories` require no changes (they are generic)
- `experience.tsx` in the frontend updated to use new field names

**Full Field Set (SRS §5 aligned):**
| Property | Type | Constraints |
|---|---|---|
| `CompanyName` | string | max 1024, Required |
| `RoleTitle` | string | max 1024, Required |
| `Location` | string | max 2048 |
| `StartDate` | DateTimeOffset | Required |
| `EndDate` | DateTimeOffset? | nullable — null implies current |
| `IsCurrent` | bool | Default: false |
| `DescriptionHtml` | string | no max (TEXT) |
| `TechnologiesUsed` | string | max 4096, nullable — JSON array |

> **Note:** `RepositoryLink` and `ExperienceType` from the old model are dropped. If needed for open-source contributions, that is a separate entity concern.

**Todo List:**
1. Replace body of `PersonalWeb.Api/Models/Experience.cs` with the new field set above
2. Delete the existing migration file `20211009072917_NewBaseMigration.cs` (and its `.Designer.cs`)
3. Drop the `PersonalWebApiContext` model snapshot entries for old fields
4. Author a fresh consolidated migration: `dotnet ef migrations add InitialCreate --project PersonalWeb.Api`
   (this migration covers Blogs, Experiences, Projects, Profiles, Engagements, Publications, Users — do after all model changes in ST-1 through ST-6 are applied to the model layer)
5. Update `PersonalWeb.Admin/src/services/experience.tsx` to use new field names
6. Run `dotnet test` to confirm no regressions

**Relevant Context:**
- [`PersonalWeb.Api/Models/Experience.cs`](PersonalWeb.Api/Models/Experience.cs)
- [`PersonalWeb.Api/Migrations/`](PersonalWeb.Api/Migrations/) — delete old migration here
- This task's migration step (step 4) should be executed **after** ST-1, ST-2, ST-3, ST-5, ST-6 model files are created, so the single `InitialCreate` migration captures every entity at once

---

### ST-5 — Update Project Model (Backend)

**Status:** `[x] done`

**Intent:** Bring the existing `Project` model into alignment with SRS §5 ("Projects") by adding `Slug`, `TechStack`, `LiveUrl`, `IsPublished`, and `OrderIndex` fields.

**Expected Outcomes:**
- `Project` model gains 5 new columns
- EF migration adding those columns
- Existing tests still pass

**Fields to Add:**
| Property | Type | Constraints |
|---|---|---|
| `Slug` | string | max 512, nullable |
| `TechStack` | string | max 4096, nullable — JSON array |
| `LiveUrl` | string | max 2048, nullable |
| `IsPublished` | bool | Default: true |
| `OrderIndex` | int | Default: 0 |

**Todo List:**
1. Add the 5 new properties to `PersonalWeb.Api/Models/Project.cs`
2. Add EF migration (`dotnet ef migrations add UpdateProject --project PersonalWeb.Api`)
3. Verify existing `ProjectsController` and `ProjectRepositories` require no changes
4. Run `dotnet test` to confirm no regressions

**Relevant Context:**
- [`PersonalWeb.Api/Models/Project.cs`](PersonalWeb.Api/Models/Project.cs)

---

### ST-6 — User Entity & JWT Authentication (Backend)

**Status:** `[x] done`

**Intent:** Implement the authentication layer defined in SRS §6. Add a `User` entity to store admin credentials, and implement the `/api/auth/login` endpoint that returns a short-lived JWT access token and sets a long-lived HttpOnly refresh token cookie. Add authorization middleware so that CMS mutation routes (POST/PATCH/PUT/DELETE) on all OData controllers require a valid JWT with an "admin" role claim. Existing integration tests that call mutation endpoints are updated to authenticate first.

**Expected Outcomes:**
- `User` record type (not inheriting `DefaultModel` — it has its own auth-specific schema)
- `POST /api/auth/login` REST endpoint returning `{ accessToken }` and setting `refreshToken` cookie
- All OData and REST mutation endpoints return `401`/`403` when no valid JWT is present
- `GET` (read) endpoints remain public
- Existing integration tests updated to call `/api/auth/login` and pass the JWT `Authorization: Bearer` header on mutation requests
- `dotnet test` passes

**User Entity Fields (from SRS §5):**
| Property | Type | Constraints |
|---|---|---|
| `Id` | Guid | PK |
| `Email` | string | max 256, Required, unique |
| `PasswordHash` | string | max 512, Required — Bcrypt hash |
| `Role` | string | max 64, Default: "admin" |
| `CreatedAt` | DateTimeOffset | |
| `UpdatedAt` | DateTimeOffset? | |

**Auth Endpoints (SRS §7):**
- `POST /api/auth/login` — body: `{ email, password }`, response: `{ accessToken }` + HttpOnly `refreshToken` cookie

**Todo List:**
1. Add `Microsoft.AspNetCore.Authentication.JwtBearer` and `BCrypt.Net-Next` NuGet packages to `PersonalWeb.Api`
2. Create `PersonalWeb.Api/Models/User.cs` (standalone class, not `DefaultModel`)
3. Add `DbSet<User> Users` to `PersonalWebApiContext`
4. Add EF migration (`dotnet ef migrations add AddUser --project PersonalWeb.Api`)
5. Create `PersonalWeb.Api/Controllers/AuthController.cs` with `POST /api/auth/login`
6. Implement JWT generation (access token: 15 min, RS256 or HS256; refresh token: 7 days, HttpOnly cookie)
7. Register `AddAuthentication().AddJwtBearer(...)` and `AddAuthorization()` in `Program.cs`
8. Add `[Authorize(Roles = "admin")]` to all mutation actions in `DefaultController<,>` (OData) and `DefaultRestController<,>` (REST)
9. Keep GET/read actions unauthenticated (`[AllowAnonymous]`)
10. Add JWT config (secret/issuer/audience) to `appsettings.json`; store secret in user-secrets for development
11. Seed a default admin user (email + bcrypt hash) in a data seed helper or via a migration — required so integration tests can authenticate
12. Update all existing integration test classes that call POST/PATCH/DELETE to first POST to `/api/auth/login` and attach the returned `accessToken` as a Bearer header
13. Run `dotnet test` to confirm all tests pass

**Relevant Context:**
- [`PersonalWeb.Api/Controllers/OData/DefaultController.cs`](PersonalWeb.Api/Controllers/OData/DefaultController.cs) — add `[Authorize]` to Post/Patch/Delete
- [`PersonalWeb.Api/Controllers/DefaultRestController.cs`](PersonalWeb.Api/Controllers/DefaultRestController.cs) — add `[Authorize]` to Post/Put/Delete
- [`PersonalWeb.Api/Program.cs`](PersonalWeb.Api/Program.cs)
- Note: `ImplicitUsings` is NOT enabled in `PersonalWeb.Api` — explicit `using` statements required

---

### ST-7 — Admin Frontend: Auth Provider + New & Updated Resource Views

**Status:** `[x] done`

**Intent:** Update the React-Admin CMS dashboard to: (a) wire a login screen via `authProvider` calling `/api/auth/login`; (b) expose the new entities (Engagement, Publication, Profile); (c) update Experience and Project views for renamed/new fields.

**Expected Outcomes:**
- `PersonalWeb.Admin/src/authProvider.ts` — implements react-admin `AuthProvider` interface, calls `POST /api/auth/login`, stores JWT in `localStorage`, attaches it as `Authorization: Bearer` header on all API requests
- `PersonalWeb.Admin/src/services/engagement.tsx` — List, Show, Create, Edit for Engagement
- `PersonalWeb.Admin/src/services/publication.tsx` — List, Show, Create, Edit for Publication (Summary, ExternalUrl, Source, CoverImageUrl, PublishedDate)
- `PersonalWeb.Admin/src/services/profile.tsx` — List, Show, Create, Edit for Profile
- `PersonalWeb.Admin/src/services/experience.tsx` updated to use renamed fields (`CompanyName`, `RoleTitle`, `DescriptionHtml`, `TechnologiesUsed`, `IsCurrent`)
- `PersonalWeb.Admin/src/services/portofolio.tsx` updated to add `Slug`, `TechStack`, `LiveUrl`, `IsPublished`, `OrderIndex`
- `authProvider` passed to `<Admin>` in `App.tsx`
- All 3 new resources registered in `App.tsx`
- `yarn test` passes

**Todo List:**
1. Create `PersonalWeb.Admin/src/authProvider.ts` implementing `login`, `logout`, `checkAuth`, `checkError`, `getPermissions` for react-admin
2. Update `App.tsx` to pass `authProvider` to the `<Admin>` component
3. Create `PersonalWeb.Admin/src/services/engagement.tsx`
4. Create `PersonalWeb.Admin/src/services/publication.tsx` (fields: Title, Slug, Summary, Source, ExternalUrl, CoverImageUrl, PublishedDate)
5. Create `PersonalWeb.Admin/src/services/profile.tsx`
6. Update `PersonalWeb.Admin/src/services/experience.tsx` with renamed fields
7. Update `PersonalWeb.Admin/src/services/portofolio.tsx` with new fields
8. Register `engagements`, `publications`, `profiles` as `<Resource>` entries in `App.tsx`
9. Run `yarn test` from `PersonalWeb.Admin/`

**Relevant Context:**
- [`PersonalWeb.Admin/src/services/blog.tsx`](PersonalWeb.Admin/src/services/blog.tsx) — canonical pattern for a service file
- [`PersonalWeb.Admin/src/App.tsx`](PersonalWeb.Admin/src/App.tsx) — where resources are registered
- OData resource name must match the EntitySet name registered in `Program.cs` (e.g. `Engagements`)
- [`PersonalWeb.Admin/jest.config.ts`](PersonalWeb.Admin/jest.config.ts) — test config, axios mapped to CJS

---

### ST-8 — Integration Tests for New Entities

**Status:** `[x] done`

**Intent:** Add integration tests for the three new backend entities (Profile, Engagement, Publication) following the existing test patterns, ensuring CRUD flows work end-to-end against a real PostgreSQL instance. Mutation calls include a valid JWT obtained by authenticating as the seeded admin user (wired in ST-6).

**Expected Outcomes:**
- `PersonalWeb.Api.Integration.Test/ProfileTest.cs`
- `PersonalWeb.Api.Integration.Test/EngagementTest.cs`
- `PersonalWeb.Api.Integration.Test/PublicationTest.cs`
- All tests pass via `dotnet test`

**Todo List:**
1. Study existing test class (e.g. `BlogTest.cs` or `ExperienceTest.cs`) for the `IClassFixture`, `IDisposable`, `ODataListResponse<T>` pattern
2. Add a shared `AuthHelper` (or base class) that calls `POST /api/auth/login` with the seeded admin credentials and returns a `Bearer` token for injection into mutation requests
3. Create `ProfileTest.cs` — test GET list, GET by id, POST (authenticated), PATCH (authenticated), DELETE (authenticated, soft-delete)
4. Create `EngagementTest.cs` — same coverage
5. Create `PublicationTest.cs` — same coverage
6. Ensure each test class implements `IDisposable` calling `ExecuteDelete()` to clean up
7. Run `dotnet test --filter "FullyQualifiedName~ProfileTest"` etc. to confirm

**Relevant Context:**
- [`PersonalWeb.Api.Integration.Test/`](PersonalWeb.Api.Integration.Test/) — existing test classes
- [`PersonalWeb.Api.Integration.Test/Models/ODataListResponse.cs`](PersonalWeb.Api.Integration.Test/Models/) — response wrapper
- AGENTS.md: connection string from `ConnectionStrings__Default` env var or `appsettings.Development.json`
