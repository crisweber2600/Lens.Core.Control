---
feature: lens-dev-new-codebase-preplan
story_id: PP-2.1
epic: PP-E2
title: Implement conductor activation and constitution load
estimate: M
sprint: 2
status: not-started
depends_on: [PP-1.3]
blocks: [PP-2.2]
prerequisite: "Baseline story 3-1 (bmad-lens-constitution partial-hierarchy fix) confirmed green before closing"
updated_at: 2026-04-28T00:00:00Z
---

# PP-2.1 — Implement conductor activation and constitution load

## Story

**As a** Lens user activating `/preplan`,  
**I want** the conductor to resolve feature context and load the domain constitution without error,  
**so that** the preplan session starts from a consistent, governed state regardless of missing org-level constitution entries.

## Context

The conductor's "On Activation" sequence is the foundation all subsequent stories build on. It must:
1. Resolve feature context via `bmad-lens-feature-yaml`.
2. Resolve the staged docs path (from `feature.yaml.docs.path` or fallback: `docs/{domain}/{service}/{featureId}`).
3. Resolve the governance mirror path (from `feature.yaml.docs.governance_docs_path` or fallback).
4. Load domain constitution via `bmad-lens-constitution` — gracefully handles partial hierarchy (relies on baseline story 3-1).
5. Handle the `/next` pre-confirmed handoff: when delegated from `/next`, no activation confirmation prompt is shown; the session begins immediately.

The `fetch-context` availability test (from PP-1.3) should turn green in this story, confirming `bmad-lens-init-feature fetch-context` is callable.

**Prerequisite:** Baseline story 3-1 must be confirmed green in `lens.core.src` before this story is closed. The conductor must not add a workaround for missing org-level constitution — the fix belongs in the shared `bmad-lens-constitution` skill.

## Implementation Target

`TargetProjects/lens-dev/new-codebase/lens.core.src/`

## File to Implement

```
_bmad/lens-work/skills/bmad-lens-preplan/SKILL.md
```
(The On Activation section — other sections remain stubs until their respective stories.)

## Acceptance Criteria

- [ ] Conductor resolves feature context via `bmad-lens-feature-yaml` on activation.
- [ ] Conductor resolves docs path and governance mirror path correctly.
- [ ] Domain constitution loaded via `bmad-lens-constitution`; partial hierarchy handled without panic (baseline story 3-1 confirmed green before closing).
- [ ] When activated via `/next` delegation, no launch confirmation prompt is presented — preplan begins immediately.
- [ ] `fetch-context` availability parity test (`test_fetch_context_availability`) turns green.
- [ ] Analyst-activation-ordering parity test (`test_analyst_activation_ordering`) still fails red (analyst is not wired in this story).
- [ ] Existing parity tests remain at their current state (no regressions).

## Definition of Done

- On Activation implementation merged to feature branch.
- `test_fetch_context_availability` green; `test_analyst_activation_ordering` still red.
- Baseline story 3-1 confirmed green in `lens.core.src` (record confirmation in PR).
