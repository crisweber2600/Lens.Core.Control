# Sprint Change Proposal - Full Clean-Room Rewrite Reset

**Feature:** `lens-dev-new-codebase-baseline`  
**Date:** 2026-04-22  
**Mode:** Batch  
**Change scope:** Major

## 1. Issue Summary

The rewrite must now be treated as **clean-room development**. Old files, modules, prompts, and skill implementations may be used only as **verification references** for expected outcomes. They may not be copied, trimmed, mirrored, adapted in place, or treated as implementation baselines.

This change was triggered by review of the current rewrite foundation work, the current planning set, and the subsequent user decision to **reset everything** because the rewrite must be clean-room. The current project artifacts repeatedly position the old-codebase discovery corpus as the behavioral baseline for implementation, and the previously attempted implementation path reflected legacy-surface carryover. That approach conflicts with the clean-room rule and has now been explicitly rejected.

### Evidence

1. `prd.md` currently says the old-codebase discovery artifacts remain the "source baseline" / "behavioral baseline" for retained-command behavior.
2. `research.md` says every surviving command should be re-walked with the old-codebase baseline.
3. `epics.md` and `implementation-readiness.md` treat old-codebase artifacts as approved runtime/parity inputs for execution planning.
4. `architecture.md` says the new codebase structure mirrors the old one and derives retention decisions from the old-codebase call graph.
5. The repository reset includes deletion of prior completion artifacts, confirming that previously claimed progress is no longer accepted as the execution baseline.

## 2. Checklist Summary

### Section 1 - Understand the Trigger and Context

- **1.1 Done** - Triggering story: `1.1 - New Codebase Scaffold, Install Surfaces, and Module Surface Reduction`
- **1.2 Done** - Issue type: **New requirement emerged from stakeholder** and **misunderstanding of original requirements**
- **1.3 Done** - Evidence collected from PRD, Architecture, Epics, Research, Implementation Readiness, and current repository state

### Section 2 - Epic Impact Assessment

- **2.1 Done** - Epic 1 cannot be considered complete as currently planned because its foundation work permits legacy-baseline implementation thinking
- **2.2 Done** - Existing epic scope must be modified and the current story set must be reset for re-evaluation
- **2.3 Done** - Epics 2-5 are all affected because they inherit the implementation method and parity assumptions from Epic 1 artifacts
- **2.4 Done** - No current epic is automatically trusted as still valid; all stories require re-evaluation before execution
- **2.5 Done** - Priority changes are required: clean-room backlog reset and re-baselining must complete before any rewrite implementation resumes

### Section 3 - Artifact Conflict and Impact Analysis

- **3.1 Done** - PRD conflicts exist; implementation method and parity language must change
- **3.2 Done** - Architecture conflicts exist; source-of-truth and retention rationale must change
- **3.3 N/A** - No separate UX design artifact was located; command-surface UX is impacted through shell/help/discovery wording instead
- **3.4 Done** - Secondary impacts include research, implementation readiness, completion reports, and sprint status

### Section 4 - Path Forward Evaluation

- **4.1 Not viable** - Direct adjustment alone is no longer sufficient because the user has rejected the current execution baseline
- **4.2 Viable** - Full reset/re-baseline is viable and now explicitly chosen
- **4.3 Not viable** - PRD MVP reduction is not necessary; the product goal stays the same
- **4.4 Done** - Recommended path: **Full reset and re-evaluation**

### Section 5 - Sprint Change Proposal Components

- **5.1 Done**
- **5.2 Done**
- **5.3 Done**
- **5.4 Done**
- **5.5 Done**

### Section 6 - Final Review and Handoff

- **6.1 Done**
- **6.2 Done**
- **6.3 Action-needed** - Awaiting user approval
- **6.4 Action-needed** - `sprint-status.yaml` should be updated after approval
- **6.5 Action-needed** - Final handoff depends on approval

## 3. Impact Analysis

### Epic Impact

| Epic | Impact | Required change |
|---|---|---|
| Epic 1 | Directly affected | Reset stories and re-baseline around clean-room foundation requirements |
| Epic 2 | Invalidated execution plan | Re-evaluate every story from clean-room command contracts |
| Epic 3 | Invalidated execution plan | Re-evaluate bug-fix scope as newly authored work, not inherited adaptation |
| Epic 4 | Invalidated execution plan | Re-evaluate planning conductors after clean-room shared primitives are defined |
| Epic 5 | Invalidated execution plan | Re-evaluate execution/closure/maintenance stories after clean-room baseline is rebuilt |

### Story Impact

**Current triggering story**

- `Story 1.1` is no longer just a reopened foundation story; it is the entry point for backlog reset and re-baselining.

**All stories**

- Every story in Epics 1-5 must be reset from `ready-for-dev` / implied execution readiness to **re-evaluate**
- Every story must be reviewed for:
  - clean-room compliance
  - whether the story still belongs in the backlog as written
  - whether acceptance criteria define contract-first implementation rather than legacy mirroring
  - whether dependencies and sequencing still make sense after the reset
- Any story that currently claims "matches old-codebase behavior" should instead claim "satisfies rewrite contract and is verified against expected legacy outcomes"

### Artifact Conflicts

1. **PRD conflict** - old-codebase docs are treated as implementation baseline rather than verification input.
2. **Architecture conflict** - the new codebase is described as mirroring the old one; retention logic depends directly on the old call graph.
3. **Epic/story conflict** - Epic 1 and Story 1.1 do not currently encode the clean-room rule.
4. **Research conflict** - research frames the old-codebase corpus as the anchor for rewrite decomposition.
5. **Implementation readiness conflict** - readiness is overstated because the current foundation work is not clean-room compliant.
6. **Execution artifact conflict** - prior completion/reporting artifacts are no longer accepted as valid execution evidence after the reset decision.

### Technical Impact

- Any code or generated surface derived from legacy carryover must be treated as invalid for execution planning until re-evaluated.
- Existing planning docs remain valuable, but only after they are rewritten to encode clean-room constraints.
- Old-codebase discovery docs may still be consulted, but only to verify output behavior, dependency expectations, and parity after the clean-room implementation is defined from rewrite contracts.

## 4. Recommended Approach

**Selected approach:** Full reset and re-evaluation

### Why this path

The user has explicitly rejected the current execution baseline and confirmed that all code has been deleted. That makes a partial correction inappropriate. The sustainable path is:

1. Reset the current story execution state.
2. Re-evaluate all stories and dependencies under the clean-room rule.
3. Correct the planning artifacts so the implementation method is unambiguous.
4. Rebuild execution sequencing only after the revised backlog is approved.

### Option evaluation

| Option | Result |
|---|---|
| Direct Adjustment | Not viable after full reset decision |
| Potential Rollback | Viable and already effectively chosen |
| PRD MVP Review | Not needed; MVP remains intact |
| **Chosen** | **Full reset and re-evaluation** |

### Risk assessment

| Risk | Level | Notes |
|---|---|---|
| Continuing without correction | High | Would recreate the same invalid baseline immediately |
| Correcting now | Medium | Requires backlog reset, document updates, and story re-evaluation |
| Reducing MVP | Low value | Solves the wrong problem; the target surface is still valid |

## 5. Detailed Change Proposals

### 5.1 PRD updates

**File:** `docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/prd.md`

**Section:** frontmatter `key_decisions`

**OLD**

```md
- Use the old-codebase discovery deep-dive and dependency mapping as the behavioral baseline
```

**NEW**

```md
- Use the rewrite PRD, retained-command contract matrix, and architecture as the implementation source of truth
- Use old-codebase discovery artifacts only as verification references for expected outcomes and dependency coverage
- No implementation may be copied from old files, modules, prompts, or skills into the rewrite
```

**Section:** `## 1. Product Intent`

**OLD**

```md
The old-codebase discovery artifacts remain the source baseline for retained-command behavior...
```

**NEW**

```md
The rewrite is contract-first and clean-room. Retained-command behavior is defined by this PRD and supporting rewrite artifacts. Old-codebase discovery outputs may be consulted only after implementation design to verify expected outcomes, preserved contracts, and dependency coverage.
```

**Section:** `### 2.2 Rewrite Invariants`

**ADD**

```md
- The rewrite is clean-room: old files, modules, prompts, and skills are verification-only references and may not be copied into the implementation.
```

**Section:** `## 4. Acceptance Criteria`

**ADD**

```md
7. Retained-command implementations are newly authored from rewrite requirements; old-codebase artifacts are used only to verify outcome parity and dependency completeness.
```

**Rationale:** The PRD must define the implementation rule explicitly or every downstream artifact will keep treating legacy materials as design input.

### 5.2 Architecture updates

**File:** `docs/lens-dev/new-codebase/lens-dev-new-codebase-baseline/architecture.md`

**Section:** `## Overview`

**OLD**

```md
The structure of the new codebase mirrors the old one.
```

**NEW**

```md
The rewrite preserves externally observable contracts, but retained implementations are newly authored in a clean-room manner. Legacy materials may inform verification only; they do not define the implementation structure.
```

**Section:** `### 1. Module Topology`

**ADD**

```md
The topology defines required retained surfaces and contracts, not permission to preserve or trim legacy files in place. Any retained prompt, help, or agent surface must be newly authored to satisfy the rewrite contract.
```

**Section:** `### 7. Internal Skill Retention Matrix`

**OLD**

```md
This matrix ... is derived from ... the old-codebase call graph ...
```

**NEW**

```md
This matrix is derived from the retained 17-command rewrite contract and the runtime obligations required to satisfy that contract. Old-codebase discovery artifacts may be used only to verify that no required outcome or dependency was missed.
```

**Section:** release-readiness / parity language

**REPLACE**

```md
verified against the old-codebase discovery deep-dive and dependency map
```

**WITH**

```md
verified first against rewrite contracts and then checked against approved legacy reference outcomes
```

**Rationale:** The architecture must stop implying structural mirroring or call-graph derivation from the old module.

### 5.3 Epic and story updates

**Files:** `epics.md`, `stories.md`, `stories/1-1-scaffold-published-surface.md`

**Epic and backlog reset**

**ADD**

```md
All rewrite stories are reset for re-evaluation. No story is implementation-ready until it is reviewed for clean-room compliance and explicitly re-approved.
```

**Story 1.1 role change**

**REPLACE current execution assumption with**

```md
Story 1.1 becomes the backlog reset and clean-room foundation story. It defines the criteria used to re-approve all later rewrite stories.
```

**All stories status handling**

**ADD**

```md
- [ ] Reset status to re-evaluate
- [ ] Re-check dependencies and sequencing
- [ ] Rewrite acceptance criteria where they imply legacy-baseline implementation
- [ ] Re-approve story only after clean-room compliance is explicit
```

**Sprint status implication**

**ADD**

```md
No story in Epics 1-5 may be treated as ready-for-dev until the reset review is complete and the backlog is re-approved.
```

**Rationale:** The user requested a full reset, not just a targeted correction.

### 5.4 Research updates

**File:** `research.md`

**Section:** `### 1.4 Related Discovery Inputs`

**OLD**

```md
... should anchor the rewrite's retained-command mapping work ...
```

**NEW**

```md
... are approved verification references for retained-command outcome checks and dependency coverage audits ...
```

**Section:** follow-on explanatory paragraph

**REPLACE**

```md
every surviving published command should be re-walked with that old-codebase baseline
```

**WITH**

```md
every surviving published command should be specified from the rewrite contract first, then reviewed against approved legacy references to verify expected behavior, dependencies, outputs, and validation anchors
```

**Rationale:** Research should support verification, not authorize legacy-derived implementation.

### 5.5 Implementation readiness updates

**File:** `implementation-readiness.md`

**Section:** frontmatter `key_decisions`

**OLD**

```md
- "Carry forward old-codebase discovery docs as parity references for runtime behavior and dependency validation."
```

**NEW**

```md
- "Treat old-codebase discovery docs as verification-only references; implementation design must remain clean-room."
```

**Section:** `### Overall Status`

**OLD**

```md
Overall Status: Conditionally Ready
```

**NEW**

```md
Overall Status: Not Ready
```

**Section:** rationale

**ADD**

```md
Foundation readiness is blocked until the entire story set is reset and re-evaluated under the clean-room rule.
```

**Rationale:** The project should not enter `/dev` while the implementation method is still mis-specified.

## 6. High-Level Action Plan

1. Update PRD, Architecture, Epics, Stories, Research, and Implementation Readiness to encode the clean-room rule.
2. Reset sprint/backlog status so all stories move to re-evaluate rather than ready-for-dev.
3. Re-assess every story for scope, dependencies, and clean-room acceptance criteria.
4. Rebuild the approved execution sequence only after backlog re-approval.
5. Resume implementation only from newly approved clean-room stories.

## 7. MVP Impact

**MVP scope:** unchanged  
**MVP method:** changed materially

The retained 17-command rewrite remains the MVP. What changes is the allowed implementation method: clean-room design is now mandatory. This increases foundation rigor but does not reduce the intended product scope.

## 8. Handoff Plan

### Scope classification

**Major**

### Recommended handoff recipients

| Role | Responsibility |
|---|---|
| Product Manager / Architect | Approve the clean-room rule as a contract change across PRD and Architecture |
| Scrum Master / Product Owner | Reset the backlog, reclassify story readiness, and update sprint status |
| Development team | Do not resume implementation until revised stories are approved |
| QA / Review | Re-review revised stories and later verify clean-room outcomes using verification-only legacy references |

## 9. Success Criteria for the Correction

The correction is complete when:

1. Core planning artifacts explicitly define the rewrite as clean-room.
2. All stories are reset and re-evaluated before implementation resumes.
3. No story remains `ready-for-dev` without explicit clean-room acceptance criteria.
4. Canonical surfaces are reintroduced only through approved clean-room stories.
5. Legacy materials appear only in verification evidence, not as implementation baselines.

## 10. Approval Status

**Status:** Approved

**Approval outcome:** Reset everything. All stories are to be reset and re-evaluated before implementation resumes.
