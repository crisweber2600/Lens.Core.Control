---
feature: lens-dev-new-codebase-techplan
doc_type: expressplan-adversarial-review
status: pass-with-warnings
review_format: abc-choice-v1
goal: "Adversarial review of business-plan.md and tech-plan.md before expressplan phase completion."
updated_at: 2026-04-28T00:00:00Z
---

# Adversarial Review: lens-dev-new-codebase-techplan / expressplan

**Reviewed:** 2026-04-28T00:00:00Z  
**Source:** phase-complete  
**Overall Rating:** pass-with-warnings

## Summary

The business plan is coherent and well-scoped. It correctly identifies the continuity goal, preserves the publish-before-author contract, and stays within clean-room bounds. `tech-plan.md` had a critical structural defect at review time — two YAML frontmatter blocks producing two separate documents — which has since been corrected: the file now has a single unified frontmatter and body. The stale `Track remains \`full\`` statement in the Data Contracts section has also been removed. Five medium/high-severity findings remain documented and should be reflected in implementation planning. The phase can proceed with warnings.

**Recommended next action:** Ensure the open questions surfaced by H2 and H3 (PRD-substitute artifact for express-track businessplan; constitution dependency timeline) are tracked before implementation starts.

---

## Findings

### Critical

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| C1 | Logic Flaws | ~~`tech-plan.md` contained two YAML frontmatter blocks, producing two separate documents. Second document `depends_on` and `key_decisions` were invisible to standard tooling.~~ **RESOLVED** — documents merged, single frontmatter, best content from both retained. |  |

### High

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| H1 | Logic Flaws | ~~Doc 1 stated "Track remains `full`; this feature does not use the express path." The feature was re-declared `track: express` during this session.~~ **RESOLVED** \u2014 stale statement replaced with accurate language scoping the feature lifecycle separate from the tracks the skill supports. |  |
| H2 | Cross-Feature Dependencies | Document 2's `depends_on` lists `constitution-partial-hierarchy-fix`. The constitution feature (`lens-dev-new-codebase-constitution`) is at `phase: preplan` — early in the pipeline. If the constitution fix is not available at runtime, the new techplan skill's constitution-loading path will fail silently or with an unhelpful error. The business plan also calls this out as a prerequisite blocker but references it only in `blocks` prose, not as a tracked feature ID. | Reference `lens-dev-new-codebase-constitution` explicitly as a feature dependency in the merged frontmatter's `depends_on`. Add an open question noting the constitution is at `preplan` and must advance before end-to-end testing can pass. |
| H3 | Cross-Feature Dependencies | The `lens-dev-new-codebase-businessplan` sibling feature — which provides the businessplan command that users run before techplan — is at `expressplan-complete` on the `express` track. Expressplan produces `business-plan.md` and `tech-plan.md`, not the full-track `prd.md` and `ux-design.md` set. The tech-plan describes a `publish-to-governance --phase businessplan` hook that assumes the predecessor artifacts are the full businessplan set. This assumption may not hold if businessplan is also express-track. | Add an open question: does `publish-to-governance --phase businessplan` produce the right artifact set when businessplan is express-track? Clarify which predecessor artifact the architecture authoring must reference — the express `business-plan.md` or a full-track `prd.md`. |

### Medium / Low

| # | Severity | Dimension | Finding | Recommendation |
|---|----------|-----------|---------|----------------|
| M1 | Medium | Coverage Gaps | No error path is documented for when a user invokes `techplan` and upstream businessplan artifacts are absent. The "PRD presence and reference enforcement" check is mentioned but what happens on failure (which error, which state) is not specified. | Add a short "Error Paths" section or note in the Validation Plan. |
| M2 | Medium | Complexity and Risk | "Any registry, installer, or discovery surfaces required to expose `techplan` as part of the retained public command set" is mentioned without specifying whether a manifest update is needed. If the new codebase uses a command manifest or module-help.csv, a missing entry would leave the command undiscoverable. | Explicitly state whether `module-help.csv`, `manifest.yaml`, or any installer-hook updates are in scope; if out of scope, say so. |
| M3 | Medium | Coverage Gaps | No rollout or incremental activation strategy. The tech-plan assumes all shared dependencies exist before the feature can be tested, but does not describe what a partial installation looks like or whether the command can be installed without the full dependency chain. | Add a "Deployment Boundaries" note — can the stub be installed even if `bmad-lens-bmad-skill` is absent? If not, sequence the dependency explicitly. |
| L1 | Low | Assumptions | The business plan's `open_questions` is empty, but M1–M3 are discoverable gaps that should be tracked there. | Populate `open_questions` in the merged business-plan frontmatter. |
| L2 | Low | Coverage Gaps | The business plan's `blocks` prose references "constitution-fix prerequisites" without a feature ID. Future automation cannot query this relationship. | Replace prose with a structured feature ID reference: `lens-dev-new-codebase-constitution`. |

---

## Accepted Risks

None at this time — no risks have been explicitly accepted by the user.

---

## Party-Mode Challenge

**Winston (Architect):** The tech-plan describes `bmad-lens-techplan` as a pure conductor that delegates architecture authoring through `bmad-lens-bmad-skill`. But the businessplan sibling is express-track, meaning its governance mirror will contain `business-plan.md` and `tech-plan.md`, not `prd.md`. When the techplan conductor runs `publish-to-governance --phase businessplan` and then validates the PRD reference, what file satisfies that reference check? If `prd.md` is the hard requirement and the express track never produces one, this feature's entry gate can never pass — even when businessplan is expressplan-complete.

**John (PM):** You're building a full-track conductor but users will approach it from an express-track businessplan. Has anyone confirmed that the `express` track experience actually leads users to `lens-techplan` as the next step, or does expressplan already produce a combined business + tech plan that makes `lens-techplan` redundant for express-track users? If the command only runs for `track: full` features and most users are on express, you might be building something that very few users will ever invoke.

**Mary (Analyst):** The clean-room constraint is met. But the test plan says "Stub invokes the shared prompt-start preflight" — where does that test live and who runs it? The focused regression model used elsewhere in this codebase (`uv run --script ...tests/test-*.py`) requires a test file to exist at the path described. If the story files don't specify test locations, the developer will invent them ad hoc and break the established test-discovery pattern.

---

## Gaps You May Not Have Considered

1. **Express-track business-plan as PRD substitute** — Has the express track's `business-plan.md` been formally designated as the PRD reference for downstream phases? If not, the `architecture.must_reference(prd)` validation contract will have no satisfying file when businessplan is express-track.
2. **Module-help.csv / command discovery** — The new codebase command surface may require an entry in the module or skill manifest for the command to appear in help output. Is that in scope or deferred?
3. **Shared-dependency availability window** — `bmad-lens-bmad-skill`, `bmad-lens-git-orchestration`, and `bmad-lens-adversarial-review` are listed as dependencies, but none of them appear as Lens features with a committed delivery date. When is each expected to be available in the target project, and who owns that sequencing?
4. **Test harness location** — The tech-plan mentions validation and regression checks but doesn't name the test file paths. This leaves the implementation agent to guess, risking inconsistency with the established `tests/test-*.py` pattern.
5. **Track-mismatch user confusion** — If this feature is now `track: express` but the SKILL.md being built is described as a full-track conductor, a user of the new codebase running `lens-techplan` on an express-track feature will hit the track gate and be confused. The skill should explicitly document which tracks it supports and what it does on a mismatch.

---

## Open Questions Surfaced

1. Does `publish-to-governance --phase businessplan` produce the right artifact set when businessplan is express-track (produces `business-plan.md` not `prd.md`)? Which file satisfies the architecture `must_reference(prd)` check?
2. Is the `bmad-lens-techplan` conductor intended only for `track: full` features, or should it also serve `track: tech-change` and `track: feature` tracks?
3. When will `lens-dev-new-codebase-constitution` (currently at `preplan`) be advanced? Does any story in the delivery sequence gate on it?
4. Where do the focused regression tests live in the target project, and which CI step runs them?
5. Does the new codebase require a `module-help.csv` or manifest update to expose `techplan` as a discoverable command?
