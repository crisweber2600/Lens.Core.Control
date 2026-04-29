---
feature: lens-dev-new-codebase-trueup
doc_type: ux-design
status: approved
goal: "Document scope decision: True Up has no UI surface requiring UX design"
key_decisions:
  - True Up is a technical audit and gap-closure initiative with no end-user UI surface
  - The deliverables are code artifacts, documents, and governance records — not user-facing UI
  - Agent interaction patterns (prompt stubs, IDE discoverability) are specified in prd.md FR-1 through FR-3
  - No UX design artifact is required for this feature
open_questions: []
depends_on: [prd]
blocks: []
updated_at: 2026-04-27T00:00:00Z
---

# UX Design — True Up (lens-dev-new-codebase-trueup)

## Scope Decision: No UX Design Required

True Up is a technical audit and gap-closure initiative. Its outputs are:

- Markdown documents (parity audit report, ADRs, parity gate spec)
- Code artifacts (SKILL.md, test stubs, prompt stubs)
- Governance records (feature.yaml phase label corrections)

There is no end-user UI surface, no user journey to design, and no component specification to produce.

### Agent Interaction Surface (Covered in PRD)

The one human-interaction surface True Up touches is **IDE prompt discoverability** — specifically, the absence of `lens-switch.prompt.md`, `lens-new-feature.prompt.md`, and `lens-complete.prompt.md` from `.github/prompts/`. This is a publishing gap, not a design gap. The interaction pattern (user types `/switch`, IDE surfaces the prompt) is fully specified by the prompt stub format and does not require a separate UX design artifact.

See [prd.md](./prd.md) FR-1, FR-2, and FR-3 for the complete prompt publishing requirements.

### N/A Declaration

This `ux-design.md` exists to satisfy the `businessplan` lifecycle gate contract (`lifecycle.yaml: phases.businessplan.completion_review.reviewed_artifacts`). It is an explicit N/A declaration, not a deferred artifact.
