# PersonalWeb.Admin — CRA to Vite Migration Plan

## Overview

**Goal:** Replace the Create React App (CRA / `react-scripts`) toolchain in `PersonalWeb.Admin` with Vite, and replace Jest + ts-jest + babel-jest with Vitest. Re-enable and update the Aspire NpmApp integration to work with Vite's dev-server conventions.

**Scope:**
- `PersonalWeb.Admin/` — all build, test, and configuration files
- `PersonalWeb.AppHost/Program.cs` — re-enable and adjust the NpmApp resource

**Non-goals:**
- No changes to React components or business logic in `src/services/`
- No changes to the API (`PersonalWeb.Api`)
- No changes to integration tests (`PersonalWeb.Api.Integration.Test`)
- No migration of the auth flow or data provider logic
- No migration to Vitest for end-to-end tests (there are none at present)

---

## Sub-Tasks

---

### Sub-Task 1: Replace Build Dependencies

**Intent:** Swap CRA and all Jest/Babel tooling for Vite and Vitest equivalents in `package.json`.

**Expected Outcomes:**
- `react-scripts` is removed
- `vite`, `@vitejs/plugin-react`, `vitest`, `@vitest/coverage-v8`, and `@testing-library/react` (if not present) are added
- Babel packages (`@babel/preset-env`, `babel-jest`, `babel.config.js`) are removed
- `ts-jest`, `jest`, `jest-environment-jsdom` are removed
- npm scripts are updated: `start` → `vite`, `build` → `vite build`, `test` → `vitest run`
- `yarn install` runs cleanly with the new dependency set

**Todo List:**
1. In `package.json`, remove from `devDependencies`: `react-scripts`, `ts-jest`, `babel-jest`, `@babel/preset-env`, `jest-environment-jsdom`
2. In `package.json`, remove from `dependencies` or `devDependencies`: `jest`, `@types/jest`
3. Add to `devDependencies`: `vite`, `@vitejs/plugin-react`, `vitest`, `@vitest/coverage-v8`, `jsdom`
4. Update `scripts`: `"start": "vite"`, `"build": "vite build"`, `"preview": "vite preview"`, `"test": "vitest run"`
5. Delete `babel.config.js`
6. Run `yarn install`

**Relevant Context:**
- [`PersonalWeb.Admin/package.json`](PersonalWeb.Admin/package.json)
- [`PersonalWeb.Admin/babel.config.js`](PersonalWeb.Admin/babel.config.js)
- The project uses Yarn 1.22; keep using Yarn

**Status:** `[x] done`

---

### Sub-Task 2: Create `vite.config.ts`

**Intent:** Add the Vite configuration file that replaces both the hidden CRA webpack config and `setupProxy.js`. The proxy target must read the Aspire-injected env var `services__apiservice__apiservice__0` at runtime (only meaningful during Aspire-driven dev) and fall back to a local default.

**Expected Outcomes:**
- `vite.config.ts` exists at `PersonalWeb.Admin/` with React plugin, path alias `@`, test configuration, and proxy rule
- `src/setupProxy.js` is deleted (it only works with `react-scripts`)
- `jest.config.ts` is deleted (replaced by the `test` block inside `vite.config.ts`)
- `setup.jest.js` is renamed / migrated to `setup.vitest.ts` if still needed

**Todo List:**
1. Create `PersonalWeb.Admin/vite.config.ts` with:
   - `@vitejs/plugin-react`
   - `resolve.alias` mapping `@` → `./src`
   - `server.port` set to `3000` (matching CRA default; Aspire injects `PORT` so read `process.env.PORT` here — `vite.config.ts` runs in Node and has access to `process.env`)
   - `server.proxy` for `/api` → `${process.env.services__apiservice__apiservice__0}/v1` with `changeOrigin: true` and path rewrite stripping `/api` prefix (because the target already has `/v1`)
   - `test` block: `globals: true`, `environment: 'jsdom'`, `setupFiles: ['./setup.vitest.ts']`, `coverage` from `@vitest/coverage-v8`
2. Create `PersonalWeb.Admin/setup.vitest.ts` (migrate content from `setup.jest.js`: polyfill `TextEncoder`/`TextDecoder`)
3. Delete `PersonalWeb.Admin/src/setupProxy.js`
4. Delete `PersonalWeb.Admin/jest.config.ts`
5. Delete `PersonalWeb.Admin/setup.jest.js`

**Relevant Context:**
- [`PersonalWeb.Admin/src/setupProxy.js`](PersonalWeb.Admin/src/setupProxy.js) — proxy target pattern using `services__apiservice__apiservice__0`
- [`PersonalWeb.Admin/jest.config.ts`](PersonalWeb.Admin/jest.config.ts) — transform config, module name mapper, setup files
- [`PersonalWeb.Admin/setup.jest.js`](PersonalWeb.Admin/setup.jest.js) — TextEncoder/TextDecoder polyfill
- Proxy note: `vite.config.ts` runs in Node at startup, so `process.env` is available for reading the Aspire service URL — no `VITE_` prefix needed for the proxy target

**Status:** `[x] done`

---

### Sub-Task 3: Update `index.html` and Entry Point

**Intent:** Move `public/index.html` to the project root (Vite requirement) and wire it to the app entry point using a `<script type="module">` tag. The CRA-specific `%PUBLIC_URL%` token must be removed.

**Expected Outcomes:**
- `PersonalWeb.Admin/index.html` exists at root (not inside `public/`)
- All `%PUBLIC_URL%` tokens are replaced with `/`
- A `<script type="module" src="/src/index.tsx"></script>` tag is present in `<body>`
- `public/index.html` is deleted

**Todo List:**
1. Copy `PersonalWeb.Admin/public/index.html` to `PersonalWeb.Admin/index.html`
2. Replace all occurrences of `%PUBLIC_URL%` with `/`
3. Rename `PersonalWeb.Admin/src/index.tsx` to `PersonalWeb.Admin/src/main.tsx`
4. Add `<script type="module" src="/src/main.tsx"></script>` before `</body>`
4. Delete `PersonalWeb.Admin/public/index.html`

**Relevant Context:**
- [`PersonalWeb.Admin/public/index.html`](PersonalWeb.Admin/public/index.html)
- Entry point is renamed from `src/index.tsx` to `src/main.tsx` (Vite convention)

**Status:** `[x] done`

---

### Sub-Task 4: Update `tsconfig.json`

**Intent:** Align TypeScript configuration with Vite's ESM-first build pipeline. The existing `commonjs` module and `es5` target are incompatible with Vite's native ESM output.

**Expected Outcomes:**
- `module` is `esnext`
- `target` is `es2020`
- `moduleResolution` is `bundler`
- `jsx` is `react-jsx` (eliminates the need for `import React from 'react'` in every file)
- `types` replaces `"jest"` with `"vitest/globals"`
- `outDir` updated from `build` to `dist`

**Todo List:**
1. In `PersonalWeb.Admin/tsconfig.json`:
   - Change `"module"` from `"commonjs"` to `"esnext"`
   - Change `"target"` from `"es5"` to `"es2020"`
   - Change `"moduleResolution"` from `"node"` to `"bundler"`
   - Change `"jsx"` from `"react"` to `"react-jsx"`
   - In `"types"`, replace `"jest"` with `"vitest/globals"`
   - Change `"outDir"` from `"build"` to `"dist"`
   - Add `"allowSyntheticDefaultImports": true` and `"resolveJsonModule": true` if not present
2. Remove the `jest` and `webpack` entries from `"exclude"` (no longer relevant)

**Relevant Context:**
- [`PersonalWeb.Admin/tsconfig.json`](PersonalWeb.Admin/tsconfig.json)
- TypeScript version is `^6.0.3` — `bundler` moduleResolution is fully supported

**Status:** `[x] done`

---

### Sub-Task 5: Replace `process.env` with `import.meta.env`

**Intent:** Vite does not expose `process.env` to client-side code. All client-side env references must use `import.meta.env`. The proxy config in `vite.config.ts` (Node-side) may still use `process.env` — only `src/` files need to change.

**Expected Outcomes:**
- `src/App.tsx` uses `import.meta.env.VITE_SERVICE_URL` (or the correct Vite-equivalent) instead of `process.env.SERVICE_URL`
- `src/registerServiceWorker.ts` uses `import.meta.env.MODE` instead of `process.env.NODE_ENV` and `import.meta.env.BASE_URL` instead of `process.env.PUBLIC_URL`
- No remaining `process.env` references in `src/` (except any that are intentionally behind a type guard for Node compatibility)

**Todo List:**
1. In `PersonalWeb.Admin/src/App.tsx` line ~138: replace `process.env.SERVICE_URL` with `import.meta.env.VITE_SERVICE_URL`
2. In `PersonalWeb.Admin/src/registerServiceWorker.ts`: replace `process.env.NODE_ENV` with `import.meta.env.MODE`, replace `process.env.PUBLIC_URL` (x2) with `import.meta.env.BASE_URL`
3. Create `PersonalWeb.Admin/.env.development` (or `.env.local`) with `VITE_SERVICE_URL=` empty (proxy handles routing in dev; set only if running without Aspire)
4. Verify no remaining `process.env` in `src/` with a grep pass

**Relevant Context:**
- [`PersonalWeb.Admin/src/App.tsx`](PersonalWeb.Admin/src/App.tsx) — line ~138
- [`PersonalWeb.Admin/src/registerServiceWorker.ts`](PersonalWeb.Admin/src/registerServiceWorker.ts)
- Vite convention: only vars prefixed `VITE_` are exposed to client bundles via `import.meta.env`

**Status:** `[x] done`

---

### Sub-Task 6: Update Aspire AppHost NpmApp Registration

**Intent:** The `AddNpmApp` block in the AppHost is commented out. Re-enable it and ensure it works with Vite's dev server conventions (different default port, different env var for port injection).

**Expected Outcomes:**
- `PersonalWeb.AppHost/Program.cs` has an active `AddNpmApp` block for the `admin` resource
- Vite reads `PORT` from environment to choose its listening port (via `vite.config.ts` `server.port`)
- The admin resource references the API service so Aspire injects `services__apiservice__apiservice__0`
- `BROWSER=none` env var is retained (suppresses browser auto-open)

**Todo List:**
1. In `PersonalWeb.AppHost/Program.cs`, uncomment the `AddNpmApp(...)` block
2. Update the npm script name: use `"start"` (which now runs `vite`)
3. Ensure `.WithEnvironment("BROWSER", "none")` is present
4. Keep `.WithReference(webApi)` so Aspire injects the API service URL
5. Keep `.WithHttpEndpoint(env: "PORT")` — Vite must read `process.env.PORT` in `vite.config.ts` and apply it as `server.port`
6. Keep `.WithExternalHttpEndpoints()` and `.PublishAsDockerFile()`

**Relevant Context:**
- [`PersonalWeb.AppHost/Program.cs`](PersonalWeb.AppHost/Program.cs)
- [`PersonalWeb.Admin/vite.config.ts`](PersonalWeb.Admin/vite.config.ts) — will be created in Sub-Task 2; `server.port` must use `parseInt(process.env.PORT ?? '3000')`

**Status:** `[x] done`

---

### Sub-Task 7: Migrate and Verify Tests

**Intent:** Confirm that existing tests run under Vitest with no failures and with the same coverage as before. The test files themselves are expected to require minimal or no changes because Vitest is Jest-API-compatible.

**Expected Outcomes:**
- `yarn test` runs via Vitest and all existing tests pass
- Coverage report is generated in `coverage/`
- The `axios` module mapper (`axios/dist/node/axios.cjs`) is re-implemented in `vite.config.ts`'s `test.alias` block
- CSS imports mocked via Vitest's built-in CSS handling or explicit alias

**Todo List:**
1. Check existing test files for any Jest-specific globals that need an explicit import under Vitest (unlikely if `globals: true`)
2. In `vite.config.ts` `test` block, add `alias` for `axios` → `axios/dist/node/axios.cjs` (same as the old Jest `moduleNameMapper`)
3. Run `yarn test` and fix any remaining failures
4. Confirm coverage directory is produced

**Relevant Context:**
- [`PersonalWeb.Admin/jest.config.ts`](PersonalWeb.Admin/jest.config.ts) — `moduleNameMapper` and `setupFiles` are the two things that must survive in `vite.config.ts`
- [`PersonalWeb.Admin/setup.jest.js`](PersonalWeb.Admin/setup.jest.js) — migrated to `setup.vitest.ts` in Sub-Task 2

**Status:** `[x] done`

---

### Sub-Task 8: Build Verification

**Intent:** Confirm the production build succeeds and the dev server starts. This is the final gate before the migration is considered complete.

**Expected Outcomes:**
- `yarn build` completes with no TypeScript or Rollup errors, producing output in `dist/`
- `yarn dev` starts the Vite dev server on port 3000 (or `PORT` if set)
- No console errors on page load in the browser
- `yarn test` passes (re-confirmed after build)

**Todo List:**
1. Run `yarn build` from `PersonalWeb.Admin/` — fix any TypeScript errors surfaced by the stricter ESM config
2. Run `yarn dev` — confirm the dev server starts and the proxy to the API resolves
3. Run `yarn test` — confirm all tests pass under Vitest
4. Update `.gitignore` in `PersonalWeb.Admin/` to include `dist/` (if not already present) and remove `build/` if it was the old output directory

**Relevant Context:**
- All previously modified files
- `PersonalWeb.Admin/.gitignore`

**Status:** `[x] done`

---

## Key Design Decisions

| Decision | Choice | Rationale |
|---|---|---|
| Test framework | Vitest (not Jest) | Natively integrates with Vite config; no separate transform pipeline needed |
| Entry point rename | Rename to `main.tsx` | Vite convention; explicit in `index.html` script tag |
| Proxy target env var | Keep `services__apiservice__apiservice__0` in `vite.config.ts` | `vite.config.ts` runs in Node — no `VITE_` prefix required; preserves Aspire injection pattern unchanged |
| `jsx` transform | `react-jsx` | Eliminates `import React` boilerplate; supported by React 17+ |
| CSS modules | No change | Project uses plain CSS imports; no migration needed |
| Service worker | Keep `registerServiceWorker.ts` | Fix env vars only; no functional change |
