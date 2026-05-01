---
feature: lens-dev-new-codebase-split-feature
doc_type: finalizeplan-review
phase: finalizeplan
source: phase-complete
verdict: pass-with-warnings
reviewed_artifacts: [business-plan, tech-plan]
parent_artifacts: [lens-dev-new-codebase-baseline/architecture, lens-dev-new-codebase-baseline/prd]
critical_count: 0
high_count: 1
medium_count: 3
low_count: 4
review_format: abc-choice-v1
status: responses-recorded
updated_at: 2026-05-01T00:00:00Z
---

# FinalizePlan Adversarial Review — lens-dev-new-codebase-split-feature

**Phase:** finalizeplan
**Source:** phase-complete
**Reviewed planning set:** `business-plan.md`, `tech-plan.md`
**Parent context:** `lens-dev-new-codebase-baseline` architecture + PRD (archived/complete 2026-05-01)
**Constitution:** lens-dev (informational), new-codebase service (informational)
**Cross-feature context:** baseline complete (story 4.4 finalizeplan rewrite confirmed); expressplan archived
**Governance sensing:** no active features depend on split-feature; `bmad-lens-feature-yaml` and `bmad-lens-git-orchestration` both delivered by baseline (complete)
**Format:** Each finding presents options A/B/C (proposed solutions), D (write own), E (keep as-is). Respond by letter.

---

## How to Respond

For each finding below, reply with the letter of your chosen option. You may add a clarifying note after the letter.

| Option | Meaning |
|---|---|
| A / B / C | Accept the proposed resolution (with its stated trade-offs) |
| D | You will provide a custom resolution — write it after "D:" |
| E | Explicitly no action — this finding is accepted as-is |

---

## Verdict: `pass-with-warnings`

The planning set can advance to the downstream bundle. The business plan and tech plan are internally consistent, well-scoped, and fully traceable to the baseline architecture contract. No finding is a hard stop on artifact quality.

**H1 is a process blocker for Step 2** (missing base branch), not a planning artifact defect. It must be resolved before the planning PR can be created, but it does not invalidate the planning documents.

**Findings summary:** 0 critical · 1 high · 3 medium · 4 low

---

## Cross-Artifact Consistency Check

### Business Plan → Tech Plan

| Business Plan Commitment | Tech Plan Resolution | Consistent? |
|---|---|---|
| Validate-first ordering is non-negotiable | §2.3 validate-split hard gate; exit codes 0/1/2 defined | ✅ |
| In-progress story blocking is a hard stop | §2.3 — exit code 2 with explicit story ID list; no workaround offered | ✅ |
| create-split-feature before move-stories in every code path | §2.4 atomic write ordering steps documented; source modified last | ✅ |
| Both markdown and YAML story-file formats | §2.3 — both .md and .yaml listed as first-class inputs; §6 test class coverage | ✅ |
| Clean-room re-implementation | §5 clean-room notes; SAFE_ID_PATTERN, atomic write, status resolution all freshly specified | ✅ |
| Script surface (validate-split, create-split-feature, move-stories) retained | §3 preserves all three subcommand call interfaces | ✅ |
| Dry-run mode for all subcommands | §3.2 and §3.3 show `[--dry-run]`; §6 dry-run regression test class | ✅ |

### Tech Plan → Baseline Architecture

| Architecture Constraint | Tech Plan Resolution | Consistent? |
|---|---|---|
| 3-hop command resolution chain | §1 system design chain correctly shows stub → release prompt → SKILL.md → script | ✅ |
| Thin conductor pattern | §2.2 — all governance mutations delegated to script; SKILL.md has no direct file writes | ✅ |
| BMB-first for SKILL.md changes | §4.1 channel column: "bmad-module-builder (BMB-first)" | ✅ |
| Atomic temp-file + rename for YAML writes | §5 implementation notes — explicitly specified | ✅ |
| Frozen `feature.yaml` / `feature-index.yaml` schema | §7 rollout: "feature.yaml and feature-index.yaml schema are unchanged (v4 frozen)" | ✅ |
| `light-preflight.py` frozen | §4.3 shared dependencies — "Frozen prompt-start gate" | ✅ |
| `bmad-lens-feature-yaml` for feature reads | §2.2 delegation table | ✅ |

**Baseline dependency status:** `lens-dev-new-codebase-baseline` archived/complete 2026-05-01. Story 4.4 (rewrite-finalizeplan) is confirmed present in baseline docs. The prerequisite is satisfied.

---

## Governance Impact Assessment

| Area | Impact | Action |
|---|---|---|
| `feature-index.yaml` | `lens-dev-new-codebase-split-feature` status shows `preplan`; actual phase is `techplan` | Update on finalizeplan completion (see L1) |
| `lens-dev-new-codebase-split-feature` base branch | Missing from control repo | Must create before Step 2 (see H1) |
| Governance staging path | `business-plan.md` + `tech-plan.md` exist only in governance mirror; control repo staging path is empty | No re-publish needed; mirror is current; noted as M3 |
| `bmad-lens-feature-yaml` / `bmad-lens-git-orchestration` | Both delivered by baseline (archived); no conflicts | No action |
| Related features (constitution, discover, upgrade, etc.) | No active feature lists split-feature in `depends_on` | No action |

---

## Findings

### H1 [HIGH] — Missing control-repo base branch

**Scope:** Step 2 process pre-condition  
**Finding:** The base branch `lens-dev-new-codebase-split-feature` does not exist in the control repo (local or remote). The 2-branch model requires both `{featureId}` and `{featureId}-plan` to exist before the planning PR can be created. FinalizePlan cannot proceed past Step 1 without this branch.  
**Impact:** Hard blocker for Step 2 (`{featureId}-plan` → `{featureId}` PR). The branch must be created before the plan PR gate.

| Option | Resolution |
|---|---|
| **A** | Create `lens-dev-new-codebase-split-feature` from `main` via `bmad-lens-git-orchestration create-feature-branches` before Step 2. FinalizePlan pauses here and resumes after the branch exists. |
| B | Retrospectively run `init-feature` for this feature, which will create the missing branch from the current control repo default branch and register it in governance state. |
| C | Skip the plan PR gate and merge `{featureId}-plan` directly to `main` via a feature PR (non-standard; bypasses the plan PR topology). |
| D | Provide a custom resolution. |
| E | No action (blocks Step 2; feature cannot progress to dev). |

---

### M1 [MEDIUM] — Test file omitted from Files to Create/Modify

**Scope:** `tech-plan.md` §4 Files to Create/Modify  
**Finding:** Section 6 (Testing Strategy) names a test file at `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/scripts/tests/test-split-feature-ops.py`. This file is not listed in §4.1 (Core Deliverables) or §4.2 (References). A developer reading §4 would not know this file is in scope for the story.  
**Impact:** Dev handoff ambiguity — the implementing agent may not know the test file must be created as part of this story.

| Option | Resolution |
|---|---|
| **A** | Add `test-split-feature-ops.py` to §4.1 Core Deliverables with action "Create" and channel "direct". Carry the addition into the dev story file. |
| B | Add a new §4.4 (Test Files) section with the test file listed, keeping the core deliverables table focused on production files. |
| C | Leave §4 unchanged; add a note in the story file task list that the test file must be created. |
| D | Provide a custom resolution. |
| E | Accept as-is. Dev agent is expected to infer from §6 that the test file is in scope. |

---

### M2 [MEDIUM] — Markdown-embedded YAML blocks absent from status resolution priority chain

**Scope:** `tech-plan.md` §2.3 validate-split  
**Finding:** Section 2.3 defines a two-source priority order: (1) sprint plan file, (2) story file frontmatter. However, the same section's description of the sprint plan parse says it "handles markdown-embedded YAML code blocks and simple `key: value` line pairs." Markdown-embedded YAML blocks are a distinct parse path from structured YAML, but they are collapsed into source 1 in the priority list without explicit specification of which embedded-block syntax variants are recognized. If a sprint-status uses a markdown-embedded YAML format the parser doesn't recognize, a story's `in-progress` status could be silently missed.  
**Impact:** Medium — validate-split could false-negative on an in-progress story if the sprint-plan uses an embedded YAML variant that isn't covered by the parser.

| Option | Resolution |
|---|---|
| **A** | Extend §2.3 to enumerate the recognized embedded YAML patterns (e.g., ` ```yaml … ``` ` fenced blocks with `stories:` map, inline `story-id: in-progress` lines) and add an "unrecognized format → treat as no-data → fall back to story frontmatter" default rule. Carry forward into test class M2-specific tests. |
| B | Add a §2.3 note: "Any sprint-plan format not recognized by the parser is treated as absent; the fallback to story-file frontmatter applies." Accept that unrecognized formats may degrade validation quality without hard-stopping. |
| C | Require the sprint plan to be in canonical YAML only (no markdown-embedded blocks); reject markdown-embedded formats with exit code 1 (runtime error). |
| D | Provide a custom resolution. |
| E | Accept as-is. Existing sprint plans in this service use canonical YAML; embedded block risk is low. |

---

### M3 [MEDIUM] — Governance artifact staging gap

**Scope:** Control repo docs path / Phase 1 staging convention  
**Finding:** `business-plan.md` and `tech-plan.md` exist only in the governance mirror (`features/lens-dev/new-codebase/lens-dev-new-codebase-split-feature/docs/`). The control repo staging path (`docs/lens-dev/new-codebase/lens-dev-new-codebase-split-feature/`) is empty. The FinalizePlan review outputs (this file) and the downstream bundle artifacts will be the first files committed to the control repo staging path for this feature. The governance mirror is current (docs were published there), but the control repo is the authority domain for staged artifacts per the lifecycle contract.  
**Impact:** Medium — the reverse publish path (governance → control repo) is not standard. If the governance artifacts are later re-published, the mirror version will overwrite the control repo version. This is a process gap, not a quality defect in the planning docs.

| Option | Resolution |
|---|---|
| A| Accept the current state. The governance mirror holds the planning docs; the control repo staging path holds only FinalizePlan outputs forward. Note the deviation in this review and ensure the story file directs the dev agent to consult governance docs for business-plan and tech-plan context. |
| **B** | Copy `business-plan.md` and `tech-plan.md` from the governance mirror into the control repo staging path as part of this FinalizePlan commit, restoring the canonical authority domain. |
| C| Leave the gap unresolved until the trueup feature or a dedicated housekeeping task reconciles governance mirrors and control repo staging paths across all features. |
| D | Provide a custom resolution. |
| E | Accept as-is. The governance mirror is sufficient for cross-feature consumers and the dev agent will be directed to it. |

---

### L1 [LOW] — Feature-index status stale (`preplan` vs actual `techplan`)

**Scope:** `feature-index.yaml` in governance repo  
**Finding:** The `lens-dev-new-codebase-split-feature` entry in `feature-index.yaml` shows `status: preplan`. The `feature.yaml` shows `phase: techplan`. The index was not updated when planning advanced through businessplan and techplan phases.  
**Impact:** Low — tooling that reads feature-index.yaml for status will show stale state. No dev-path blocking.

| Option | Resolution |
|---|---|
| **A** | Update `feature-index.yaml` `status` to `techplan` on the governance repo as part of this FinalizePlan commit, then advance to `finalizeplan` on phase completion. |
| B | Leave the index stale and update it only when the finalizeplan phase completes (advance directly to `finalizeplan-complete`). |
| C | Treat the feature-index status field as a non-authoritative cache and skip updating it. |
| D | Provide a custom resolution. |
| E | Accept as-is. |

---

### L2 [LOW] — SAFE_ID_PATTERN validation placement ambiguous

**Scope:** `tech-plan.md` §5 Implementation Notes vs. §3 Script Surface  
**Finding:** Section 5 lists `SAFE_ID_PATTERN` validation on all feature IDs as an implementation requirement. Section 3 (the preserved API spec) does not show where this validation fires — whether it is enforced in the CLI argument parser before any logic runs, or at the point where the ID is used to construct a path.  
**Impact:** Low — if the pattern is applied in multiple ad-hoc locations instead of at a single early parse boundary, a path-traversal ID could survive one check and fail another, creating an inconsistent rejection surface.

| Option | Resolution |
|---|---|
| **A** | Add a note to §3 that all `--feature-id`, `--source-feature-id`, and `--new-feature-id` arguments are validated against `SAFE_ID_PATTERN` at argument-parse time (before any subcommand logic runs), with exit code 1 on failure. |
| B | Add to §6 Testing a test class: "Identifier validation regression — invalid IDs (uppercase, spaces, path traversal) rejected with exit code 1 before any file operation." |
| C | Accept the current §5 specification as sufficient; the implementing developer is expected to apply the pattern at parse time. |
| D | Provide a custom resolution. |
| E | Accept as-is. |

---

### L3 [LOW] — Moved story files may contain stale cross-references

**Scope:** `tech-plan.md` §5, move-stories behavior  
**Finding:** Section 5 states "Story files are treated as compatibility boundaries: the script moves files as-is without modifying content." Story file frontmatter typically contains a `feature:` field pointing to the source feature ID, and story body text may reference the source feature by name. After move-stories runs, these references will point to the old feature, potentially confusing the dev agent consuming the moved story files.  
**Impact:** Low — the moved stories are expected to be dev-ready inputs. Stale `feature:` frontmatter won't cause tool failures, but it will require the dev agent or reviewer to notice and manually correct the reference.

| Option | Resolution |
|---|---|
| A | Add a post-move step to `move-stories`: update `feature:` frontmatter in moved story files to reference the new feature ID. Only the `feature:` field is rewritten; body content is not modified. |
| **B** | Add a note to the SKILL.md that after `move-stories` completes, the Lens agent should scan moved story files for `feature:` fields and report any that still reference the source feature. No automatic rewrite. |
| C | Accept "as-is" policy for all story content, including `feature:` frontmatter. Document the expectation in the story handoff note so the dev agent can manually fix before implementing. |
| D | Provide a custom resolution. |
| E | Accept as-is. |

---

### L4 [LOW] — Concurrent execution safety not addressed

**Scope:** `tech-plan.md` §2.4, §8 Observability  
**Finding:** The atomic temp-file + rename pattern protects against partial writes from a single process. However, the tech plan does not address concurrent execution: if two instances of `split-feature-ops.py` target the same governance repo simultaneously (e.g., from two parallel Lens agent sessions), both could read the same governance state before either commits, producing duplicate feature entries or conflicting feature-index updates.  
**Impact:** Low — Lens governance operations are expected to be single-session. The risk is theoretical but worth acknowledging.

| Option | Resolution |
|---|---|
| A | Add a file-lock guard (e.g., `filelock` or a `.lock` file in the governance repo root) to `create-split-feature` and `move-stories` before any write. |
| **B** | Add a note to §8 Observability: "Concurrent execution from two sessions targeting the same governance repo is unsupported. Feature authors must ensure only one split-feature session is active at a time." No code change. |
| C | Accept as-is. Single-session assumption is implicit in all Lens governance operations. |
| D | Provide a custom resolution. |
| E | Accept as-is. |

---

## Party-Mode Blind Spot Challenge

Three additional risks surfaced during the party-mode blind spot round:

### BS-1 — `in_progress` vs `in-progress` delimiter normalization

**Identified by:** edge-case hunter  
The tech plan uses `in-progress` (hyphen) throughout. Sprint-status YAML files in this repo use `status: in-progress` (confirmed). However, the old codebase may have generated files using `in_progress` (underscore) for some status fields. validate-split's status recognition is not documented as case-or-delimiter–normalized. If a sprint-plan has `in_progress: true` style or an underscore-delimited status, a story could silently pass the hard gate.  
**Recommended action:** Add a normalization step to the status reader: treat `in_progress`, `in-progress`, `IN_PROGRESS`, `in progress` as equivalent before the hard-stop check. Add a test case for the underscore variant.

### BS-2 — Old-codebase behavioral reference artifact not located

**Identified by:** acceptance auditor  
The business plan says "Clean-room re-implementation — old codebase consulted as behavioral reference only." The tech plan references "old-codebase" in the comparison table (§2.2) but does not link to the old-codebase discovery document or specify which old-codebase file is the reference. During dev, the implementing agent will need to know exactly which old-codebase file is the behavioral reference for output parity testing.  
**Recommended action:** Add to §5 a reference pointer to `TargetProjects/lens-dev/old-codebase/lens.core.src/` (or its governance mirror) as the behavioral reference path, and note that the discovery feature (`lens-dev-old-codebase-discovery`) is the governance-registered source for old-codebase artifacts.

### BS-3 — `feature-index.yaml` duplicate detection not mentioned

**Identified by:** blind hunter  
`create-split-feature` creates a new feature-index entry, but the tech plan does not specify what happens if the target `new-feature-id` already exists in `feature-index.yaml`. This could occur if a split was partially run and interrupted before completion. The atomic write pattern prevents partial feature.yaml writes, but a pre-existing feature-index entry for the new ID would not be caught by the current spec.  
**Recommended action:** Add to `create-split-feature` pre-conditions in §2.4: "Before writing any artifact, check that `new-feature-id` does not already exist in `feature-index.yaml`. If it does, exit 1 with a clear duplicate-feature error."

---

## Summary and Next Actions

| Finding | Severity | Response | Applied |
|---|---|---|---|
| H1: Missing base branch | High | **A** — create from main | ✓ base branch created, PR #37 open |
| M1: Test file not in §4 | Medium | **A** — added to §4.1 Core Deliverables | ✓ tech-plan.md updated |
| M2: Embedded YAML priority ambiguous | Medium | **A** — patterns enumerated, fallback rule added | ✓ tech-plan.md §2.3 updated |
| M3: Governance staging gap | Medium | **B** — planning docs staged in control repo | ✓ committed in 3cdaff2 |
| L1: Feature-index stale | Low | **A** — status updated to `techplan` | ✓ feature-index.yaml updated |
| L2: SAFE_ID_PATTERN placement | Low | **A** — parse-time validation note added to §3 | ✓ tech-plan.md updated |
| L3: Stale cross-references in moved stories | Low | **B** — post-move scan requirement in SKILL.md deliverable | ✓ tech-plan.md §4.1 + §5 updated |
| L4: Concurrent execution | Low | **B** — unsupported note added to §8 | ✓ tech-plan.md updated |
| BS-1: Delimiter normalization | Advisory | Applied — normalization spec in §2.3, test case in §6 | ✓ tech-plan.md updated |
| BS-2: Old-codebase reference path | Advisory | Applied — reference pointer added to §5 | ✓ tech-plan.md updated |
| BS-3: Duplicate feature-index detection | Advisory | Applied — pre-condition check added to §2.4 | ✓ tech-plan.md updated |

**Carry-forward blockers into dev story:**
- Resolve BS-1 (delimiter normalization) in validate-split implementation  
- Resolve BS-2 (old-codebase reference path) in story context section
- Resolve BS-3 (duplicate detection) in create-split-feature pre-conditions

**Phase gate:** H1 must be resolved (base branch created) before Step 2 proceeds. All other items are documented and carry forward into the dev story file or implementation notes.
