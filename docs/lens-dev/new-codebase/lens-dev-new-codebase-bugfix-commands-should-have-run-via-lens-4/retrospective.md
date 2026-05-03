---
feature: lens-dev-new-codebase-bugfix-commands-should-have-run-via-lens-4
doc_type: retrospective
status: approved
track: express
updated_at: '2026-05-03T23:20:00Z'
---

# Retrospective: Commands Should Have Run Via Lens 4F788235

## What Went Well

- FinalizePlan and Dev were completed end-to-end for the bugfix feature.
- A clean-room source branch was created from `develop` and the fix was reimplemented without cherry-picking.
- The core ambiguity was removed: expressplan delegation now explicitly loads SKILL.md and forbids shell-command interpretation.
- Source PR was opened and linked back into governance.

## What Didn’t Go Well

- Lifecycle completion was blocked because `/lens-complete` runtime script is not yet implemented in this installed module version.
- `feature-index.yaml` status drifted from runtime phase updates and required explicit manual reconciliation.

## Key Learnings

- Explicit SKILL.md load instructions are necessary in orchestration steps whenever a similarly named shell command could be inferred.
- Governance synchronization checks should run as part of every phase transition to prevent index/state drift.

## Action Items

- [ ] Implement `lens-complete/scripts/complete-ops.py` in the `lens-dev-new-codebase-complete` feature.
- [ ] Add a lightweight validator to flag phase/index mismatch before archive operations.
