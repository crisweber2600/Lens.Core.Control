---
feature: lens-dev-new-codebase-preplan
story_id: PP-3.1
epic: PP-E3
title: Wire validate-phase-artifacts.py for review-ready
estimate: M
sprint: 3
status: not-started
depends_on: [PP-2.2]
blocks: [PP-4.1]
prerequisite: "Sprint 3 gate verification command passes; baseline story 1-2 confirmed callable in lens.core.src"
updated_at: 2026-04-28T00:00:00Z
---

# PP-3.1 — Wire validate-phase-artifacts.py for review-ready

## Story

**As a** Lens user re-activating preplan when all artifacts already exist,  
**I want** the conductor to detect the review-ready state via the shared validation script and skip to the adversarial review gate,  
**so that** I don't re-run authoring flows unnecessarily.

## Context

The review-ready fast path allows users to skip the brainstorm → research → product-brief authoring flow when all three artifacts already exist. This path is detected at activation via `validate-phase-artifacts.py` and routes directly to the adversarial review gate.

The conductor must call the script via its canonical CLI invocation — no inline artifact checks.

**Sprint 3 Prerequisite Gate:** Before starting this story, confirm the Sprint 3 gate verification command passes from `TargetProjects/lens-dev/new-codebase/lens.core.src`:
```bash
uv run --with pytest pytest \
  _bmad/lens-work/skills/bmad-lens-validate-phase-artifacts/scripts/tests/ \
  _bmad/lens-work/skills/bmad-lens-batch/scripts/tests/ \
  _bmad/lens-work/skills/bmad-lens-constitution/scripts/tests/ \
  -q
```

**Hard Prerequisite:** Baseline story 1-2 (`validate-phase-artifacts.py` callable with `--phase preplan --contract review-ready`) confirmed green in `lens.core.src` before this story is closed.

## CLI Invocation (Exact)

```bash
uv run {module_path}/_bmad/lens-work/skills/bmad-lens-validate-phase-artifacts/scripts/validate-phase-artifacts.py \
  --phase preplan \
  --contract review-ready \
  --lifecycle-path {lifecycle_path} \
  --docs-root {docs_root} \
  --json
```

Response parsing:
- `status: pass` → fast path to adversarial review gate (skip authoring)
- `status: fail` → normal authoring flow

## Acceptance Criteria

- [ ] On activation, conductor calls `validate-phase-artifacts.py` with `--phase preplan --contract review-ready` and `--json`.
- [ ] `status: pass` triggers the review-ready fast path directly to the adversarial review gate.
- [ ] `status: fail` triggers normal authoring flow (brainstorm → research → product-brief).
- [ ] No inline artifact presence checks in the conductor (no direct checks for `brainstorm.md`, `research.md`, `product-brief.md`).
- [ ] `test_review_ready_delegation` parity test turns green.
- [ ] Baseline story 1-2 confirmed callable in `lens.core.src` (record confirmation in PR description).
- [ ] Sprint 3 gate verification command confirmed passing (record result in PR description).
- [ ] No existing parity tests regress.

## Definition of Done

- Review-ready delegation merged to feature branch.
- `test_review_ready_delegation` green.
- Baseline story 1-2 and Sprint 3 gate confirmation recorded in PR.
