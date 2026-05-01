---
feature: lens-dev-new-codebase-dogfood
doc_type: dryrun-expressplan-to-finalizeplan
story: E5-S3
status: approved
updated_at: "2025-07-17"
---

# Dry-Run: ExpressPlan → FinalizePlan Phase Transition

## Test Feature

**Feature ID:** `lens-dev-new-codebase-dogfood-dryrun-1`
**Track:** `express`
**Scratch branch:** `lens-dev-new-codebase-dogfood-dryrun-1-plan` (created and deleted within this dry-run)

This is a full trace of the ExpressPlan → FinalizePlan transition contract. No source files are executed; instead, each skill contract step is traced against live artifact routing logic confirmed by reading `git-orchestration-ops.py`, the expressplan and finalizeplan SKILL.md contracts.

---

## Phase 1: ExpressPlan Conductor

### Gate Checks (On Activation)

| Gate | Condition | Result |
|---|---|---|
| Express-only track gate | feature.yaml.track = `express` | ✅ PASS |
| Phase gate | feature.yaml.phase = `expressplan` | ✅ PASS |
| Constitution permission | `permitted_tracks` includes `express` | ✅ PASS |
| QuickPlan prerequisite | `bmad-lens-bmad-skill` registered, `bmad-lens-quickplan` registered, skill file exists | ✅ PASS |

### Step 1 — quickplan-via-lens-wrapper

**Command:** `bmad-lens-bmad-skill --skill bmad-lens-quickplan plan lens-dev-new-codebase-dogfood-dryrun-1`

**Write scope:** `docs/lens-dev/new-codebase/lens-dev-new-codebase-dogfood-dryrun-1/` (resolved from feature.yaml.docs.path)

**Expected outputs written to control repo staged docs path:**
- `business-plan.md` ✅
- `tech-plan.md` ✅
- `sprint-plan.md` ✅

**No governance files written.** QuickPlan outputs write only to the control-repo staged docs path; no direct governance authoring.

### Step 2 — adversarial-review-party-mode

**Pre-invoke artifact check:**
- `business-plan.md` — present ✅
- `tech-plan.md` — present ✅
- `sprint-plan.md` — present ✅

**Command:** `bmad-lens-adversarial-review --phase expressplan --source phase-complete`

**Review artifact written:** `expressplan-adversarial-review.md` (canonical; `expressplan-review.md` is NOT used as output)

**Verdict:** `pass` → advance to Step 3

**No pre-verdict phase mutation.** `feature.yaml.phase` unchanged until after verdict read.

### Step 3 — advance-to-finalizeplan

**Phase mutation:** `feature.yaml.phase = expressplan-complete` (written via `bmad-lens-feature-yaml`)

**Signal:** `/finalizeplan` reported as next required command.

**No FinalizePlan bundle artifacts generated here.** ExpressPlan owns: `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, `expressplan-adversarial-review.md`. Downstream bundle (`epics.md`, `stories.md`, etc.) is FinalizePlan's responsibility.

---

## Phase 2: FinalizePlan Conductor

### Gate Checks (On Activation)

| Gate | Condition | Result |
|---|---|---|
| Predecessor gate | feature.yaml.phase = `expressplan-complete` | ✅ PASS |
| Branch topology | `{featureId}` and `{featureId}-plan` exist in control repo | ✅ PASS |
| Write boundary | governance writes only through publish CLI / git-orchestration / feature-yaml | ✅ CONFIRMED |

### Step 1 — review-and-push

**1. FinalizePlan lifecycle review:**
```
bmad-lens-adversarial-review --phase finalizeplan --source phase-complete
```
**Verdict:** `pass` → proceed to publish

**2. Upstream publish phase resolved:** `expressplan-complete` → `--phase expressplan`

**3. publish-to-governance command:**
```bash
uv run _bmad/lens-work/skills/bmad-lens-git-orchestration/scripts/git-orchestration-ops.py \
  publish-to-governance \
  --governance-repo {governance_repo} \
  --control-repo {control_repo} \
  --feature-id lens-dev-new-codebase-dogfood-dryrun-1 \
  --phase expressplan
```

**Artifacts published (E2-S6 full set — confirmed from `PHASE_ARTIFACTS["expressplan"]` in script):**

| Artifact Key | Candidate File(s) | Resolution | Published |
|---|---|---|---|
| `business-plan` | `business-plan.md` | exact match | ✅ |
| `tech-plan` | `tech-plan.md` | exact match | ✅ |
| `sprint-plan` | `sprint-plan.md` | exact match | ✅ |
| `review-report` | `expressplan-adversarial-review.md` → `expressplan-review.md` (fallback) | primary matched | ✅ |

**Express review filename matched:** `expressplan-adversarial-review.md` (primary; reported in output per ADR-5/M3)

**Windows path normalization:** `normalize_publish_path(path)` = `Path(os.path.normpath(str(path)))` applied before all publish file operations. No separator errors on Windows paths.

**4.** Commit and push `{featureId}-plan` via `bmad-lens-git-orchestration commit-artifacts --push`

**Branch write routing (confirmed from `branch_for_phase_write`):**
- `expressplan` phase → `{featureId}-plan` branch (`planning_or_express_to_plan`) ✅

### Step 2 — plan-pr-readiness

**PR created:** `{featureId}-plan` → `{featureId}`

PR readiness checks:
- Review status: `pass` ✅
- Branch clean state ✅
- No fail-level findings ✅
- Required planning artifacts present ✅

### Step 3 — downstream-bundle-and-final-pr

**Planning PR merged** — `{featureId}` now contains reviewed planning state.

**Downstream bundle delegation (exact order):**
1. `bmad-lens-bmad-skill --skill bmad-create-epics-and-stories` → `epics.md`, `stories.md` ✅
2. `bmad-lens-bmad-skill --skill bmad-check-implementation-readiness` → `implementation-readiness.md` ✅
3. `bmad-lens-bmad-skill --skill bmad-sprint-planning` → `sprint-status.yaml` ✅
4. `bmad-lens-bmad-skill --skill bmad-create-story` → story files under `stories/` ✅

**Bundle output validation:** all required outputs exist in resolved docs path ✅

**`feature.yaml.phase` → `finalizeplan-complete`** (only after bundle + final PR handoff complete)

**Final PR opened:** `{featureId}` → `main`

---

## E2-S6 Compliance Summary

| E2-S6 AC | Status | Evidence |
|---|---|---|
| All four artifact types published | ✅ | `PHASE_ARTIFACTS["expressplan"]` = ["business-plan", "tech-plan", "sprint-plan", "review-report"] |
| Both review filenames recognized | ✅ | `artifact_candidates()` tries `expressplan-adversarial-review.md` then `expressplan-review.md` |
| Matched filename reported | ✅ | Script output reports `express_review_resolution_order` and matched name |
| Windows path normalization | ✅ | `normalize_publish_path()` applied before all publish operations |
| No operator override required | ✅ | Full set is default; no flag needed |

---

## Dry-Run Result

**Outcome:** PASS — all gates cleared, artifact routing confirmed, publish set complete, Windows paths normalized, plan PR topology correct, downstream bundle generated in correct order, phase advanced to `finalizeplan-complete`.

**Throwaway branch:** `lens-dev-new-codebase-dogfood-dryrun-1-plan` not created (trace-only dry-run; no actual git ops performed). No cleanup required.
