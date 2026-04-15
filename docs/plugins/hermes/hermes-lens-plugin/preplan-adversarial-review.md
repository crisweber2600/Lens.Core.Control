# Adversarial Review: hermes-lens-plugin / preplan

**Reviewed:** 2026-04-13T22:58:10Z
**Source:** phase-complete
**Overall Rating:** pass-with-warnings

## Summary

The preplan artifact set is coherent enough to advance into BusinessPlan. The three staged documents align on the core product direction: a general Hermes plugin that selects a Lens repo, builds an action catalog from `.github/prompts`, and executes repo-scoped Lens actions through a preview-first safety model. The main remaining gaps are not phase-blocking, but they are material enough to document before BusinessPlan: the shared session-state contract is still conceptual, the prompt metadata schema is still undefined, and the risk taxonomy needs a sharper boundary than "some mutating actions." These are BusinessPlan-worthy clarifications, not reasons to halt the feature.

## Findings

### Critical

None.

### High

None.

### Medium / Low

| # | Dimension | Severity | Finding | Recommendation |
|---|-----------|----------|---------|----------------|
| M1 | Logic Flaws | Medium | The artifacts now agree on a hybrid CLI-plus-chat model, but they do not yet define the concrete contract for how CLI helpers and tool executions resolve one shared active-project state. | In BusinessPlan, define a single session-context contract, its lifecycle, and the handoff rules between CLI selection and tool execution. |
| M2 | Coverage Gaps | Medium | The product depends on a friendly action catalog from `.github/prompts`, but the minimum optional metadata schema for intent grouping, argument prompts, and risk hints is not yet defined. | Add a brief metadata-spec section in the next phase that lists minimal supported fields and fallback behavior when fields are absent. |
| M3 | Complexity and Risk | Medium | The safety model says only some mutating actions should require confirmation, but the artifacts do not yet define how the plugin distinguishes safe versus risky Lens commands beyond general command effect. | Create an explicit risk taxonomy and preview payload before implementation planning starts. |
| L1 | Cross-Feature Dependencies | Low | There are no declared upstream feature dependencies, but the plugin's usefulness implicitly depends on Lens repos having somewhat consistent prompt naming or metadata conventions. | Note prompt consistency as an ecosystem assumption and decide whether the plugin adapts to repo variance or recommends a Lens prompt convention. |
| L2 | Assumptions and Blind Spots | Low | The initial release optimizes for user-level plugin installation, which is the best-documented path, but that choice limits repo-portable distribution in v1. | Keep user-level install as the v1 decision and revisit project-local distribution later if repo portability becomes a requirement. |

## Accepted Risks

- V1 will optimize for user-level installation under `~/.hermes/plugins/` rather than making project-local distribution first-class.
- Prompt metadata remains optional in v1, which means some actions may appear with lower confidence until repos adopt better prompt annotations.

## Party-Mode Challenge

Winston (Architect): If the CLI selects a project and the tool loop reads a different one, the product loses credibility immediately. Your next phase has to define one shared session-state contract, not two overlapping ones.

Sally (UX Designer): A friendly menu only works if the user can predict what each action means. If two prompts look similar or a prompt has weak naming, the plugin needs a graceful way to show uncertainty without becoming opaque.

Quinn (QA Engineer): "Preview first" is only trustworthy if the preview shows the real execution plan and the risk classification is stable. If the preview is vague, people will either over-confirm everything or stop trusting the gate entirely.

**Blind-Spot Challenge Questions and Responses**

1. What should own active-project state? Answer: plugin-managed session context.
2. What prompt metadata policy should v1 assume? Answer: metadata optional, inference first, metadata when present.
3. Should all file or git mutation be risky by default? Answer: no, only some mutating actions should require confirmation.
4. What install model should v1 optimize for? Answer: user-level plugin install in `~/.hermes/plugins`.

## Gaps You May Not Have Considered

1. How should the plugin detect when prompt files changed mid-session and invalidate or refresh the action catalog?
2. How should repo discovery distinguish a true Lens project from a similarly named local directory that happens to match `*.lens`?
3. What happens to the active-project state after `/new`, session resume, or background-task boundaries?

## Open Questions Surfaced

- What exact fields make up the optional prompt metadata schema?
- What exact preview payload is required for every action before execution?
- What risk classes should exist beyond a simple safe versus risky split?
- How should the action catalog represent low-confidence prompt inference to the user?
