---
feature_id: lens-dev-new-codebase-new-domain
story_key: "2-1-stub"
epic: 2
story: 1
title: "Control repo stub: lens-new-domain.prompt.md"
type: implementation
estimate: XS
priority: P1
status: not-started
assigned: crisweber2600
sprint: 2
depends_on:
  - "1-5-integration-tests"
  - "1-4-parity-tests"
blocks:
  - "2-2-release-prompt"
created_at: 2026-04-26T00:00:00Z
updated_at: 2026-04-26T00:00:00Z
---

# Story 2.1 — Control repo stub: lens-new-domain.prompt.md

## What To Build

The control repo stub at `.github/prompts/lens-new-domain.prompt.md`. This is the entrypoint for the `/new-domain` command on the Lens surface. The stub must follow the same pattern as other command stubs.

---

## File Locations

| File | Action |
|---|---|
| `.github/prompts/lens-new-domain.prompt.md` | Create |

---

## Pattern to Follow

Before creating this file, read at least two existing stubs in `.github/prompts/` to understand the exact pattern. Common stub commands to reference: check if `lens-new-feature.prompt.md` or `lens-new-service.prompt.md` exist in `.github/prompts/` and use those as the reference pattern.

The stub must:
1. Run `light-preflight.py` as the first action
2. On exit 0: load the release prompt from `lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md`
3. On non-zero exit: surface the preflight error to the user
4. Contain no domain creation logic itself

---

## Acceptance Criteria

- [ ] File created at `.github/prompts/lens-new-domain.prompt.md`
- [ ] Stub runs `uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py` as first action
- [ ] On exit 0: loads `lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md`
- [ ] Stub structure matches the pattern of at least one existing command stub in `.github/prompts/`
- [ ] No domain creation logic in the stub
