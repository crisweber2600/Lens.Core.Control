---
feature: lens-dev-new-codebase-bugbash
doc_type: finalizeplan-review
status: approved
goal: "Final adversarial review of all staged planning artifacts; governance cross-check; action items before dev and post-dev validation."
key_decisions:
  - Three pre-dev critical findings identified; all resolved before/during implementation
  - Post-dev review (manual-rerun): PASS-WITH-WARNINGS with 2 MEDIUM follow-ups
  - featureId empty-string guard missing at script layer — CLI bypass risk (FINDING-PD01)
  - Orphaned Inprogress recovery path undocumented — operational gap (FINDING-PD03)
  - All 32 FRs implemented; 61/61 regression tests passing
open_questions: []
depends_on:
  - prd.md
  - architecture.md
  - epics.md
  - tech-plan.md
blocks: []
updated_at: 2026-05-03T00:00:00Z
post_dev_updated_at: 2026-05-03T02:00:00Z
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
| `goal` frontmatter | ✅ Populated — matches executive summary intent |
| Status | draft — acceptable pre-dev |

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
| A1 | HIGH | ~~Update architecture.md: align bug storage path to status-folder model~~ **RESOLVED** — architecture.md starter template updated to match status-folder model | Dev | ✅ Done |
| A2 | HIGH | ~~Add "operational state" annotation to architecture.md and tech-plan.md re: direct governance writes~~ **RESOLVED** — explicit write-authority clarification added to both files | Dev | ✅ Done |
| A3 | MEDIUM | Verify `--update-feature-index` flag in publish-to-governance CLI | Dev | Before Story 2.1 |
| A4 | LOW | Add missing-directory init to Story 1.1 ACs (BS-1) | Dev | Story 1.1 |
| A5 | LOW | Add slug collision hardening note to Story 1.1 (BS-2) | Dev | Story 1.1 |
| A6 | LOW | Add empty-queue UX note to Story 2.1 ACs (BS-3) | Dev | Story 2.1 |
| A7 | LOW | Add governance-repo path validation test case to Story 1.3 (BS-4) | Dev | Story 1.3 |
| A8 | LOW | ~~Set `goal` field in prd.md frontmatter~~ **RESOLVED** — goal field is populated | Dev | ✅ Done |

---

## 7. Review Summary

**Verdict:** PASS-WITH-WARNINGS — planning bundle is complete and coherent. HIGH items (A1, A2) resolved in this PR. Six LOW items to be addressed at story level.

**Recommended next step:** Proceed to sprint planning and Story 1.3 implementation.

---

---

# Post-Dev Adversarial Review (Manual Rerun — 2026-05-03)

**Source:** manual-rerun after dev completion
**Reviewer:** lens-adversarial-review skill
**Scope:** Implementation artifacts vs. planning contract; blind-spot follow-ups from prior review
**Regression gate:** 61/61 passing

---

## Pre-Dev Action Items — Final Disposition

| ID | Priority | Item | Status |
|----|---------|------|--------|
| A1 | HIGH | Bug storage path aligned to status-folder model | ✅ Resolved in implementation |
| A2 | HIGH | bugs/ is operational state; direct script writes explicit | ✅ Resolved in implementation |
| A3 | MEDIUM | `--update-feature-index` flag superseded; `init-feature-ops.py create` handles feature-index natively | ✅ Resolved by tech-plan update |
| A4 | LOW (BS-1) | `mkdir(parents=True, exist_ok=True)` in bug-reporter-ops.py + test | ✅ Resolved in Story 1.1 |
| A5 | LOW (BS-2) | SHA256(title+description)[:8] slug — collision-resistant for distinct bugs; idempotent for same bug | ✅ Resolved by design choice |
| A6 | LOW (BS-3) | "0 bugs discovered. Queue is clean." message in SKILL.md Phase 1 | ✅ Resolved in Story 2.1 |
| A7 | LOW (BS-4) | `assert_governance_repo_exists()` in all 3 scripts; 3 exit-1 tests | ✅ Resolved in Story 1.3 |

---

## Post-Dev Findings

### FINDING-PD01 — featureId emptiness not validated at script layer (MEDIUM)

**Affected:** `bug-fixer-ops.py` `cmd_move_to_inprogress`
`validate_transition("New", "Inprogress")` checks state machine only. `--feature-id ""` accepted without error. SKILL.md orchestration path is safe (Phase 2 always runs before Phase 3), but a direct CLI call bypasses this.
**Action:** Add `if not feature_id.strip(): sys.exit(1)` guard in `cmd_move_to_inprogress`.

### FINDING-PD02 — Planning artifacts still `status: draft` (LOW)

**Affected:** `prd.md`, `tech-plan.md`
Per repo convention, dev-ready artifacts carry `status: approved`. Both remain `status: draft`.
**Action:** Update both files to `status: approved`.

### FINDING-PD03 — Orphaned Inprogress recovery path undocumented (MEDIUM)

**Affected:** `bmad-lens-bug-fixer` SKILL.md
If `--fix-all-new` is interrupted between Phase 3 and Phase 4, bugs are stranded in `Inprogress/` with no `--fix-all-new` visibility. No recovery runbook exists.
**Action:** Add "Error Recovery" section to `bmad-lens-bug-fixer/SKILL.md`: interrupted sessions should call `--complete {featureId}` or inspect `bugbash-ops.py status-summary` output.

### FINDING-PD04 — pyyaml fallback parses colon-embedded values incorrectly (LOW)

**Affected:** `bug-fixer-ops.py`, `bugbash-ops.py`
The `yaml=None` fallback `_parse_frontmatter` fails on values like `"Login: Session expires"`. Correct under `uv run --script`; latent risk under direct `python script.py` without pyyaml installed.
**Action:** Assert pyyaml importable in tests; raise `ImportError` explicitly in fallback path.

### FINDING-PD05 — Tech-plan regression coverage counts stale (LOW)

**Affected:** `tech-plan.md` § 7
Tech-plan specifies 15 test cases; implementation delivered 61 (scope-guard:10, schema:21, reporter:9, fixer:16, conductor:5).
**Action:** Update § 7 test counts.

---

## Post-Dev Party-Mode Blind-Spot Challenge

**Bob (QA):** Is `_atomic_move` (write-verify-delete) safe under concurrent fixer sessions on Windows NTFS? Two sessions processing the same slug would race on the source delete.

**Winston (Architect):** The `_update_frontmatter` line parser is correct for the current flat schema. Is the frontmatter spec intentionally frozen, or should a `schema_version: 1` field protect against silent future breakage?

**Sally (UX):** When Phase 4 (expressplan delegation) fails, the SKILL.md outcome report has no ❌ lines — only missing ✅ lines. Is this detectable without also running `bugbash-ops.py status-summary`?

### Open Blind-Spot Questions
1. Concurrent write safety: does `_atomic_move` need an OS-level lock for Windows NTFS?
2. Orphaned Inprogress recovery: is the runbook surfaced in `status-summary` output or only in SKILL.md?
3. Phase 4 failure visibility: is a partial outcome report (no ❌ lines) distinguishable from a successful zero-bug batch?
4. Should `featureId` emptiness be a schema-level enforcement or CLI-level guard?
5. Should the bug frontmatter spec include `schema_version: 1` for forward compatibility?

---

## Post-Dev Verdict

**PASS-WITH-WARNINGS**

All 10 stories delivered. All pre-dev action items resolved. 61/61 regression tests passing. Two MEDIUM findings (PD01, PD03) are addressable before PR merge without story-level work. Two LOW findings (PD02, PD05) are documentation updates only.
