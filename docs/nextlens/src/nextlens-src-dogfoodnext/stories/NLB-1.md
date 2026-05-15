---
feature: nextlens-src-dogfoodnext
story_id: NLB-1
doc_type: story
status: ready-for-dev
title: Skill Registration And Command Surface
depends_on: []
implementation_kind: skill-registration
epic: 1
updated_at: 2026-05-15T20:00:00Z
---

# NLB-1 - Skill Registration And Command Surface

## Goal

Create the discoverable Lens-owned NextLens bugfix skill surface in `lens.core.src`.

## Scope

- Choose one canonical skill and prompt name, expected to be `lens-nextlens-bugfix` unless dev finds an existing naming constraint.
- Add the skill entrypoint, prompt alias, manifest/help metadata, and setup or release-sync references required by Lens discovery.
- State the two boundaries in the operator-facing contract: skill source in `lens.core.src`, runtime fixes only under `TargetProjects/nextlens/src/NextLens`.

## Acceptance Criteria

- Given Lens skill discovery runs, when it lists registered Lens skills, then the NextLens bugfix skill appears exactly once with the canonical name.
- Given help output is requested, when the skill is shown, then it names the required inputs: what happened, what should have happened, and chat history.
- Given the prompt alias is invoked, when it routes to the skill, then it does not bypass Lens governance, story, or review gates.
- Given setup or release-sync validation runs, when metadata is stale or missing, then the check fails with the missing registration surface.

## Validation

- Add or update focused tests for skill registry, prompt alias, help output, and setup or release-sync metadata.
- Run the narrow Lens discovery/help validation command identified by the implementation surface.

## Dev Notes

- Do not implement runtime bugfix delegation in this story.
- Preserve existing `/lens-core-bugfix` naming and behavior.