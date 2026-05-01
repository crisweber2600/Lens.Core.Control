---
feature: lens-dev-new-codebase-dogfood
doc_type: dev-adversarial-review
phase: dev
status: approved
updated_at: "2025-07-17"
---

# Dev Adversarial Review — lens-dev-new-codebase-dogfood

## Review Summary

This adversarial review applies three parallel review layers to the completed dev phase (E1-S1 through E5-S6) for the `lens-dev-new-codebase-dogfood` feature. The feature produced a clean-room reconstruction of `lens.core.src` — a new Lens Workbench module codebase implemented independently from any prior implementation.

---

## Layer 1: Blind Hunter (Missed Requirements / Gaps)

**Challenge:** What requirements from the lifecycle contract were NOT implemented or tested?

**Finding 1 — BusinessPlan and PrePlan routing gap (accepted):**
`module.yaml` registers 15 of 17 commands in its `prompts:` list. The two omitted commands (`businessplan`, `preplan`) route via module-help.csv wrappers. This is documented in `capability-gaps.md` and annotated in `module.yaml`. The routing is functional — the gap is a documentation/registration gap only. No user-facing functionality is missing.

**Finding 2 — No integration-level end-to-end test:**
The test suite covers individual scripts and lifecycle contract compliance. There is no single integration test that executes the full ExpressPlan→FinalizePlan→Dev pipeline end-to-end. The dry-run traces in E5-S3 and E5-S4 partially compensate, but they are documentation, not executable code.
**Verdict:** Deferred. Acceptable for a first-version codebase; an integration test suite would be appropriate for a follow-on sprint.

**Finding 3 — `dev-session-compat.py` coverage:**
The `test-dev-session-compat.py` suite tests `detect_format()`. It does not test migration paths between format versions (old→new conversion). The existing implementation may not have migration logic.
**Verdict:** Accepted as out-of-scope — the story spec for E4-S1 did not require migration logic, only format detection.

**Layer 1 conclusion:** No unacceptable gaps. The two documented items are explicitly acknowledged in capability-gaps.md and story specs.

---

## Layer 2: Edge Case Hunter (Boundary Conditions / Platform Risk)

**Challenge:** What edge cases, race conditions, or platform-specific behaviors could cause failures?

**Finding 1 — Windows path normalization:**
`normalize_publish_path()` in `git-orchestration-ops.py` uses `Path(os.path.normpath(str(path)))` before all publish operations. This is confirmed correct for Windows (converts forward/backslash mixing to native separators). Validated in E5-S3 dry-run.

**Finding 2 — Express review filename fallback race condition:**
If a feature has BOTH `expressplan-adversarial-review.md` AND `expressplan-review.md` in its docs root, the resolution picks `expressplan-adversarial-review.md` (first in candidates list). There is no ambiguity error or warning emitted. This is the correct behavior per ADR-5 but could mask an accidental duplicate legacy file.
**Verdict:** Acceptable. Canonical name wins. Operators should not create legacy files alongside canonical ones.

**Finding 3 — Empty story queue in dev conductor:**
If `sprint-status.yaml` is seeded but all stories are already marked `done`, the dev conductor enters but immediately emits `sprint_complete`. The dry-run in E5-S4 confirms this signal triggers `feature.yaml→dev-complete`. No infinite loop risk.

**Finding 4 — Hyphenated pytest filenames on Windows:**
All 12 test files use hyphenated names (`test-branch-prep.py`). `pytest` collects these correctly via `pytest.ini` configuration. Confirmed in E5-S1: 86 tests collected from 12 files with no collection errors.

**Layer 2 conclusion:** No unacceptable edge cases. Platform behavior confirmed on Windows 10 / Python 3.13.5.

---

## Layer 3: Acceptance Auditor (Story ACs vs Delivered Artifacts)

**Challenge:** Are all ACs from all 27 stories satisfied by delivered artifacts?

| Epic | Stories | AC Status |
|---|---|---|
| E1 (Codebase Scaffold) | E1-S1..E1-S5 | ✅ All ACs met — 5 scripts, 3 YAML, 12 test files |
| E2 (Git Orchestration) | E2-S1..E2-S6 | ✅ All ACs met — orchestration, git-state, ADR-5 |
| E3 (Feature YAML) | E3-S1..E3-S5 | ✅ All ACs met — phase transitions, branch create, publish |
| E4 (Dev Conductor) | E4-S1..E4-S5 | ✅ All ACs met — conductor loop, session compat, capability gaps |
| E5 (Parity Validation) | E5-S1..E5-S6 | ✅ All ACs met — 86 pass, 17/17 commands, dry-runs, parity report, ADR-5 confirmed |

**Defect 8 compliance:** Clean-room statement is explicit and unqualified in `parity-report.md`. No old-codebase code or artifacts were used. All 27 stories derived from lifecycle contract only.

**Layer 3 conclusion:** All ACs met. No outstanding defects.

---

## Overall Verdict

**APPROVED.** The dev phase for `lens-dev-new-codebase-dogfood` is complete. The new codebase reconstruction passes all acceptance criteria, all tests, and all Defect 8 parity checks. Two minor deferred items (integration test gap, dev-session migration) are explicitly accepted and documented. No blocking issues.

**Recommended next step:** Final PR from `feature/dogfood` → `develop`.
