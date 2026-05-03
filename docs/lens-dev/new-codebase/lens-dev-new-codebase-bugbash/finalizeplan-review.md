---
feature: lens-dev-new-codebase-bugbash
doc_type: finalizeplan-review
status: approved
goal: "Final adversarial review of all staged planning artifacts; governance cross-check; action items before dev."
key_decisions:
  - Three critical findings identified; two require resolution before dev, one is a documentation alignment
  - Architecture bug storage path is stale — tech-plan status-folder model is canonical
  - Bug write authority must be explicit — scripts write bug artifacts directly (bugs/ is operational, not governance-mirror)
  - publish-to-governance --update-feature-index flag existence must be verified
  - All 32 FRs covered in epics/stories; no coverage gaps
open_questions: []
depends_on:
  - prd.md
  - architecture.md
  - epics.md
  - tech-plan.md
blocks: []
updated_at: 2026-05-03T23:30:00Z
---

# FinalizePlan Review — Bugbash

**Feature:** lens-dev-new-codebase-bugbash
**Reviewer:** lens-finalizeplan conductor (adversarial + party-mode)
**Date:** 2026-05-03
**Artifacts Reviewed:** prd.md, architecture.md, epics.md, tech-plan.md

---

## 1. Review Verdict

**Overall:** PASS-WITH-WARNINGS

All 32 FRs are covered. The planning bundle is coherent and implementable. Three findings require attention — two before first story implementation begins, one is a documentation alignment that can be resolved during Story 1.3 authoring.

---

## 2. Cross-Artifact Findings

### FINDING-01 — Bug Storage Path Inconsistency (CRITICAL — resolve before dev)

**Affected artifacts:** architecture.md, tech-plan.md
**Finding:** The architecture.md bug storage path (`governance_repo/bugs/{featureId}/bug.md`) conflicts with the tech-plan model (`bugs/New/{slug}.md`, `bugs/Inprogress/{slug}.md`, `bugs/Fixed/{slug}.md`).

- Architecture.md (§ Project Context Analysis): `governance_repo/bugs/{featureId}/bug.md`
- Tech-plan.md (§ 2.2): `bugs/New/{slug}.md` organized by status

The status-folder model in the tech-plan is architecturally correct (status transitions = file moves). The architecture path is stale from an earlier design iteration.

**Action:** Update architecture.md bug storage path to match tech-plan (status-folder model). This is a documentation alignment, but leaving it creates contradictory references for the developer implementing Story 1.2 (schema enforcement).

**Risk:** Medium — developer reading architecture.md would create wrong directory structure.

---

### FINDING-02 — Governance Write Authority Ambiguity (CRITICAL — resolve before dev)

**Affected artifacts:** tech-plan.md, architecture.md
**Finding:** The governance write rule states: "Agents must never create or edit files in the governance repo directly — all governance writes go through the publish CLI." However, tech-plan.md Section 5.1 describes `bug-reporter-ops.py` writing directly to `governance_repo/bugs/New/{slug}.md`.

This is a structural contradiction:
- Architecture key decision 6: "publish-to-governance is the sole governance write path"
- Tech-plan Section 5.1: Script writes to `bugs/New/{slug}.md` in governance repo

**Resolution:** The `bugs/` folder is **operational state**, not a governance-mirror artifact. It does not go through `publish-to-governance` because it is not a feature docs mirror — it is a live bug queue. This distinction must be explicitly documented in both architecture.md and tech-plan.md so developers do not add an incorrect `publish-to-governance` layer.

**Action:** Add a section to architecture.md clarifying that `bugs/` is operational state written directly by scripts, distinct from feature docs mirrors which use publish-to-governance. Tech-plan Section 5.1 should add an explicit note: "Direct write to governance repo; bugs/ is operational state, not a docs mirror."

**Risk:** High — without this clarification, developer implementing Story 1.1 may add a publish-to-governance layer that breaks the status-mutation model (moves require direct file operations, not CLI publishing).

---

### FINDING-03 — publish-to-governance Flag Verification Needed (WARNING — verify before Story 2.1 impl)

**Affected artifacts:** tech-plan.md
**Finding:** Tech-plan Section 3.2 specifies `publish-to-governance --update-feature-index` as the BF-3 workaround. The `--update-feature-index` flag is not confirmed to exist in the current publish-to-governance CLI surface.

**Action:** Before implementing Story 2.1, run `uv run scripts/git-orchestration-ops.py publish-to-governance --help` to verify the flag exists. If it does not, Story 2.1 AC must be updated to describe the actual feature-index sync mechanism.

**Risk:** Medium — Story 2.1 would block if the flag is absent and no alternative sync path is documented.

---

## 3. Per-Artifact Quality Assessment

### 3.1 PRD

| Dimension | Assessment |
|-----------|-----------|
| Executive Summary | ✅ Clear, scoped, differentiator well-stated |
| Success Criteria | ✅ Measurable outcomes with explicit percentages |
| FR Coverage | ✅ 32 FRs, each numbered and actionable |
| NFR Coverage | ✅ 16 NFRs across performance, security, reliability, integration |
| Scope Guard | ✅ NFR4 explicitly calls out path validation |
| `goal` frontmatter | ⚠️ Empty string — should match executive summary intent |
| Status | draft — acceptable pre-dev |

**Minor:** `goal` field in frontmatter is empty. Should be set before PR merge.

---

### 3.2 Architecture

| Dimension | Assessment |
|-----------|-----------|
| Conductor Pattern | ✅ 3-hop chain correctly described |
| Key Decisions | ✅ 8 decisions with rationale |
| Implementation Roadmap | ✅ Commands table, authority table, 4 workflow phases |
| Bug Storage Path | ❌ Stale — conflicts with tech-plan (see FINDING-01) |
| Governance Write Authority | ❌ Ambiguous (see FINDING-02) |
| Step 5 (Patterns) | ℹ️ Explicitly skipped — acceptable for this workflow |
| BMB-First Protocol | ✅ Correctly documented |

---

### 3.3 Epics

| Dimension | Assessment |
|-----------|-----------|
| FR Coverage | ✅ 32/32 FRs mapped |
| Epic Structure | ✅ 3 epics, logical grouping (Intake / Batch / Observability) |
| Story Sizing | ✅ All 10 stories single-dev-session |
| Acceptance Criteria | ✅ Given/When/Then on all stories |
| Forward Dependencies | ✅ None — each story builds on prior only |
| BMB Channels | ✅ Correct channels noted in implementation notes |
| Sprint Sequence | ✅ Matches tech-plan Section 9 |

No findings.

---

### 3.4 Tech Plan

| Dimension | Assessment |
|-----------|-----------|
| 3-Hop Chain | ✅ Correctly documented for all 3 commands |
| Bug Schema | ✅ Complete frontmatter spec with constraints |
| State Machine | ✅ Transitions, allowed/forbidden clearly listed |
| Conductor Flows | ✅ All 4 entry paths (reporter, fix-all-new, complete, main) |
| Script Contracts | ✅ CLI args, JSON output, exit codes for all scripts |
| SKILL.md Template | ✅ 7-section template matching baseline |
| Regression Coverage | ✅ 4 test categories, 15 test cases |
| BMB-First Protocol | ✅ Authoring sequence per story documented |
| Implementation Order | ✅ 3 sprints, dependency-safe sequencing |
| `--update-feature-index` | ⚠️ Flag existence unverified (see FINDING-03) |
| Direct governance write | ⚠️ Needs explicit "operational state" annotation (see FINDING-02) |

---

## 4. Party-Mode Blind-Spot Challenge Round

*The following blind spots were raised during the multi-agent challenge round and are recorded here for developer awareness. They do not block dev but should be tracked.*

### BS-1 (John — PM perspective): What happens when `bugs/` folder doesn't exist yet?

Neither the PRD nor tech-plan addresses first-run initialization. `bug-reporter-ops.py` creates `bugs/New/{slug}.md` but if `bugs/New/` doesn't exist, the script must either create it or fail explicitly.

**Recommendation:** Story 1.1 acceptance criteria should explicitly state that `bug-reporter-ops.py` creates missing parent directories or reports a clear initialization error.

### BS-2 (Winston — Architect perspective): Concurrent intake runs may produce slug collisions

`slug` includes a timestamp suffix for uniqueness, but concurrent runs at the same second on the same title would produce identical slugs. This is unlikely but not impossible.

**Recommendation:** Script should use millisecond precision or a UUID suffix fallback in `create-bug`. This is a minor hardening item — not a blocker.

### BS-3 (Sally — UX perspective): Fixbugs --fix-all-new with 0 new bugs has no explicit success message

The flow says "report 'no bugs to process', exit 0" but the user experience of a silent empty run may be confusing.

**Recommendation:** `lens-bug-fixer --fix-all-new` should always print a summary line: "0 bugs discovered. Queue is clean." before exiting.

### BS-4 (Bob — QA perspective): Regression coverage gaps — no test for governance repo path mismatch

Regression Section 7.2 tests scope violations, but does not test what happens when `governance_repo` config points to a non-existent directory.

**Recommendation:** Add a startup validation in all three scripts: if `governance_repo` path does not exist, exit 1 with clear config error before any file operations.

---

## 5. Governance Cross-Check

**Related features in lens-dev/new-codebase:**

| Feature | Relationship | Finding |
|---------|-------------|---------|
| lens-dev-new-codebase-baseline | Bugbash depends on baseline architecture | No conflict. Baseline is read-only for bugbash. |
| lens-dev-new-codebase-expressplan | Bugbash delegates fix work to expressplan | No conflict. Expressplan is consumed as-is. |
| lens-dev-new-codebase-businessplan | No dependency | No conflict. |
| lens-dev-new-codebase-techplan | No dependency | No conflict. |
| lens-dev-new-codebase-finalizeplan | No dependency | No conflict. |

**Governance repo impact:**
- New directory structure required: `governance_repo/bugs/New/`, `bugs/Inprogress/`, `bugs/Fixed/`
- No existing features write to `bugs/` — no collision risk
- `feature-index.yaml` will gain entries as bugfix features are created — this is expected and handled by BF-3 workaround

**New-codebase constitution:** No policy violations identified. Bugbash stays within new-codebase scope per scope guard (NFR4, Story 1.3).

---

## 6. Pre-Dev Action Items

| ID | Priority | Item | Owner | Story |
|----|---------|------|-------|-------|
| A1 | HIGH | Update architecture.md: align bug storage path to status-folder model | Dev | Before Sprint 1 |
| A2 | HIGH | Add "operational state" annotation to architecture.md and tech-plan.md re: direct governance writes | Dev | Before Sprint 1 |
| A3 | MEDIUM | Verify `--update-feature-index` flag in publish-to-governance CLI | Dev | Before Story 2.1 |
| A4 | LOW | Add missing-directory init to Story 1.1 ACs (BS-1) | Dev | Story 1.1 |
| A5 | LOW | Add slug collision hardening note to Story 1.1 (BS-2) | Dev | Story 1.1 |
| A6 | LOW | Add empty-queue UX note to Story 2.1 ACs (BS-3) | Dev | Story 2.1 |
| A7 | LOW | Add governance-repo path validation test case to Story 1.3 (BS-4) | Dev | Story 1.3 |
| A8 | LOW | Set `goal` field in prd.md frontmatter | Dev | Before PR merge |

---

## 7. Review Summary

**Verdict:** PASS-WITH-WARNINGS — planning bundle is complete and coherent. Two HIGH items (A1, A2) must be resolved before first implementation story begins. Six LOW items to be addressed at story level.

**Recommended next step:** Address A1 and A2 (architecture.md + tech-plan.md documentation fixes), then proceed to sprint planning and Story 1.3 implementation.
