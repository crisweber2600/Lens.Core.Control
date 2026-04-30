---
feature: lens-dev-new-codebase-expressplan
doc_type: story-file
story_id: 1.1
status: ready-for-dev
goal: "Keep /expressplan discoverable and guarded by light preflight before any workflow logic runs"
depends_on: []
updated_at: 2026-04-27T22:50:00Z
---

# Story 1.1 - Prompt Routing and Light Preflight

## User Story

As a Lens operator, I want `/expressplan` to run light preflight before loading workflow logic so bootstrap failures stop immediately and clearly.

## Acceptance Criteria

- The stub prompt runs `light-preflight.py` from the workspace root.
- A non-zero preflight exit stops the command and surfaces the failure.
- Successful preflight loads the release prompt and no other prompt-local logic.
- Help or module discovery still exposes the command.

## Implementation Notes

- Touch only the prompt/help surfaces needed for discoverability and delegation.
- Keep orchestration details in the skill, not the prompt stub.
- Do not add direct governance writes to the prompt path.

## Validation Targets

- Narrow prompt-surface check
- Preflight fail-fast behavior
- Help/module discoverability check