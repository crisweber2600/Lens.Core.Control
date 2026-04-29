---
feature: lens-dev-new-codebase-preplan
story_id: PP-3.2
epic: PP-E3
title: Wire bmad-lens-batch for batch mode
estimate: M
sprint: 3
status: not-started
depends_on: [PP-2.1]
blocks: [PP-4.1]
prerequisite: "Sprint 3 gate verification command passes; baseline story 1-3 confirmed callable in lens.core.src"
updated_at: 2026-04-28T00:00:00Z
---

# PP-3.2 — Wire bmad-lens-batch for batch mode

## Story

**As a** Lens user running preplan in batch mode,  
**I want** the conductor to delegate entirely to `bmad-lens-batch` for the 2-pass contract,  
**so that** batch intake and resume are governed by the shared contract and not re-implemented inline.

## Context

Batch mode in Lens follows a strict 2-pass contract:
- **Pass 1:** Write `preplan-batch-input.md` and stop. No lifecycle artifacts (no `brainstorm.md`, `research.md`, `product-brief.md`, no `feature.yaml` update).
- **Pass 2:** `batch_resume_context` is loaded as pre-approved context. Authoring resumes from the stored context.

The conductor must delegate entirely to `bmad-lens-batch --target preplan`. Any inline `if mode == batch and batch_resume_context absent` logic is a defect.

**Sprint 3 Prerequisite Gate:** Before starting this story, confirm the Sprint 3 gate verification command passes (same as PP-3.1).

**Hard Prerequisite:** Baseline story 1-3 (`bmad-lens-batch --target preplan` callable) confirmed green in `lens.core.src` before this story is closed.

## CLI Invocation (Exact)

Pass 1:
```bash
bmad-lens-batch --target preplan
```
(stops after writing `preplan-batch-input.md`)

Pass 2:
- `batch_resume_context` is present in the activation context
- Conductor treats it as pre-approved answers and resumes authoring

## Acceptance Criteria

- [ ] On batch pass 1: conductor calls `bmad-lens-batch --target preplan` and stops; only `preplan-batch-input.md` is written; no lifecycle artifacts (no `brainstorm.md`, no `feature.yaml` update).
- [ ] On batch pass 2: `batch_resume_context` is loaded as pre-approved context; authoring resumes correctly.
- [ ] No inline `if mode == batch and batch_resume_context absent` logic exists in the conductor SKILL.md.
- [ ] `test_batch_pass1_stop` and `test_batch_pass2_resume` parity tests turn green.
- [ ] Baseline story 1-3 confirmed callable in `lens.core.src` (record in PR description).
- [ ] Sprint 3 gate verification command confirmed passing (record result in PR description).
- [ ] No existing parity tests regress.

## Definition of Done

- Batch wiring merged to feature branch.
- Both batch parity tests green.
- Baseline story 1-3 and Sprint 3 gate confirmation recorded in PR.
