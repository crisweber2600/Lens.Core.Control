---
feature: lens-dev-new-codebase-preplan
story_id: PP-4.2
epic: PP-E4
title: Align help surfaces and resolve target-repo scope
estimate: S
sprint: 4
status: not-started
depends_on: [PP-4.1]
blocks: []
updated_at: 2026-04-28T00:00:00Z
---

# PP-4.2 — Align help surfaces and resolve target-repo scope

## Story

**As a** Lens user discovering available commands,  
**I want** `preplan` to appear consistently in help surfaces and module manifests,  
**so that** the command is discoverable and its position in the 17-command surface is official.

## Context

Two items remain after phase completion is implemented:

**1. Help surface alignment**

`module-help.csv` and `lens.agent.md` in `lens.core.src` must include `preplan` in the 17-command surface. There may be a broader 17-command surface alignment sweep owned by a separate feature — in that case, this story documents the delegation and records the reference, but does not block on it.

**2. Target-repo interruption-and-resume scope decision**

The open question from the business plan and tech plan: should `bmad-lens-target-repo` interruption-and-resume within preplan be implemented in this delivery slice?

This story closes that question with a documented decision:
- **Option In-Scope:** Add PP-4.3 story to this feature's backlog with a defined implementation scope.
- **Option Deferred:** Add a written deferral note to the out-of-scope section of the business plan or tech plan, naming the follow-up feature or issue where this work will be tracked.

## Acceptance Criteria

- [ ] One of the following is true for `module-help.csv`: (a) it includes `preplan` in the 17-command surface in `lens.core.src`, OR (b) a written note documents that this is owned by a separate feature alignment sweep with a reference to that feature or issue.
- [ ] One of the following is true for `lens.agent.md`: same condition as above.
- [ ] A documented scope decision exists for `bmad-lens-target-repo` interruption-and-resume: either a PP-4.3 story is added with defined scope, OR a deferral note is added to the planning docs with a named follow-up reference.
- [ ] No parity tests regress.
- [ ] Feature-level Definition of Done is met (see `implementation-readiness.md`).

## Definition of Done

- Story closed with all three decision items documented.
- Feature-level DoD confirmed met.
- Final PR merged (or already merged from PP-4.1 if this story shares the sprint PR).
