# Project Overview — Next Command (/next)

**Feature ID:** lens-dev-new-codebase-next
**Track:** Express
**PR:** https://github.com/crisweber2600/Lens.Core.Src/pull/13 (merged 2026-05-01)

---

## What Was Delivered

The `/next` command is a lifecycle routing conductor for the LENS Workbench. It reads the current feature's state from `feature.yaml` + `lifecycle.yaml` and routes the user to the appropriate next phase skill — or surfaces blockers if the feature cannot advance.

---

## Architecture

### Prompt Chain

```
.github/prompts/lens-next.prompt.md          (public stub — runs preflight, loads release)
  └── _bmad/lens-work/prompts/lens-next.prompt.md  (release redirect)
        └── _bmad/lens-work/skills/bmad-lens-next/SKILL.md  (conductor shell)
```

### Conductor Shell (`SKILL.md`)

The conductor shell:
1. Runs `next-ops.py` via `uv run` to determine feature routing
2. Reads JSON result (`status`, `recommendation`, `blockers`, `warnings`, `phase`, `track`)
3. On `status=unblocked`: delegates immediately to `bmad-lens-bmad-skill --skill bmad-lens-{phase} --feature-id {featureId}` — no second confirmation prompt
4. On `status=blocked`: surfaces blockers to the user
5. On `status=warn`: displays warnings, then continues to delegate

**Non-negotiable:** The conductor shell makes no write tool calls. All routing decisions come from `next-ops.py`.

### Routing Engine (`next-ops.py`)

```
_bmad/lens-work/skills/bmad-lens-next/scripts/next-ops.py
```

- Inline uv dependencies: `pyyaml>=6.0`
- Reads `feature.yaml` from governance repo, `lifecycle.yaml` from module path
- Resolves current phase, track, dependency states
- Returns JSON: `{ status, recommendation, blockers, warnings, phase, track, error }`
- Paused-state handling: returns `status=blocked` with recovery instructions
- No filesystem writes

### Discovery Registration

`_bmad/lens-work/module.yaml` — `lens-next.prompt.md` added to prompts list.

---

## Test Coverage

| File | Tests | Status |
|------|-------|--------|
| `scripts/tests/test_next_no_writes.py` | 6 | ✓ Pass |
| `scripts/tests/next-routing-full-track.yaml` | Fixture — 4 full-track phase cases | N/A |
| `scripts/tests/next-routing-express-track.yaml` | Fixture — 3 express-track cases | N/A |
| `scripts/tests/next-routing-edge-cases.yaml` | Fixture — 4 edge cases (paused, warnings, unknown-phase, missing yaml) | N/A |

---

## Known Constraints

- **Constitution dependency:** `lens-dev-new-codebase-constitution` (phase: preplan) must reach `finalizeplan-complete` or later before `/next` will route this feature forward in production. The routing engine correctly returns `blocked` until this dependency is resolved.
- **Phase tracking automation:** Feature.yaml phase advancement after target-repo PR merge is currently manual. A future improvement should automate `dev-complete` milestone on PR merge.

---

## Key Files

| Path (relative to `lens.core.src`) | Purpose |
|------------------------------------|---------|
| `.github/prompts/lens-next.prompt.md` | Public prompt stub |
| `_bmad/lens-work/prompts/lens-next.prompt.md` | Release prompt |
| `_bmad/lens-work/skills/bmad-lens-next/SKILL.md` | Conductor shell |
| `_bmad/lens-work/skills/bmad-lens-next/scripts/next-ops.py` | Routing engine |
| `_bmad/lens-work/skills/bmad-lens-next/scripts/tests/` | Test fixtures and no-write tests |
| `_bmad/lens-work/module.yaml` | Module discovery (updated) |
