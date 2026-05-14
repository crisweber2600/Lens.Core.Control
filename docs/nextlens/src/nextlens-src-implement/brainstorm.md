---
feature: nextlens-src-implement
doc_type: brainstorm
status: draft
track: full
source_material:
  - docs/nextlens/src/rawNotes/Reimagine.md
  - docs/nextlens/src/rawNotes/TopDown.md
  - docs/nextlens/src/nextlens-src-topdownlens/
updated_at: 2026-05-14T00:00:00Z
---

# Brainstorm - nextlens-src-implement

## Purpose

Use the completed TopDownLens planning, implementation notes, examples, schemas, guides, dogfood outputs, and raw NextLens notes as source material for the next implementation feature. This brainstorm treats `nextlens-src-topdownlens` as completed context rather than a plan to repeat.

The central question is: what is the smallest useful `nextlens-src-implement` slice that proves NextLens can turn the TopDownLens contracts into a real runtime workflow while staying inside Lens governance boundaries?

## Source Material Used

- Raw notes under `docs/nextlens/src/rawNotes/`, especially the top-down and reimagine notes.
- TopDownLens planning artifacts: business plan, tech plan, sprint plan, epics, stories, readiness, reviews, status, dev-session, and retrospective.
- Story files TL-1 through TL-12.
- Guides for top-down discovery, bottom-up compatibility, BMAD bridge, Salmon signals, self-hosting topology, constitution layering, doctor checks, bugfix flow, and CI pipelines.
- Schemas and examples for ontology, landscape entities, relationships, derived graph, BMAD packet, and Salmon signals.
- Dogfood walkthrough, report, packet, Salmon signal, derived graph, freshness, workflow templates, and topology notes.

## Analyst Frame

TopDownLens establishes a planning and context model where BMAD remains the execution engine and NextLens supplies coherence: landscape memory, traceability, impact analysis, feature selection, graph traversal, doctor checks, and Salmon feedback. The strongest architecture pattern is separation between durable archive/history, living human interpretation, and rebuildable derived graph outputs.

The implementation feature should avoid expanding into every future NextLens idea. Auspex, full automated extraction, dedicated self-hosting repos, and a complete command taxonomy are important but can swamp the first runtime slice. The first implementation should prove a narrow loop: load or validate landscape material, rebuild/read graph state, select exactly one feature or slice, generate a BMAD packet, run doctor checks, and record one Salmon signal.

Known constraints and tensions:

- Stable IDs are durable; file paths are mutable addresses.
- Derived graph files must be marked rebuildable and disposable.
- BMAD packets must prevent scope expansion beyond one selected feature or slice.
- Salmon is upstream consistency validation, not notification plumbing.
- Story/status truth has drift in the source set and needs a single validation posture.
- Release verification is deferred because the eventual release surface does not exist yet.
- `feature` is the schema term, while `slice` is a stronger operator term in the raw notes.
- Output-root and target-repo expectations are inconsistent across readiness notes and later dev evidence.

## Divergent Ideas

### Technical Runtime

1. Build a schema validation command that validates all TopDownLens contracts and examples before any graph or packet command runs.
2. Build a graph rebuild command that reads landscape entities and relationships, then writes a derived graph with freshness metadata.
3. Build a graph inspect command that answers one-hop questions such as "what stories support this capability?"
4. Build a packet generator that produces exactly one BMAD packet from one selected feature or slice ID.
5. Build a packet validator that rejects missing traceability, multiple selected features, or unbounded include scopes.
6. Build a Salmon recorder that writes a signal with severity, target entity, evidence, and routing recommendation.
7. Build a freshness checker that warns when derived graph timestamps are older than their source files.
8. Build a status drift checker that compares story frontmatter, sprint status, dev-session, and retrospective claims.
9. Build a relationship validator that rejects edges whose source or target IDs are absent from landscape entities.
10. Build a minimal repository adapter that can run over the control docs tree without requiring new self-hosted repos.

### CLI And Workflow

11. Provide one `nextlens doctor` entry point that runs schemas, graph freshness, status drift, and packet sanity checks.
12. Provide one `nextlens graph rebuild` command and keep all graph outputs under a derived directory.
13. Provide one `nextlens packet prepare --feature <id>` command that emits a BMAD-ready packet.
14. Provide one `nextlens salmon raise --target <id> --severity <level>` command for manual evidence capture.
15. Add a dry-run mode for every command so planning docs can prove behavior before implementation repos mature.
16. Add a `--docs-root` argument so incubation can run against `docs/nextlens/src/...` without hardcoding paths.
17. Add a `--strict` flag that turns advisory findings into hard failures for CI or release gates.
18. Add machine-readable JSON output for every command so Lens workflows can consume results.
19. Add human-readable summaries that explain why a packet or graph failed validation.
20. Keep command names provisional and isolate them behind a small command registry.

### Governance Safety

21. Fail closed when a command tries to write outside the resolved docs path or target repo.
22. Treat governance publication as out of scope for runtime commands unless routed through Lens orchestration.
23. Make derived graph files explicitly derived and rebuildable in both content and command output.
24. Preserve feature.yaml as lifecycle authority and avoid writing feature state from NextLens runtime commands.
25. Record source documents in generated outputs so review can trace packet and graph decisions.
26. Require explicit operator intent before any Salmon signal blocks promotion or suggests correct-course.
27. Keep release-surface checks advisory until `nextlens-release` exists.
28. Separate validation of contracts from validation of lifecycle phase gates.
29. Make every command safe for a dirty worktree by reporting state rather than auto-mutating it.
30. Provide negative fixtures for forbidden governance, release, and `.github` writes.

### UX And Operator Flow

31. Use `slice` in operator-facing text while preserving `feature` IDs in persisted schemas for compatibility.
32. Present packet preparation as "choose one slice" rather than "build a full plan."
33. Show include, exclude, and guardrail sections side by side so scope boundaries are visible.
34. Show graph freshness as a simple status line before deeper diagnostic details.
35. Show Salmon severity as local note, landscape update, correct-course candidate, or promotion blocker.
36. Provide a short "why this packet is safe for BMAD" explanation after packet validation.
37. Provide a "what changed upstream" view for Salmon signals that affect capability, journey, or outcome records.
38. Keep bottom-up promotion language cautious: "candidate adjacency" before "new product area."
39. Make unresolved source drift visible instead of hiding it behind pass/fail summaries.
40. Avoid asking users to understand all hierarchy levels before they can run one useful command.

### Testing And Quality

41. Turn the existing examples into golden fixtures for schema, packet, graph, and Salmon validation.
42. Add one negative fixture per contract: missing ID, broken relationship, stale graph, multi-feature packet, invalid severity.
43. Add tests proving paths can move while ID references still resolve.
44. Add tests proving derived graph output can be deleted and rebuilt from source material.
45. Add tests for status drift across story frontmatter and sprint/dev session records.
46. Add tests for Windows path handling because the control repo runs on Windows paths.
47. Add tests that command JSON output stays stable enough for Lens orchestration.
48. Add CI docs first, then wire workflows only when the implementation target is confirmed.
49. Add a dogfood test that rebuilds graph and prepares a BMAD packet from the current TopDownLens docs.
50. Add release-deferred tests that keep missing release surfaces visible but nonblocking.

### Future Visibility

51. Treat Auspex as a read-only dashboard layer over graph, packet, evidence, freshness, risk, and BMAD progress.
52. Design command JSON so Auspex can consume it later without scraping markdown.
53. Keep stakeholder-facing views separate from lifecycle mutation commands.
54. Preserve enough provenance for a future "why are we building this?" trace from task to outcome.
55. Add hooks for portfolio-level sensing only after single-feature runtime behavior is trustworthy.

## Convergent Themes

- The first runtime should prove an end-to-end loop rather than a broad platform surface.
- Validation should come before generation: schemas, IDs, relationships, freshness, and status consistency need to fail clearly.
- BMAD packet preparation is the best bridge between TopDownLens planning and BMAD execution.
- Salmon should be implemented as evidence-backed consistency feedback with explicit routing, not as a generic message log.
- Governance boundaries are part of the product: the runtime must show it can operate without bypassing Lens lifecycle rules.
- Existing source drift is not just cleanup; it is a valuable first doctor-check use case.

## Candidate Implementation Slices

### Slice A - Contract Doctor First

Build a `doctor` workflow that validates schemas, examples, relationships, derived graph freshness, and status drift. This proves safety and gives immediate feedback on the current TopDownLens source set.

Why it is attractive:

- Low ambiguity.
- Strong fit with existing schemas and examples.
- Turns known drift into useful product feedback.
- Provides a foundation for CI and release gates.

Risk:

- It proves correctness more than workflow value unless paired with packet or Salmon output.

### Slice B - BMAD Packet Bridge First

Build packet preparation and validation for exactly one selected feature or slice. This proves the core TopDownLens promise: choose a bounded unit of work and hand it to BMAD without scope expansion.

Why it is attractive:

- Closest to user-visible value.
- Exercises hierarchy, guardrails, traceability, and acceptance evidence.
- Keeps BMAD as the execution engine.

Risk:

- Needs enough graph/entity resolution to avoid becoming a hand-authored markdown exporter.

### Slice C - Graph Rebuild First

Build a derived graph rebuild/read command from landscape entities and relationships, with freshness output. This proves the data model and supports packet generation later.

Why it is attractive:

- Directly implements the three-truth architecture.
- Makes derived/rebuildable behavior concrete.
- Provides reusable substrate for doctor, packet, and Auspex.

Risk:

- Can feel invisible unless paired with inspect output or a packet demo.

### Slice D - Salmon Feedback First

Build manual Salmon signal creation and validation with routing recommendations. This proves recursive upstream consistency feedback and gives a path for deferred release verification signals.

Why it is attractive:

- Distinctive NextLens capability.
- Uses existing severity examples.
- Connects dogfood warnings to a durable mechanism.

Risk:

- Without graph and packet context, routing may feel arbitrary.

### Slice E - Thin End-To-End Demo

Build a narrow demo that validates source contracts, rebuilds the graph, prepares one packet, runs doctor checks, and records one Salmon signal over the existing TopDownLens docs.

Why it is attractive:

- Proves the whole story.
- Uses the current docs as real fixture material.
- Produces strong business-plan and tech-plan evidence.

Risk:

- Must be aggressively scoped to avoid implementing every future command.

## Open Questions

1. Is the implementation target `TargetProjects/nextlens/src/NextLens`, `Lens.Core.Control`, or a new self-hosted NextLens repo surface?
2. Should persisted contracts keep `feature` while operator UX says `slice`, or should schemas introduce `slice` now?
3. Should the first output root be the feature docs path, `_bmad-output/lens`, or a migration bridge between both?
4. Which command names are stable enough for implementation stories, and which should remain provisional?
5. Which Salmon severities block lifecycle promotion in this feature, and which remain advisory?
6. Should release verification remain explicitly deferred or become a nonblocking doctor finding?
7. Which source drift should the first doctor check treat as expected fixture data versus actual failure?
8. How much of packet preparation should be automatic versus operator-confirmed?
9. What is the minimum evidence needed before bottom-up adjacency becomes a promotion candidate?
10. Should Auspex be mentioned only as future-facing architecture, or should JSON output be shaped now for later Auspex consumption?

## Recommended Next Research

- Confirm the authoritative implementation target and write boundary for `nextlens-src-implement`.
- Inspect the existing `TargetProjects/nextlens/src/NextLens` surface, if present, to see what runtime or package skeleton already exists.
- Compare command names in the guides against any existing scripts or CLI entry points.
- Decide whether `feature` versus `slice` is a schema decision, a UX decision, or both.
- Identify which TopDownLens examples should become golden fixtures for the first implementation tests.
- Define the first pass/fail contract for the doctor command, especially around known status drift and deferred release verification.