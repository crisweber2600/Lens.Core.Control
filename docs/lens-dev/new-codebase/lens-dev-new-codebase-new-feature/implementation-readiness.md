---
feature: lens-dev-new-codebase-new-feature
doc_type: implementation-readiness
status: approved
goal: "Validate planning artifacts are complete and aligned before dev begins"
key_decisions:
  - Feature is ready for dev. All planning artifacts are present and internally consistent.
  - Merge ordering with lens-dev-new-codebase-new-service must be confirmed before both features enter dev in the same sprint.
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T15:30:00Z
---

# Implementation Readiness — New Feature Command

**Feature:** lens-dev-new-codebase-new-feature  
**Assessed:** 2026-04-27  
**Overall Verdict:** **READY FOR DEV** — all critical and high gates pass; two medium items require pre-sprint coordination

---

## Artifact Checklist

| Artifact | Present | Status | Notes |
|---|---|---|---|
| business-plan.md | ✅ | Approved | Executive summary, stakeholders, success criteria documented |
| tech-plan.md | ✅ | Approved | 6 ADRs, API contracts, data model, testing strategy; ADR 6 covers fetch-context |
| sprint-plan.md | ✅ | Approved | 4 sprints, 12 stories, Definition of Done updated for fetch-context |
| expressplan-review.md | ✅ | Approved | Pass-with-warnings; H1 and H2 resolved via expressflow exception + user decision |
| finalizeplan-review.md | ✅ | Approved | Pass; 0 critical, 0 high, 3 medium, 1 low; A1 closed |
| epics.md | ✅ | Approved | 4 epics aligned to 4 sprints with cross-epic dependency map |
| stories.md | ✅ | Approved | 12 stories with acceptance criteria; fetch-context NF-4.3 fully specified |

---

## Requirements Traceability

| Requirement | Source | Epic/Story | Coverage |
|---|---|---|---|
| `new-feature` command available via installed stub | business-plan.md §Goals | Epic 1 / NF-1.1 | ✅ Full |
| `new-feature` command available via release prompt | business-plan.md §Goals | Epic 1 / NF-1.1 | ✅ Full |
| Progressive disclosure interaction (name/domain/service → track) | tech-plan.md §Prompt Contract | Epic 1 / NF-1.2 | ✅ Full |
| Canonical feature identity `{domain}-{service}-{featureSlug}` | tech-plan.md ADR 2 | Epic 2 / NF-2.1 | ✅ Full |
| `featureSlug` stored separately | tech-plan.md ADR 2 | Epic 2 / NF-2.1 | ✅ Full |
| v4 feature.yaml schema | tech-plan.md §Data Model | Epic 2 / NF-2.1 | ✅ Full |
| feature-index.yaml entry with all required fields | tech-plan.md §Script Contract | Epic 2 / NF-2.1 | ✅ Full |
| Validation gates before writes | tech-plan.md §Script Contract | Epic 2 / NF-2.2 | ✅ Full |
| Dry-run mode | tech-plan.md §Script Contract | Epic 2 / NF-2.3 | ✅ Full |
| Start phase from lifecycle.yaml | tech-plan.md ADR 3 | Epic 3 / NF-3.1 | ✅ Full |
| Control-repo branch commands via git-orchestration | tech-plan.md ADR 4 | Epic 3 / NF-3.2 | ✅ Full |
| Activation commands via switch | tech-plan.md ADR 4 | Epic 3 / NF-3.2 | ✅ Full |
| Express track defers planning PR | tech-plan.md §Script Contract | Epic 3 / NF-3.3 | ✅ Full |
| `--execute-governance-git` fail-fast on dirty repos | tech-plan.md ADR 5 | Epic 4 / NF-4.1 | ✅ Full |
| `fetch-context` subcommand full parity | tech-plan.md ADR 6 | Epic 4 / NF-4.3 | ✅ Full |
| Help/manifest alignment | tech-plan.md §Open Questions | Epic 4 / NF-4.2 | ✅ Scoped (conditional) |
| Parity tests pass before implementation | sprint-plan.md §Cross-Sprint Dependencies | NF-1.3 → NF-2.1 gate | ✅ Full |

**Traceability verdict:** All requirements from business-plan.md and tech-plan.md are covered by at least one story. No orphaned requirements found.

---

## Epics vs. Stories Alignment

| Epic | Stories | Acceptance Criteria Coverage | Issues |
|---|---|---|---|
| Epic 1: Command Surface | NF-1.1, NF-1.2, NF-1.3 | Prompt paths, skill contract, test skeletons — all defined | None |
| Epic 2: Script-Level Creation | NF-2.1, NF-2.2, NF-2.3 | Identity, validation, dry-run — all defined | NF-2.1 gate: parity tests from NF-1.3 must be green at acceptance |
| Epic 3: Git and Lifecycle | NF-3.1, NF-3.2, NF-3.3 | Phase routing, command generation, express deferral — all defined | None |
| Epic 4: Context and Release | NF-4.1, NF-4.2, NF-4.3 | Governance git, help/manifests, fetch-context — all defined | NF-4.2 may be a no-op if 17-command sweep owns it |

**Alignment verdict:** All epic acceptance definitions are traceable to specific story acceptance criteria. No orphaned stories found. No epic with undefined acceptance.

---

## Risk Register

| Risk | Severity | Mitigation | Status |
|---|---|---|---|
| Feature ID drift between old and new codebase | High | NF-1.3 parity tests define the exact expected output schema; NF-2.1 acceptance requires those tests to pass green | ✅ Mitigated |
| `fetch-context` parity gap | High | ADR 6 specifies full parity contract; NF-4.3 is L-estimate, non-deferrable, with 8 test case requirements | ✅ Mitigated (user decision) |
| Concurrent `init-feature-ops.py` edits (new-feature + new-service) | High | A2 action item: coordinate PR merge order before both enter dev | ⚠️ Requires action before dev |
| Governance partial writes after failed preflight | Medium | ADR 5 mandates preflight before writes; NF-4.1 dirty-repo test covers this | ✅ Mitigated |
| Sprint 4 sizing (3 stories incl. 1 L) | Medium | NF-4.2 can be skipped if 17-command sweep owns it, reducing Sprint 4 to NF-4.1 + NF-4.3 | ✅ Acceptable |
| BMB-first rule (informational gate) | Medium | Confirmed as informational-gate; non-blocking for dev | ⚠️ Confirm with lead before Sprint 1 |
| Quickplan alias not tested | Low | NF-1.3 includes quickplan alias test skeleton | ✅ Mitigated |
| Preplan artifacts absent | Low | Expressflow exception rationale documented in feature.yaml | ✅ Accepted |

---

## Cross-Feature Readiness

| Dependency | Status | Required Before |
|---|---|---|
| `lens-dev-new-codebase-baseline` | Complete | ✅ Available now |
| `lens-dev-new-codebase-new-service` (script conflict) | expressplan-complete | Coordinate PR merge order before Sprint 3+ (NF-2.x onward) |
| `lens-dev-new-codebase-switch` (activation commands) | Unknown | Validate before NF-3.2 integration tests |
| `lens-dev-new-codebase-preflight` | Working (passes in CI) | ✅ Available now |

---

## Gates Summary

| Gate | Status | Notes |
|---|---|---|
| All required planning artifacts present | ✅ PASS | 7/7 artifacts present and approved |
| All requirements traceable to stories | ✅ PASS | 100% traceability |
| No orphaned stories | ✅ PASS | All 12 stories map to epics |
| No critical or high open findings | ✅ PASS | finalizeplan-review verdict: pass (0C/0H) |
| fetch-context in scope and specified | ✅ PASS | ADR 6 + NF-4.3 with L estimate |
| Cross-sprint dependencies documented | ✅ PASS | Dependency chain: NF-1.3 → NF-2.x → NF-3.x → NF-4.x |
| Merge-order coordination required | ⚠️ ACTION | Coordinate with new-service lead before Sprint 2 dev start |
| BMB-first rule confirmed | ⚠️ ACTION | Confirm gate type before Sprint 1 |

**Overall: READY FOR DEV** — proceed to `/dev` and Sprint 1. Resolve the two ⚠️ items before Sprint 2.
