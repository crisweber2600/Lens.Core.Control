---
feature: lens-dev-new-codebase-preplan
story_id: PP-2.3
epic: PP-E2
title: Implement research and product-brief delegation
estimate: S
sprint: 2
status: not-started
depends_on: [PP-2.2]
blocks: [PP-3.1]
updated_at: 2026-04-28T00:00:00Z
---

# PP-2.3 — Implement research and product-brief delegation

## Story

**As a** Lens user after brainstorming completes,  
**I want** the conductor to offer research and product-brief authoring through canonical BMAD wrappers,  
**so that** all authoring goes through Lens-governed wrappers with no conductor-owned file generation.

## Context

After `brainstorm.md` exists, the conductor enters Phase B and offers research and product-brief authoring. The rules:

- **Research routing:** Use the *narrowest applicable canonical wrapper identifier* at runtime:
  - `bmad-domain-research` for domain/industry research
  - `bmad-market-research` for competitive/market research
  - `bmad-technical-research` for technology/architecture research
  
  These exact names must be used consistently in both implementation and tests. No shorthand aliases (e.g., "research-wrapper", "bmad-research").

- **Product-brief routing:** Always routes through `bmad-product-brief` via `bmad-lens-bmad-skill`.

- **No direct authoring:** The conductor does not write any files directly. Every artifact is produced by the delegated wrapper.

- **Ambiguous research scope:** If the feature scope makes the narrowest wrapper ambiguous, the conductor asks the user for clarification before selecting — it does not auto-select silently.

## Implementation Target

`TargetProjects/lens-dev/new-codebase/lens.core.src/`

## Acceptance Criteria

- [ ] After `brainstorm.md` exists, conductor offers research and product-brief authoring steps.
- [ ] Research routing uses exact canonical wrapper names: `bmad-domain-research`, `bmad-market-research`, or `bmad-technical-research` (narrowest applicable selected at runtime).
- [ ] Product brief routes through `bmad-product-brief` via `bmad-lens-bmad-skill`.
- [ ] Conductor does not write any artifact files directly (no file write operations in SKILL.md logic for this phase).
- [ ] When research scope is ambiguous, user clarification is requested before wrapper selection.
- [ ] No parity tests from PP-1.3 or PP-2.2 regress.

## Definition of Done

- Research and product-brief delegation merged to feature branch.
- No regressions in any existing parity tests.
- PR merged.
