---
feature: lens-dev-new-codebase-constitution
doc_type: business-plan
status: draft
goal: "Rewrite bmad-lens-constitution as a thin, read-only governance resolver that tolerates partial constitution hierarchies without hard-failing, keeps express-track governance usable, achieves behavioral parity with the old codebase across all three subcommands, and unblocks the Epic 4 planning command rewrites."
key_decisions:
  - bmad-lens-constitution is a shared runtime primitive — all downstream planning and dev commands call it; the rewrite must complete before Epic 4 command rewrites begin
  - Partial-hierarchy tolerance replaces the existing org-level hard-fail behavior (baseline story 3.1 contract)
  - Express-track parity is required: constitution validation must accept `express` when lifecycle and constitutions permit it, without downstream command-specific workarounds
  - Read-only command: no governance writes, no feature state mutations under any code path
  - BMB-first authoring channel enforced for SKILL.md and release prompt changes
  - Clean-room re-implementation: old codebase consulted as behavioral reference only; no code or content copied
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline (story 1.1 — scaffold-published-surface)
blocks:
  - lens-dev-new-codebase-baseline (epic 4 — preplan/businessplan/techplan/finalizeplan/expressplan rewrites)
  - lens-dev-new-codebase-baseline (epic 5 — dev command rewrite)
  - lens-dev-new-codebase-baseline (epic 6 — sensing and dashboard rewrites)
updated_at: 2026-05-01T00:00:00Z
---

# Business Plan — Rewrite constitution Command

**Feature:** lens-dev-new-codebase-constitution  
**Author:** crisweber2600  
**Date:** 2026-05-01  
**Track:** express  

---

## 1. Executive Summary

The `constitution` command is the governance rule resolver at the heart of the Lens lifecycle. Every planning and dev command calls it to determine which tracks are permitted, which artifacts are required, and which gate strictness applies for a given domain, service, and repo scope. It is read-only by contract — it never writes governance artifacts and never mutates feature state.

The old-codebase implementation contains a hard-fail condition: if `org/constitution.md` is absent, the `resolve` subcommand returns an error immediately, regardless of whether domain- or service-level constitutions are present. This causes all downstream callers to fail when an org-level constitution file is missing, even in partial-hierarchy deployments that are otherwise fully configured.

This feature rewrites bmad-lens-constitution as a clean-room thin conductor that:

1. Resolves the 4-level constitution hierarchy (org → domain → service → repo) additively, skipping any missing level rather than failing
2. Returns informational warnings (not errors) when levels are absent
3. Preserves full behavioral parity for the three subcommands — `resolve`, `check-compliance`, and `progressive-display` — including express-track callers
4. Establishes regression coverage for partial-hierarchy, additive-merge, and express-track scenarios

The constitution rewrite is the shared runtime dependency for all Epic 4 command rewrites. No planning command can be rewritten until the constitution resolver they depend on is stable and tested.

---

## 2. Problem Statement

The old-codebase constitution command has three structural issues that the new-codebase rewrite must address:

### 2.1 Org-Level Hard-Fail

The `resolve` subcommand checks whether `org/constitution.md` was successfully loaded, and if not, returns an error payload immediately:

```
error: org_constitution_missing — detail: org/constitution.md is required
```

This hard-fail breaks callers (preplan, businessplan, dev, etc.) when:
- A new deployment doesn't yet have an org-level constitution file
- A partial hierarchy is intentional (domain-only or service-only governance)
- The org file is temporarily missing during a governance repo migration

The fix: missing levels must be skipped silently and recorded as warnings. A constitution resolved from partial levels is valid; a constitution resolved from zero levels returns defaults with a warning.

### 2.2 Missing Regression Coverage for Partial Hierarchies

The old-codebase test suite only exercises the full-hierarchy happy path and the explicit org-missing error path. There are no tests for:
- Domain-only resolution (no org, no service)
- Service-only resolution (no org, no domain)
- Additive merge with a sparse level set

### 2.3 Inline Coupling in Callers

Because the old `resolve` subcommand can return an error for org-missing, every caller must defensively wrap the call. The new implementation eliminates the error condition for missing levels, allowing callers to treat the constitution response as always-valid for partial hierarchies.

---

## 3. Stakeholders

| Stakeholder | Role | Interest |
|---|---|---|
| Lens users (all tracks) | Primary users | Constitution resolution works without error for any valid partial hierarchy |
| Lens maintainers | Internal | Reduced caller defensive wrapping; testable partial-hierarchy behavior; read-only guarantee preserved |
| Downstream phase skills (preplan, businessplan, techplan, finalizeplan, expressplan, dev, sensing) | Runtime consumers | Constitution call never returns a hard error for a missing-level scenario; only compliance failures return non-zero |
| init-feature | Bootstrap consumer | Constitution file stubs created correctly for new domains and services |
| dashboard | Display consumer | Can call progressive-display without error even if org-level is absent |

---

## 4. Success Criteria

### Constitution resolve Subcommand

| Criterion | Acceptance Test |
|---|---|
| Missing org-level constitution does not cause error exit | partial-hierarchy regression: no `org_constitution_missing` error in any missing-level scenario |
| Missing org-level constitution produces informational warning | partial-hierarchy regression: `warnings` array in resolve output contains level-absent entry for `org` |
| All present levels merged additively in org → domain → service → repo order | additive-merge regression: lower levels add constraints on top of higher levels |
| Empty hierarchy (no levels present) returns defaults + warnings | empty-hierarchy regression: resolved constitution equals DEFAULTS, warnings non-empty |
| Permitted tracks: intersection of all present levels | merge-rules regression: two-level intersection produces correct track set |
| Required artifacts: union of all present levels, no duplicates | merge-rules regression: two-level union produces deduplicated artifact list |
| Gate mode: strongest wins across all present levels | merge-rules regression: `hard` overrides `informational` when any level specifies hard |
| Enforce_stories / enforce_review: true wins over false | merge-rules regression: one level setting true produces true in resolved output |
| Exit code 0 for all valid (including partial) hierarchies | exit-code regression: all valid partial and full hierarchies return code 0 |
| Exit code 1 only for script errors (bad arguments, unreadable path) | exit-code regression: org-missing is NOT a code-1 scenario |
| Express remains a valid governed track when constitutions permit it | merge-rules regression: resolved permitted_tracks retains `express` when present in the active hierarchy |

### Constitution check-compliance Subcommand

| Criterion | Acceptance Test |
|---|---|
| Track compliance validated against resolved constitution | compliance regression: quickplan track checked against permitted_tracks |
| Express track compliance validated when constitutions permit it | compliance regression: express feature passes without unknown-track rejection or local workaround |
| Required artifact presence validated for given phase | compliance regression: missing artifact produces FAIL entry in checks |
| enforce_review compliance produces check entry when true | compliance regression: participants absent + enforce_review=true produces FAIL |
| Exit code 2 for compliance failure (hard gate) | exit-code regression: hard gate + missing artifact exits 2 |
| Exit code 0 for informational failure | exit-code regression: informational gate + missing artifact exits 0 with failure in payload |

### Constitution progressive-display Subcommand

| Criterion | Acceptance Test |
|---|---|
| Returns context-filtered governance rules for given phase + track | display regression: phase=planning returns required_artifacts_for_phase for planning bucket |
| Track filters recognize express when the hierarchy permits it | display regression: `--track express` returns `track_permitted=true` when express is allowed |
| `full_constitution_available` reflects whether org level was loaded | display regression: org-missing sets full_constitution_available=false |
| Warnings from resolve propagated to display output | display regression: warnings array present when org-missing |

---

## 5. Scope

### In Scope

- Clean-room rewrite of `lens.core/_bmad/lens-work/skills/bmad-lens-constitution/SKILL.md`
- Clean-room rewrite of `lens.core/_bmad/lens-work/prompts/lens-constitution.prompt.md`
- Verify / update `.github/prompts/lens-constitution.prompt.md` stub chain
- Clean-room rewrite of `lens.core/_bmad/lens-work/skills/bmad-lens-constitution/scripts/constitution-ops.py`
- Regression tests for partial hierarchy, additive merge, express-track parity, and all three subcommands

### Out of Scope

- Caller command rewrites (preplan, businessplan, techplan, etc.) — deferred to Epic 4 feature set
- Governance repo structure changes
- Lifecycle schema changes (v4 is frozen)
- Constitution file format changes (YAML frontmatter in `.md` files is retained)
- New subcommands beyond the existing three

---

## 6. Business Value and Risk

### Business Value

| Value | Impact |
|---|---|
| Unblocks Epic 4 (all planning command rewrites) | HIGH — no planning command rewrite can begin until the constitution resolver they depend on is stable |
| Removes partial-hierarchy deployment blocker | HIGH — enables new domain/service deployments that lack org-level constitutions |
| Keeps express planning usable without downstream local workarounds | HIGH — expressplan and express-routed features depend on constitution accepting `express` as a governed track |
| Read-only guarantee preserved and testable | MEDIUM — reduces governance audit surface for all callers |
| Simplified caller integration | MEDIUM — callers no longer need defensive wrapping for org-missing scenarios |

### Risk

| Risk | Likelihood | Mitigation |
|---|---|---|
| Behavior regression in full-hierarchy case | MEDIUM | Full regression suite preserves existing passing tests alongside new partial-hierarchy tests |
| Express-track support drifts from lifecycle and baseline rewrite contracts | MEDIUM | Add explicit express-track parity criteria to resolve, compliance, and progressive-display regressions |
| Progressive-display callers depend on org-missing error | LOW | No confirmed callers rely on hard error for partial hierarchy; `full_constitution_available` flag serves as equivalent signal |
| Merge rule drift from old codebase | LOW | Behavioral reference review of all 5 merge rules against old codebase before test authoring |
| Partial-hierarchy defaults mismatch | LOW | DEFAULTS structure verified against old codebase before clean-room implementation |

---

## 7. Dependencies

| Dependency | Type | Notes |
|---|---|---|
| lens-dev-new-codebase-baseline (story 1.1) | depends_on | Published surface scaffold required before constitution command chain can be wired |
| lens-dev-new-codebase-baseline (epic 4) | blocks | All Epic 4 planning rewrites depend on the stable constitution resolver |
| lens-dev-new-codebase-baseline (epic 5) | blocks | Dev command depends on constitution for gate mode and artifact enforcement |
| lens-dev-new-codebase-baseline (epic 6) | blocks | Sensing and dashboard depend on constitution progressive-display |
| lens-dev-new-codebase-baseline | reference | 3-hop chain, BMB-first authoring, authority domain rules |
