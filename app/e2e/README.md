# Browser tests

Playwright tests covering the JA/EN toggle and the CRUD round trip — the behaviour the xUnit
tests cannot reach, because it only exists once the page is running in a browser.

## Running them

```bash
cd app/e2e
npm install
npx playwright install chromium   # first time only
npm test
```

Playwright starts the application itself via `webServer` in `playwright.config.js`, so there is
no need to run `dotnet run` first. To test an already-running instance instead:

```bash
BASE_URL=http://localhost:5244 npm test
```

If Chromium is already present (CI images, dev containers), point at it rather than downloading
another copy:

```bash
CHROMIUM_PATH=/path/to/chrome npm test
```

## What is covered

| Spec | Covers |
|------|--------|
| `localization.spec.js` | Japanese default under an `en-US` browser, no-reload switching, title updates, cookie persistence across a round trip, data left untranslated, accessible names, localized server-side validation |
| `crud.spec.js` | Create/edit/delete round trip, `CreatedAt` immutability, favourite toggle, 404 handling, open-redirect rejection |

## Notes

- `fullyParallel` is off and `workers` is 1: the CRUD specs share one SQLite database, so running
  them concurrently makes them flaky. The CRUD spec uses timestamped names to stay independent of
  leftover rows.
- Assertions read `textContent`, not `innerText`. `innerText` applies CSS `text-transform`, so an
  uppercase-styled element would compare unequal against its real content.
