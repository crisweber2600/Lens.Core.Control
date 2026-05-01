---
feature: lens-dev-new-codebase-split-feature
doc_type: business-plan
status: draft
goal: "Rewrite split-feature command as a clean-room thin conductor preserving validate-first semantics, in-progress story blocking, and atomic create-then-move ordering."
key_decisions:
  - Validate-first ordering is non-negotiable — validate-split must pass before any governance mutation is attempted
  - In-progress story blocking is a hard stop with no workaround offered
  - create-split-feature runs before move-stories in every split operation (atomic ordering)
  - Both markdown and YAML story-file formats must be preserved at the script boundary
  - Clean-room re-implementation — old codebase consulted as behavioral reference only; new implementation authored from scratch against the architecture contract
  - Script surface (validate-split, create-split-feature, move-stories) is a retained published API; all three subcommands must remain intact
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline (baseline architecture/rewrite contract; see in-repo baseline docs including prd.md and research.md)
blocks: []
updated_at: 2026-04-30T00:00:00Z
---

# Business Plan — Rewrite split-feature Command

**Feature:** lens-dev-new-codebase-split-feature  
**Author:** crisweber2600  
**Date:** 2026-04-30  
**Track:** full  

---

## 1. Executive Summary

The `split-feature` command allows Lens users to divide a feature's scope or move its story files into a new, independently tracked feature. It is one of the 17 retained published commands in the lens-work rewrite and is explicitly listed in the PRD as a command whose behavioral contract must be fully preserved. This feature rewrites the split-feature command from scratch using the clean-room approach mandated by the baseline architecture: no content is copied from the old codebase; the new implementation is authored against the rewrite contract and validated for parity against the old-codebase discovery artifacts.

The behavioral non-negotiables are: validate first, block in-progress stories, create the new feature before modifying the source, and preserve both markdown and YAML story-file formats. The goal is output parity with zero behavioral regression, implemented through a thin conductor that delegates to the existing `split-feature-ops.py` script surface.

---

## 2. Problem Statement

The old-codebase split-feature skill has three documented structural issues that must be corrected in the rewrite:

1. **No shared utility delegation** — the old skill contains inline governance-write paths that bypass the publish-before-author contract introduced in baseline story 1.4. The new skill must route all governance mutations through the correct delegation points.
2. **Inconsistent story-file status reads** — the old implementation has inconsistent handling of sprint-status YAML vs. story-file frontmatter status, creating cases where in-progress stories can be incorrectly classified as eligible. The new implementation must read both sources consistently.
3. **Partial dry-run coverage** — the old skill exposes a `--dry-run` flag on `create-split-feature` and `move-stories` but does not clearly enforce dry-run semantics before governance mutations during `validate-split`. The new implementation must make dry-run a first-class pre-execution path for all subcommands.

These issues increase the risk of leaving governance in a broken partial state after an interrupted or mis-validated split operation. The rewrite eliminates these gaps while preserving the complete behavioral surface that users and tests depend on.

---

## 3. Stakeholders

| Stakeholder | Role | Interest |
|---|---|---|
| Lens users who split features | Primary users | split-feature works identically to the old codebase — no behavioral regression |
| Lens maintainers | Internal | Clean script boundary, testable delegation, governance audit integrity |
| Features downstream of a split | Consumers | New feature gets complete governance artifacts: feature.yaml, feature-index entry, summary stub |
| Lens PRD / architecture owners | Governance | Retained command surface stays complete; split-feature is not silently demoted |

---

## 4. Success Criteria

| Criterion | Acceptance Test |
|---|---|
| validate-split runs before any governance mutation | validate-first regression: no create-split-feature or move-stories call proceeds unless validate-split exits 0 |
| In-progress stories hard-stop the split | in-progress blocking regression: story with `status: in-progress` in sprint-plan or story-file frontmatter causes a hard stop with clear error output |
| create-split-feature runs before move-stories in every code path | atomic-ordering regression: governance creates the child feature directory and feature.yaml before any story file is moved |
| Both markdown and YAML story-file formats are read correctly | story-file format regression: test suite covers both .md and .yaml story files for validate-split and move-stories |
| New feature receives complete governance setup | governance-completeness regression: feature.yaml, feature-index entry, and summary stub are all present for the new feature after create-split-feature |
| Dry-run mode produces no governance mutations | dry-run regression: `--dry-run` flag on create-split-feature and move-stories produces no file writes |
| The split-feature command remains in the published 17-command surface | installer parity: `.github/prompts/lens-split-feature.prompt.md` stub exists post-rewrite |

---

## 5. Scope

### In Scope

- Rewrite `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/SKILL.md` — thin conductor
- Rewrite `lens.core/_bmad/lens-work/prompts/lens-split-feature.prompt.md` — release prompt
- Rewrite `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/scripts/split-feature-ops.py` — clean-room script preserving all three subcommand surfaces
- Verify `.github/prompts/lens-split-feature.prompt.md` stub chain integrity
- Regression coverage: validate-first, in-progress blocking, atomic ordering, story-file format, dry-run, governance completeness

### Out of Scope

- Changes to `bmad-lens-git-orchestration`, `validate-phase-artifacts.py`, or `bmad-lens-feature-yaml` — those shared implementations are delivered by baseline stories
- Changes to other phase commands (preplan, businessplan, techplan, etc.)
- New split-feature capabilities beyond behavioral parity
- Changes to `feature-index.yaml` schema or `feature.yaml` schema

---

## 6. Business Value and Risk

### Value

- Closes the governance partial-state risk from inconsistent status reads and missing validate-first enforcement
- Restores the published command surface to a clean, auditable thin-conductor pattern
- Enables the `split-feature` test suite (`test-split-feature-ops.py`) to run against a fully specified implementation rather than an ad-hoc old-codebase port
- Demonstrates the conductor pattern for a utility command (feature reshaping), complementing the phase-command examples in businessplan, techplan, and finalizeplan

### Risks

| Risk | Likelihood | Mitigation |
|---|---|---|
| Sprint-plan YAML format variations across features causing missed in-progress status reads | Medium | Unit tests cover all documented sprint-plan formats before merge |
| Dry-run semantics not enforced consistently across all three subcommands | Low | Explicit dry-run regression test for each subcommand |
| New feature governance artifacts incomplete after create-split-feature if an intermediate step fails | Low | Ordered create/write sequence with per-file atomic replace where applicable, plus cleanup or idempotent rerun rules so a failure cannot leave unrecoverable partial state |

---

## 7. Dependencies

### Prerequisites (must be complete before split-feature story 5.3)

| Dependency | Why Required |
|---|---|
| Baseline story 4.4 (rewrite finalizeplan) | Finalizeplan is the phase conductor that may trigger split; its governance artifact handoff boundary must be stable before split is rewritten |
| `bmad-lens-feature-yaml` skill | split-feature reads source feature.yaml and creates the new feature.yaml using the same schema; must be stable |
| `bmad-lens-git-orchestration` publish-to-governance CLI | split-feature commit pattern uses the same git-orchestration CLI used by all phase conductors |

### Blocks

None — split-feature is not a prerequisite for other features in the baseline rewrite sequence.

---

## 8. Timeline Expectations

No external deadline. This feature is sequenced after finalizeplan (story 4.4) and follows the documented prerequisite ordering in Section 7 of this plan.
