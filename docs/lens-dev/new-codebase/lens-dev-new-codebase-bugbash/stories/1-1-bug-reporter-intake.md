---
story_id: "1.1"
epic: "Epic 1"
feature: lens-dev-new-codebase-bugbash
title: Bug Reporter Intake Prompt & Artifact Creation
priority: High
size: M
status: not-started
sprint: sprint-1
updated_at: 2026-05-03T23:45:00Z
---

# Story 1.1 — Bug Reporter Intake Prompt & Artifact Creation

## Context

The `lens-bug-reporter` command captures one bug per run as a governance artifact in the
`governance_repo/bugs/New/` folder. This is the first working command in the bugbash suite.

The 3-hop chain for this command:
```
.github/prompts/lens-bug-reporter.prompt.md  (stub)
  → lens.core/_bmad/lens-work/prompts/lens-bug-reporter.prompt.md  (release prompt)
    → skills/bmad-lens-bug-reporter/SKILL.md  (thin conductor)
      → scripts/bug-reporter-ops.py  (artifact creation)
```

**Slug generation:** `{title-slug}-{sha256(title+description)[:8]}` — stable content-hash key,
idempotent across reruns with identical inputs.

**Script output:** `{ "slug": str, "path": str, "status": "created" | "duplicate" }`

Depends on: Story 1.3 (scope guard) and Story 1.2 (schema enforcement).

## Tasks

1. Create `lens.core/_bmad/lens-work/scripts/bug-reporter-ops.py` with `create-bug` command:
   - Validate required fields (title, description, chat-log)
   - Apply scope guard (Story 1.3 utility)
   - Compute content hash: `sha256(title + description)[:8]`
   - Generate slug: `{title-slug}-{content-hash}`
   - Check idempotency: if artifact with same slug exists in New, Inprogress, or Fixed → return "duplicate", exit 0
   - Create missing parent directories (`bugs/New/`) — clear error if creation fails (A4)
   - Write artifact to `bugs/New/{slug}.md` with validated frontmatter + chat log body
   - Return JSON result
2. Create `lens.core/_bmad/lens-work/skills/bmad-lens-bug-reporter/SKILL.md` via **bmad-module-builder** using the 7-section SKILL.md template from tech-plan Section 6.
3. Create `lens.core/_bmad/lens-work/prompts/lens-bug-reporter.prompt.md` (release prompt) via **bmad-workflow-builder**.
4. Create `.github/prompts/lens-bug-reporter.prompt.md` (stub) following the invariant pattern:
   - Runs `light-preflight.py`; exits on non-zero
   - Loads `lens.core/_bmad/lens-work/prompts/lens-bug-reporter.prompt.md`
5. Consider slug collision hardening: millisecond precision or UUID fallback to prevent concurrent identical runs (A5).
6. Write end-to-end test: create-bug with valid inputs, verify artifact created at correct path.
7. Commit with message: `[dev:1.1] lens-dev-new-codebase-bugbash — bug reporter intake command`.

## Acceptance Criteria

- [ ] `/lens-bug-reporter` creates exactly one artifact at `governance_repo/bugs/New/{slug}.md`
- [ ] Artifact contains valid frontmatter: title, description, status=New, featureId="", slug, created_at, updated_at
- [ ] Omitting required fields (title, description, chat log) blocks creation and prompts for correction
- [ ] Idempotent: re-run with identical inputs returns "duplicate"; no second artifact written
- [ ] `.github/prompts/lens-bug-reporter.prompt.md` stub exists and invokes light-preflight.py
- [ ] `lens.core/_bmad/lens-work/prompts/lens-bug-reporter.prompt.md` release prompt delegates to SKILL.md
- [ ] `lens.core/_bmad/lens-work/skills/bmad-lens-bug-reporter/SKILL.md` exists (via bmad-module-builder)
- [ ] `lens.core/_bmad/lens-work/scripts/bug-reporter-ops.py` create-bug command works end-to-end
- [ ] Missing parent directories (`bugs/New/`) are created or a clear initialization error is reported (A4)
- [ ] Slug collision hardening addressed (A5)

## Implementation Notes

- `bug-reporter-ops.py` exit codes: 0=success, 1=validation failure, 2=scope violation, 3=write error
- Chat log content follows frontmatter as markdown body — preserved verbatim
- BMB-First protocol: Python script authored directly; SKILL.md via bmad-module-builder; release prompt via bmad-workflow-builder
