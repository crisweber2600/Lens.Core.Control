---
feature: lens-dev-new-codebase-complete
doc_type: story-file
story_id: CP-3
status: ready-for-dev
goal: "Confirm /complete remains discoverable in prompt and help surfaces after the rewrite"
depends_on: [CP-1]
updated_at: 2026-04-27T22:55:00Z
---

# Story CP-3 - Prompt and Help Surface Checks

## User Story

As a Lens operator, I want `/complete` to remain discoverable in help surfaces so I can find the closure command without reading source code.

## Acceptance Criteria

- The published prompt stub for `/complete` exists at the expected path and delegates to the release path.
- `/complete` appears in the module help or command discovery surface.
- No prompt stub embeds workflow logic — all orchestration belongs to `bmad-lens-complete`.
- No help text references `final-summary.md` (only `summary.md`).

## Implementation Notes

- This is a static-check or narrow test story, not a logic implementation.
- Check the path: `_bmad/lens-work/prompts/lens-complete.prompt.md` (or equivalent published stub).
- Verify the module help CSV or surface includes `complete` as a listed command.

## Validation Targets

- Stub prompt path exists and contains a delegation directive.
- Module help surface lists `/complete`.
- No legacy naming references in help text.
