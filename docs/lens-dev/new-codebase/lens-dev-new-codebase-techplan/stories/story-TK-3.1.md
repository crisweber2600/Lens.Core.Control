---
feature: lens-dev-new-codebase-techplan
story_id: "TK-3.1"
doc_type: story
status: complete
title: "Deliver Shared Utility Surfaces"
priority: P0
story_points: 5
epic: "Epic 3 — Shared Utility Delivery and Parity"
depends_on: ["TK-2.1"]
blocks: ["TK-3.2"]
updated_at: 2026-04-29T00:00:00Z
---

# Story TK-3.1 — Deliver Shared Utility Surfaces

**Feature:** `lens-dev-new-codebase-techplan`  
**Epic:** 3 — Shared Utility Delivery and Parity  
**Priority:** P0 | **Points:** 5 | **Status:** not-started

---

## Goal

Implement the four shared utility surfaces that `bmad-lens-techplan` depends on but cannot create inline: the publish hook, the BMAD wrapper routing, the adversarial review gate, and the constitution loader.

---

## Context

The conductor skill in TK-2.3 documents these dependencies with visible `[TK-3.1]` prerequisite markers. This story satisfies those dependencies. Implementation is authoritative for this feature — sibling features (expressplan, finalizeplan) consume these surfaces; they do not re-implement them.

**F3 sequencing note (from expressplan-adversarial-review.md):** `lens-dev-new-codebase-techplan` owns these shared utilities. If a sibling feature attempts to ship conflicting implementations, the conflict must be surfaced as a blocker before that sibling's dev-complete gate. This story's completion sets the authoritative versions.

**Clean-room rule:** Implementation derived from baseline rewrite docs only. No old-codebase utility prose reproduced.

**Abandon condition:** If TK-2.3 is not marked complete within 3 business days of this story becoming unblocked (i.e., TK-2.1 in-progress or complete), escalate to the feature lead. If the conductor skill cannot be successfully invoked via the stub after 5 business days, abandon Epic 3 and file a blocker in governance — do not continue accumulating debt in TK-3.1.

---

## Acceptance Criteria

**Publish Hook:**
- [ ] A publish hook (or equivalent mechanism) is available to the conductor skill and executes `bmad-lens-git-orchestration publish-to-governance --phase businessplan` before architecture authoring begins.
- [ ] The hook exits non-zero and stops the skill if governance is not up-to-date.

**BMAD Wrapper Routing (`bmad-lens-bmad-skill`):**
- [ ] The BMAD wrapper is callable from `bmad-lens-techplan/SKILL.md` and routes architecture authoring requests to the correct BMAD agent.
- [ ] The wrapper does not duplicate agent logic — it wraps and delegates only.

**Adversarial Review Gate:**
- [ ] An adversarial review gate is available to `bmad-lens-techplan/SKILL.md` and enforces that the `expressplan-adversarial-review.md` artifact is present and in `status: responses-recorded` before architecture can be finalized.
- [ ] The gate is callable as a shared utility, not duplicated inline.

**Constitution Loader:**
- [ ] A constitution loader is callable from `bmad-lens-techplan/SKILL.md` and resolves the applicable constitution for the current feature's domain/service context.
- [ ] The loader follows the `bmad-lens-constitution` skill resolution chain (domain → global fallback).

**Integration:**
- [ ] TK-2.3's `bmad-lens-techplan/SKILL.md` is updated to activate (un-stub) the four dependency hooks.
- [ ] The `[TK-3.1 pending]` markers in TK-2.3's SKILL.md are replaced with active wiring.

---

## Dev Notes

- Check existing `lens.core/_bmad/lens-work/skills/` for any partial implementations of these surfaces that can be re-used.
- When wiring the adversarial review gate, the gate's behavior is: presence check + status field equality check. It is not a content reviewer.
- For the constitution loader, the domain/service path is: `lens-dev/new-codebase` → constitution at `TargetProjects/lens/lens-governance/constitutions/`.
- After completing this story, remove or update TK-3.1 skip annotations in TK-2.5 test file.

---

## Dev Agent Record

### Agent Model Used

### Debug Log References

### Completion Notes List

### File List

- (Shared utility files — to be confirmed during implementation)
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md` (updated)
