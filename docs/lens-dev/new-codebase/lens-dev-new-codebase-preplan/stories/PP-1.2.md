---
feature: lens-dev-new-codebase-preplan
story_id: PP-1.2
epic: PP-E1
title: Write SKILL.md thin conductor contract
estimate: M
sprint: 1
status: not-started
depends_on: [PP-1.1]
blocks: [PP-1.3, PP-2.1]
updated_at: 2026-04-28T00:00:00Z
---

# PP-1.2 — Write SKILL.md thin conductor contract

## Story

**As a** developer implementing the preplan conductor,  
**I want** a complete SKILL.md behavioral contract that describes every integration point and sequencing rule,  
**so that** implementation decisions can be made against a written specification rather than guesswork.

## Context

The SKILL.md is the only implementation artifact owned by this feature in the skills directory. There is no `preplan-ops.py` script. The SKILL.md describes *what* the conductor does and *how it delegates* — it is not a runnable script. It is the spec that parity tests and implementation PRs are written against.

Key behavioral contracts the SKILL.md must describe:
1. **On Activation** — feature context resolution, docs-path resolution, governance mirror path resolution, constitution load.
2. **Analyst activation first** — `bmad-agent-analyst` runs before any brainstorm mode is selected; analyst frames goals, constraints, and assumptions.
3. **Brainstorm mode choice** — user chooses between `bmad-brainstorming` (divergent ideation) and `bmad-cis` (structured innovation); both route through `bmad-lens-bmad-skill`.
4. **Brainstorm-first ordering** — `brainstorm.md` must exist before research or product-brief wrappers are invoked.
5. **Batch 2-pass contract** — delegates entirely to `bmad-lens-batch --target preplan`; no inline batch logic.
6. **Review-ready fast path** — delegates to `validate-phase-artifacts.py --phase preplan --contract review-ready`; no inline artifact checks.
7. **Phase completion gate** — `bmad-lens-adversarial-review --phase preplan --source phase-complete` in party mode; `fail` blocks phase update.
8. **No governance writes** — `publish-to-governance` is NOT called during preplan; businessplan owns that publish step.
9. **`/next` pre-confirmed handoff** — when activated via `/next` delegation, no launch confirmation prompt; preplan begins immediately.
10. **Integration points table** — all delegated skills listed with their call sites.

## Implementation Target

`TargetProjects/lens-dev/new-codebase/lens.core.src/`

## File to Create

```
_bmad/lens-work/skills/bmad-lens-preplan/SKILL.md
```

A `scripts/tests/` subpath is **permitted** (it is not an "owned script layer" — per ADR 1 this constraint applies only to implementation scripts). The `SKILL.md` must not reference a `preplan-ops.py` or any equivalent implementation script.

## Reference

`lens.core/_bmad/lens-work/skills/bmad-lens-preplan/SKILL.md` is the behavioral specification. Author the new SKILL.md independently using the spec as the source of truth — do not copy it verbatim.

## Acceptance Criteria

- [ ] `_bmad/lens-work/skills/bmad-lens-preplan/SKILL.md` exists in `lens.core.src`.
- [ ] SKILL.md documents analyst activation before brainstorm mode selection.
- [ ] SKILL.md documents user choice between `bmad-brainstorming` and `bmad-cis`; both route through `bmad-lens-bmad-skill`.
- [ ] SKILL.md documents brainstorm-first ordering (`brainstorm.md` must exist before downstream synthesis).
- [ ] SKILL.md documents batch delegation to `bmad-lens-batch` with no inline batch logic.
- [ ] SKILL.md documents review-ready delegation to `validate-phase-artifacts.py` with no inline artifact checks.
- [ ] SKILL.md documents the phase completion gate (party-mode adversarial review, `fail` blocks update).
- [ ] SKILL.md documents the no-governance-write invariant.
- [ ] SKILL.md documents the `/next` pre-confirmed handoff (no activation confirmation prompt when invoked via `/next`).
- [ ] SKILL.md includes an integration points table listing all delegated skills.
- [ ] SKILL.md does NOT reference a `preplan-ops.py` or equivalent implementation script.
- [ ] Contract matches the release SKILL.md behavioral specification without verbatim copying.

## Definition of Done

- SKILL.md committed on a feature branch in the target project.
- All acceptance criteria above checked off.
- PR reviewed by at least one other team member before merge.
