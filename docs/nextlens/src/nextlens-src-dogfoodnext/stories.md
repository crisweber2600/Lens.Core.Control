---
feature: nextlens-src-dogfoodnext
doc_type: stories
status: approved
track: express
phase: finalizeplan
depends_on: [epics]
blocks: []
updated_at: 2026-05-15T20:00:00Z
inputDocuments:
  - business-plan.md
  - tech-plan.md
  - sprint-plan.md
  - expressplan-adversarial-review.md
  - finalizeplan-review.md
---

# Stories Index - NextLens Bugfix Skill

Per-story files live in `stories/`. Each story carries the source/target boundary, acceptance criteria, validation expectations, and dependencies needed by `/dev`.

| Story | Title | Epic | Kind | Depends On |
|-------|-------|------|------|------------|
| [NLB-1](stories/NLB-1.md) | Skill Registration And Command Surface | 1 | skill-registration | (root) |
| [NLB-2](stories/NLB-2.md) | Intake Schema And Transcript Minimization | 1 | parser | NLB-1 |
| [NLB-3](stories/NLB-3.md) | Namespaced Bug Artifact Operations | 2 | bug-state | NLB-2 |
| [NLB-4](stories/NLB-4.md) | Design Context And Target Resolver | 2 | resolver | NLB-1 |
| [NLB-5](stories/NLB-5.md) | Fix Specification Generation | 3 | specification | NLB-2, NLB-3, NLB-4 |
| [NLB-6](stories/NLB-6.md) | Fresh Branch Delegation And Boundary Enforcement | 3 | conductor | NLB-3, NLB-4, NLB-5 |
| [NLB-7](stories/NLB-7.md) | Validation, PR Recording, And Closeout | 3 | closeout | NLB-3, NLB-6 |
| [NLB-8](stories/NLB-8.md) | End-To-End Tests And Documentation | 3 | validation-docs | NLB-1, NLB-2, NLB-3, NLB-4, NLB-5, NLB-6, NLB-7 |

## Suggested Sprint Order

1. NLB-1
2. NLB-2
3. NLB-3
4. NLB-4
5. NLB-5
6. NLB-6
7. NLB-7
8. NLB-8

## Shared Constraints

- Skill source boundary: `lens.core.src`.
- Runtime fix boundary: `TargetProjects/nextlens/src/NextLens`.
- Design context root: `docs/nextlens/src`.
- Bug namespace: `bugs/nextlens/{New|QuickDev|Inprogress|Fixed}/{slug}.md`.
- No direct governance docs mirror writes; governance updates must use approved Lens operations.
- Durable artifacts summarize chat evidence and use references by default instead of copying raw transcripts.