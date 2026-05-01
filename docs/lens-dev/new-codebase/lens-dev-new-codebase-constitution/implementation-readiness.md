---
feature: lens-dev-new-codebase-constitution
doc_type: implementation-readiness
status: approved
goal: "Assess readiness for dev handoff of the constitution rewrite after FinalizePlan bundling."
key_decisions:
  - The feature is implementation-ready once the bundle is present; remaining gates are release-hardening conditions, not planning blockers.
  - No external target repo is required; the implementation surface is the current control repo under lens.core/_bmad/lens-work.
  - Read-only and path-safety regressions are mandatory before merge, even though they do not block implementation start.
open_questions:
  - Does any downstream caller need a specific repo-level fixture before the first implementation PR?
  - Is a broader caller audit for sensing_gate_mode needed before marking the feature complete?
depends_on:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-constitution/stories.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-constitution/finalizeplan-review.md
blocks: []
updated_at: 2026-05-01T16:30:00Z
---

# Implementation Readiness - Constitution Command Rewrite

## Readiness Assessment

### Overall Verdict: READY WITH CONDITIONS

The constitution feature is ready for implementation handoff. The planning packet now
contains the express-path context, dedicated architecture coverage, a finalized story list,
and a clear release-hardening lane. Remaining items are execution gates inside the story
set, not reasons to keep FinalizePlan open.

| Condition | Status | Owner | Blocks |
| --- | --- | --- | --- |
| Express-state alignment remains the governance source of truth | READY | Dev / maintainer | None |
| Read-only boundary preserved during implementation | READY | Dev | None |
| Negative safety regressions land before merge | PENDING | Dev | E4-S2, release gate |
| Caller follow-up notes recorded before final PR | PENDING | Dev / maintainer | E4-S3 |

---

## Delivery Risks

| ID | Risk | Likelihood | Impact | Mitigation | Status |
| --- | --- | --- | --- | --- | --- |
| R1 | Partial-hierarchy fix regresses merge behavior for existing callers | Medium | High | E2-S2 plus E4-S1 parity fixtures | OPEN |
| R2 | Express-track support is added to defaults but not preserved through display/compliance | Medium | High | E2-S2, E3-S1, E3-S2 | OPEN |
| R3 | Malformed frontmatter or traversal input escapes safe failure behavior | Low | High | E2-S3 and E4-S2 negative tests | OPEN |
| R4 | Prompt or skill layers reintroduce inline logic and drift from the script contract | Medium | Medium | E1-S2 and E3-S3 | OPEN |
| R5 | `sensing_gate_mode` is accidentally dropped from merged output | Medium | Medium | E2-S2 plus caller-focused verification in E4-S3 | OPEN |

### Closed / Reduced Planning Risks

| ID | Risk | Mitigation Applied |
| --- | --- | --- |
| R6 | FinalizePlan lacked a downstream bundle | FinalizePlan bundle now defines epics, stories, readiness, sprint status, and story files | CLOSED |
| R7 | Express packet could be mistaken for the old full-path planning set | FinalizePlan review and Epic 1 stories make the express predecessor set explicit | CLOSED |

---

## Prerequisites Checklist

### Planning prerequisites
- [x] Business plan produced
- [x] Tech plan produced
- [x] Architecture artifact produced
- [x] Sprint plan produced
- [x] ExpressPlan adversarial review completed with responses recorded
- [x] FinalizePlan review completed and preserved in staged docs
- [x] Epics defined
- [x] Stories defined
- [x] Implementation readiness recorded
- [x] Sprint tracking created

### Dev prerequisites
- [x] Active control-repo branch for the feature exists
- [x] Governance `feature.yaml` records the express path for this feature
- [ ] Temp-directory fixture harness prepared for parity tests
- [ ] Negative safety tests implemented before merge
- [ ] Final release notes capture any remaining caller audit items

---

## Implementation Surface

Primary implementation paths in this repo:

- `lens.core/_bmad/lens-work/skills/bmad-lens-constitution/SKILL.md`
- `lens.core/_bmad/lens-work/prompts/lens-constitution.prompt.md`
- `lens.core/_bmad/lens-work/skills/bmad-lens-constitution/scripts/constitution-ops.py`
- `lens.core/_bmad/lens-work/skills/bmad-lens-constitution/scripts/tests/test-constitution-ops.py`
- `lens.core/_bmad/lens-work/skills/bmad-lens-constitution/references/*.md`

No separate target repo is required. The final PR for this feature is expected to open from
`lens-dev-new-codebase-constitution` to `main` in this control repo after the implementation
stories are completed.

---

## Entry Gates By Epic

### Epic 1 entry
- Planning PR merged: yes
- Express predecessor packet recorded: yes

### Epic 2 entry
- Epic 1 story notes available: pending through story execution
- Read-only boundary acknowledged: yes

### Epic 3 entry
- Resolver core merged and testable: pending
- Merge contract and express parity implemented: pending

### Epic 4 entry
- Resolver, compliance, and display behavior implemented: pending
- Fixture harness path confirmed: pending

---

## Release Gate

The feature can be marked implementation-complete once:

1. The story set through E4-S3 is complete.
2. Partial-hierarchy, express-track, and safety regressions pass in temp-directory fixtures.
3. Prompt, skill, script, and references describe the same read-only contract.
4. Final PR notes capture any caller audit that remains post-merge.