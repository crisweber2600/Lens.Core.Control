---
feature: lens-dev-new-codebase-split-feature
doc_type: implementation-readiness
status: approved
goal: "Assess implementation readiness for the split-feature command rewrite: script fixes, conductor rewrite, and test suite completion."
key_decisions:
  - Feature is ready for dev handoff; three script gaps must be closed as the first sprint priority
  - SKILL.md rewrite can proceed in Sprint 1 alongside script fixes (no ordering dependency between them)
  - Test suite additions in E3-S1 are blocked by E1 script fixes (tests need correct behavior to assert against)
open_questions: []
depends_on:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-split-feature/tech-plan.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-split-feature/business-plan.md
blocks: []
updated_at: '2026-05-01T00:00:00Z'
---

# Implementation Readiness — Rewrite split-feature Command

## Readiness Assessment

### Overall Verdict: READY WITH CONDITIONS

The feature is ready for dev handoff. The existing `split-feature-ops.py` (565 lines)
and test file (87 passing tests) provide a solid foundation. Three implementation gaps
identified in the adversarial review must be closed before Sprint 1 is marked done.
The SKILL.md rewrite follows in Sprint 1 alongside the script fixes.

| Condition | Status | Owner |
|-----------|--------|-------|
| Status normalization (BS-1) — `in_progress`, `IN_PROGRESS`, `in progress` variants | OPEN — must fix | Dev (E1-S1) |
| Feature-index.yaml duplicate check (BS-3) — read index, not file.exists | OPEN — must fix | Dev (E1-S2) |
| Sprint-status.yaml list-format parsing — `stories: [{id, status}]` | OPEN — must fix | Dev (E1-S3) |
| SKILL.md thin-conductor rewrite | PENDING | Dev (E2-S1) |
| Missing test cases closed | PENDING — after E1 | Dev (E3-S1) |

---

## Risk Assessment

### Risk Register

| ID | Risk | Likelihood | Impact | Mitigation | Status |
|----|------|-----------|--------|-----------|--------|
| R1 | Status normalization gap causes false-negative on `in_progress` old-codebase stories | Medium | High — in-progress stories could incorrectly pass the hard gate | E1-S1: add normalize_status() helper and apply in both validation paths | OPEN |
| R2 | Feature-index.yaml duplicate check gap allows double-write if feature directory was cleaned up | Low | High — corrupts feature-index.yaml state | E1-S2: read feature-index.yaml explicitly before first write | OPEN |
| R3 | List-format sprint-status.yaml not parsed — all stories treated as unknown status | Medium | Medium — validate-split false-pass when sprint-status.yaml uses list format | E1-S3: extend _extract_statuses_from_yaml_str to handle list format | OPEN |
| R4 | SKILL.md rewrite introduces regression in split-feature activation flow | Low | Medium — split-feature would fail for users during rewrite window | E2-S1: rewrite follows thin-conductor spec; all tests pass before merge | OPEN |
| R5 | Test suite additions reveal additional bugs not in the original 87 tests | Low | Low — bugs would be caught before final PR | E3-S2: run full suite, fix any failures found | OPEN |

### Closed / Mitigated Risks

| ID | Risk | Mitigation Applied |
|----|------|-------------------|
| R6 | SAFE_ID_PATTERN validation missing | SAFE_ID_PATTERN validation already present at argument parse time in both cmd_create_split_feature and cmd_move_stories | CLOSED |
| R7 | Atomic write pattern missing for YAML artifacts | atomic_write_yaml() already implemented and used for all YAML writes | CLOSED |
| R8 | Dry-run coverage incomplete | --dry-run supported by create-split-feature and move-stories; 87 passing tests include dry-run regression coverage | CLOSED |
| R9 | Script subcommand surface not preserved | All three subcommands (validate-split, create-split-feature, move-stories) present and tested | CLOSED |

---

## Prerequisites Checklist

### Planning prerequisites ✓
- [x] Business plan produced and reviewed (tech-plan §5 references)
- [x] Tech plan produced and reviewed (finalizeplan-review.md status: responses-recorded)
- [x] FinalizePlan adversarial review completed (pass-with-warnings; all findings responded)
- [x] Planning PR (#37) merged into base branch `lens-dev-new-codebase-split-feature`
- [x] Epics defined (4 epics)
- [x] Stories with acceptance criteria defined (9 stories)
- [x] Old-codebase reference located at `TargetProjects/lens-dev/old-codebase/lens.core.src/`

### Implementation prerequisites (verify in Sprint 1)
- [ ] E1-S1: Status normalization implemented in split-feature-ops.py
- [ ] E1-S2: Feature-index.yaml duplicate detection implemented
- [ ] E1-S3: List-format sprint plan parsing implemented
- [ ] E2-S1: SKILL.md rewritten as thin conductor
- [ ] E2-S2: Prompt files verified correct

### Dev handoff prerequisites (Sprint 2 exit)
- [ ] All 10 test class categories from tech-plan §6 covered
- [ ] Full test suite passing (≥ 95 tests, zero failures)
- [ ] Module discovery registration confirmed
- [ ] feature.yaml target_repos includes lens.core.src
- [ ] Final PR open
- [ ] feature.yaml phase = finalizeplan-complete
