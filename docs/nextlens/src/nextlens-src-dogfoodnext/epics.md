---
feature: nextlens-src-dogfoodnext
doc_type: epics
status: approved
track: express
phase: finalizeplan
depends_on: [business-plan, tech-plan, sprint-plan]
blocks: []
updated_at: 2026-05-15T20:00:00Z
inputDocuments:
  - business-plan.md
  - tech-plan.md
  - sprint-plan.md
  - expressplan-adversarial-review.md
  - finalizeplan-review.md
---

# Epics - NextLens Bugfix Skill

The feature delivers a Lens-owned NextLens bugfix conductor. The skill source is authored in `lens.core.src`, while runtime bugfix implementation is constrained to `TargetProjects/nextlens/src/NextLens`. The conductor mirrors the proven `/lens-core-bugfix` flow: structured intake, stable slug, namespaced bug artifact, fresh branch, bounded delegation, PR recording, validation evidence, and stateful closeout.

## Epic 1 - Skill Surface And Intake Contracts

**Goal:** Establish the discoverable skill surface and normalize chat-history-driven bug reports into durable, minimized intake records.

**Stories:**

- NLB-1 - Skill Registration And Command Surface
- NLB-2 - Intake Schema And Transcript Minimization

**Done when:**

- A single canonical skill name is registered across skill folder, prompt alias, manifest/help metadata, and setup or release-sync validation.
- The intake parser requires what happened, what should have happened, and chat history, then emits the fields needed by the bug reporter without persisting raw transcripts by default.

## Epic 2 - Bug Artifact And Context Resolution

**Goal:** Create a namespace-aware bug record and resolve the design and target roots without current-working-directory assumptions.

**Stories:**

- NLB-3 - Namespaced Bug Artifact Operations
- NLB-4 - Design Context And Target Resolver

**Done when:**

- `bugs/nextlens/{New|QuickDev|Inprogress|Fixed}/{slug}.md` operations preserve slug idempotency, duplicate lookup, PR recording, closeout, and existing Lens core bug behavior.
- The skill resolves `docs/nextlens/src`, `lens.core.src`, and `TargetProjects/nextlens/src/NextLens` from approved configuration or feature metadata and rejects escaped overrides.

## Epic 3 - Conductor Delegation And Closure Evidence

**Goal:** Generate a deterministic fix specification, enforce write boundaries, validate with NextLens Doctor, and close the namespaced bug only after PR evidence exists.

**Stories:**

- NLB-5 - Fix Specification Generation
- NLB-6 - Fresh Branch Delegation And Boundary Enforcement
- NLB-7 - Validation, PR Recording, And Closeout
- NLB-8 - End-To-End Tests And Documentation

**Done when:**

- The conductor derives a fresh branch from the bug slug and blocks no-op completion, branch reuse for unrelated bugs, missing commits, and writes outside `TargetProjects/nextlens/src/NextLens`.
- PR URL and validation evidence are recorded on the namespaced bug artifact before it moves to `Fixed`.
- Regression tests prove existing `bugs/QuickDev` Lens core behavior still works after adding the NextLens namespace.

## Delivery Notes

- Stories should be implemented in dependency order unless a dev conductor explicitly parallelizes non-overlapping tests and documentation.
- NLB-3 and NLB-7 are the highest-risk stories because they touch operational bug state and closeout behavior.
- NLB-8 is the final closure story and should not be started until the conductor path has a realistic fixture covering create, branch, delegate, PR record, validation, and closeout.