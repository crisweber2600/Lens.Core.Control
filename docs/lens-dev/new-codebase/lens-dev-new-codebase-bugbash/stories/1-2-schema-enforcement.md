---
story_id: "1.2"
epic: "Epic 1"
feature: lens-dev-new-codebase-bugbash
title: Bug Frontmatter Schema Enforcement
priority: High
size: S
status: not-started
sprint: sprint-1
updated_at: 2026-05-03T23:45:00Z
---

# Story 1.2 — Bug Frontmatter Schema Enforcement

## Context

Bug artifacts must have a strictly enforced frontmatter schema and status state machine.
This story builds the validator and state-machine logic used by both `bug-reporter-ops.py`
(at intake) and `bug-fixer-ops.py` (at status mutations).

**Bug frontmatter schema** (from tech-plan Section 2.1):
```yaml
title: "Short descriptive title"
description: "Concise description"
status: New          # enum: New | Inprogress | Fixed
featureId: ""        # empty at intake; populated at fix kickoff
slug: "descriptive-slug-a3f72c1d"
created_at: 2026-05-03T12:00:00Z
updated_at: 2026-05-03T12:00:00Z
```

**Allowed transitions:**
- `intake` → sets `New`
- `fix kickoff` → `New` → `Inprogress` (requires featureId assigned first)
- `fix completion` → `Inprogress` → `Fixed`

**Forbidden transitions (hard-blocked):**
- `New` → `Fixed`
- `Fixed` → any state (terminal)
- `Inprogress` → `New`

Depends on: Story 1.3 (scope guard must be in place).

## Tasks

1. Implement schema validation in Python: validate all required fields, enforce status enum.
2. Implement state machine: validate that requested transition is allowed before any file operation.
3. Invalid transitions must log as explicit errors — no silent coercion.
4. Integrate schema validation into `bug-reporter-ops.py` (validate before write).
5. Integrate schema + state machine into `bug-fixer-ops.py` (validate before move operations).
6. Write unit tests covering the schema and state machine cases from tech-plan Section 7.1.
7. Commit with message: `[dev:1.2] lens-dev-new-codebase-bugbash — frontmatter schema enforcement`.

## Acceptance Criteria

- [ ] Frontmatter contains exactly: title (string), description (string), status (enum), featureId (string or empty), slug, created_at, updated_at
- [ ] Status values restricted to `New`, `Inprogress`, `Fixed` — invalid values rejected with explicit error
- [ ] Invalid transitions (New→Fixed, Fixed→any, Inprogress→New) are blocked; prior valid status is preserved
- [ ] Schema validation in both `bug-reporter-ops.py` and `bug-fixer-ops.py`
- [ ] No silent coercion; all violations logged as explicit errors in per-item outcome report

## Regression Coverage (tech-plan § 7.1)

| Test | Expected |
|------|----------|
| Intake with all required fields | Artifact created; status=New; featureId="" |
| Intake missing title | Rejected; no file written |
| Intake missing description | Rejected; no file written |
| Status set to invalid value | Operation rejected; prior state preserved |
| Invalid transition (New→Fixed) | Blocked; explicit error |
| Invalid transition (Fixed→New) | Blocked; explicit error |
| Invalid transition (Inprogress→New) | Blocked; explicit error |
